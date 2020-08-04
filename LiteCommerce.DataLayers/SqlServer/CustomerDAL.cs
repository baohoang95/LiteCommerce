using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace LiteCommerce.DataLayers.SqlServer
{
    /// <summary>
    /// Giao tiếp của Customer với CSDL
    /// </summary>
    public class CustomerDAL : ICustomerDAL
    {
        private string connectionString;

        public CustomerDAL (string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Thêm mới 1 Customer , Trả về id
        /// </summary>
        /// <param name="data">Kieu Doi tuong</param>
        /// <returns></returns>
        public string Add(Customer data)
        {
            string customerId = "";
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Customers
                                          (
                                              CustomerID,
	                                          CompanyName,
	                                          ContactName,
	                                          ContactTitle,
	                                          Address,
	                                          City,
	                                          Country,
	                                          Phone,
	                                          Fax
                                          )
                                          VALUES
                                          (
                                              @CustomerID,
	                                          @CompanyName,
	                                          @ContactName,
	                                          @ContactTitle,
	                                          @Address,
	                                          @City,
	                                          @Country,
	                                          @Phone,
	                                          @Fax
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
                cmd.Parameters.AddWithValue("@CompanyName", data.CompanyName);
                cmd.Parameters.AddWithValue("@ContactName", data.ContactName);
                cmd.Parameters.AddWithValue("@ContactTitle", data.ContactTitle);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@Phone", data.Phone);
                cmd.Parameters.AddWithValue("@Fax", data.Fax);
           

                customerId = Convert.ToString(cmd.ExecuteScalar());

                connection.Close();
            }

            return customerId;
        }

        /// <summary>
        /// Đếm số lượng Customer thỏa mãn điều kiện 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public int Count(string searchValue , string  Country , string address)
        {

            int count = 0;
            //nếu searchValue empty thì thêm tìm kiếm tương đối  %searchValue%
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }

            if (!string.IsNullOrEmpty(Country))
            {
                Country = "%" + Country + "%";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select COUNT(*) from Customers      
                                    where (@searchValue = N'' or ContactName like @searchValue) 
                                        and (@Country = N'' or Country like @Country) 
                                        and  (@address = N''  or Address like @address) ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@Country", Country);
                cmd.Parameters.AddWithValue("@address", address);

                //ExecuteScalar : tra ve 1 gia tri
                count = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return count;
        }

        /// <summary>
        /// Xoa Nhieu Customers
        /// </summary>
        /// <param name="customerIDs"></param>
        /// <returns></returns>
        public int Delete(string[] customerIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Customers
                                            WHERE(CustomerID = @CustomerId)
                                              AND(CustomerID NOT IN(SELECT CustomerID FROM Orders))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;

                cmd.Parameters.Add("@customerId", SqlDbType.Char);
                foreach (string customerId in customerIDs)
                {
                    cmd.Parameters["@customerId"].Value = customerId;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }

        /// <summary>
        /// Lay 1 Customer qua ID
        /// </summary>
        /// <param name="customerID">string type</param>
        /// <returns></returns>
        public Customer Get(string customerID)
        {
            Customer data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Customers WHERE CustomerID = @customerID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@customerID", customerID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Customer()
                        {
                            CustomerID = Convert.ToString(dbReader["CustomerID"]),
                            CompanyName = Convert.ToString(dbReader["CompanyName"]),
                            ContactName = Convert.ToString(dbReader["ContactName"]),
                            ContactTitle = Convert.ToString(dbReader["ContactTitle"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            Phone = Convert.ToString(dbReader["Phone"]),
                            Fax = Convert.ToString(dbReader["Fax"])
                           
                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

        public List<Customer> List(int page, int pageSize, string searchValue ,string Country ,string address)
        {
             List<Customer> data = new List<Customer>();
        
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }
            if (!string.IsNullOrEmpty(Country))
            {
                Country = "%" + Country + "%"; ;
            }
            if (!string.IsNullOrEmpty(address))
            {
                address = "%" + address + "%"; ;
            }

            ////where ((@searchValue = '') or (CompanyName like @searchValue))
            //and((@country = N'') or(Country like @country))"

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * 
                                    from (select ROW_NUMBER() over (order by ContactName) as RowNumber, 
		                                    Customers.*
		                                    from Customers
		                                    where (@searchValue = N'' or ContactName like @searchValue) 
                                             and (@Country = N'' or Country like @Country)                                     and  (@address = N''  or Address like @address) 
		                          
	                                     ) as t

                                    where (@pageSize=-1) or t.RowNumber between (@page-1)*@pageSize+1 and @page*@pageSize
                                    order by t.RowNumber
                                    ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@Country", Country);
                cmd.Parameters.AddWithValue("@address", address);
                //ExecuteReader trả về nhiều giá trị
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Customer()
                        {
                            CustomerID = Convert.ToString(dbReader["CustomerID"]),
                            CompanyName= Convert.ToString(dbReader["CompanyName"]),
                            ContactName = Convert.ToString(dbReader["ContactName"]),
                            ContactTitle = Convert.ToString(dbReader["ContactTitle"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            Phone = Convert.ToString(dbReader["Phone"]),
                            Fax = Convert.ToString(dbReader["Fax"]),
                      
                          

                        });
                    }
                }


                connection.Close();
            }
            return data;
        }

        /// <summary>
        /// Cap nhat lai Customer qua ID
        /// </summary>
        /// <param name="data">Class Object</param>
        /// <returns></returns>
        public bool Update(Customer data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Customers
                                           SET CompanyName = @CompanyName 
                                              ,ContactName = @ContactName
                                              ,ContactTitle = @ContactTitle
                                              ,Address = @Address
                                              ,City = @City
                                              ,Country = @Country
                                              ,Phone = @Phone
                                              ,Fax = @Fax
                                           
                                          WHERE CustomerID = @CustomerID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
                cmd.Parameters.AddWithValue("@CompanyName", data.CompanyName);
                cmd.Parameters.AddWithValue("@ContactName", data.ContactName);
                cmd.Parameters.AddWithValue("@ContactTitle", data.ContactTitle);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@Phone", data.Phone);
                cmd.Parameters.AddWithValue("@Fax", data.Fax);
           

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }
            // fasle
            return rowsAffected > 0;
        }
    }
}
