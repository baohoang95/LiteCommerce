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
    /// Giao tiếp với table Employee
    /// </summary>
    public class EmployeeDAL : IEmployeeDAL
    {
        private string connectionString;

        /// <summary>
        ///Hàm tạo với chuổi kết nối
        /// </summary>
        /// <param name="connectionString"></param>
        public EmployeeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Employee data)
        {
     
            int employeeId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Employees
                                          (
	                                          LastName,
	                                          FirstName,
	                                          Title,
	                                          BirthDate,
	                                          HireDate,
	                                          Email,
	                                          Address,
	                                          City,
	                                          Country,
                                              HomePhone,
	                                          Notes,
	                                          PhotoPath,
	                                          Password,
                                              Roles
	                                          
                                          )
                                          VALUES
                                          (
	                                       
	                                          @LastName,
	                                          @FirstName,
	                                          @Title,
	                                          @BirthDate,
	                                          @HireDate,
	                                          @Email,
	                                          @Address,
	                                          @City,
	                                          @Country,
                                              @HomePhone,
	                                          @Notes,
	                                          @PhotoPath,
	                                          @Password,
                                              @Roles
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@BirthDate", data.BirthDate);
                cmd.Parameters.AddWithValue("@HireDate", data.HireDate);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@HomePhone", data.HomePhone);
                cmd.Parameters.AddWithValue("@Notes", data.Notes);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                cmd.Parameters.AddWithValue("@Password", data.Password);
                cmd.Parameters.AddWithValue("@Email", data.Email);
                cmd.Parameters.AddWithValue("@Roles", data.Roles);

                employeeId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return employeeId;
        }

        /// <summary>
        /// Đếm số bản ghi tương ướng với searchValue
        /// </summary>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <returns></returns>
        public int Count(string searchValue)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            //1.thiết lập kết nối
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //mở kết nối
                connection.Open();

                //2.tạo và xử lý lệnh thông qua nghiệp vụ tương ứng
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select COUNT(*) from Employees where ( @searchValue = N'') or (LastName like @searchValue) or (Email like @searchValue) "; 
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());
                //đóng kết nối
                connection.Close();
            }

            //3.tra ve kết quả thông đầu ra 
            return count;
        }

        public int Delete(int[] employeeIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Employees
                                            WHERE(EmployeeID = @employeeId)
                                              AND(EmployeeID NOT IN(SELECT EmployeeID FROM Orders))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@employeeId", SqlDbType.Int);
                foreach (int employeeId in employeeIDs)
                {
                    cmd.Parameters["@employeeId"].Value = employeeId;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }

        /// <summary>
        /// Lay 1 nhan vien
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public Employee Get(int employeeID)
        {
            Employee data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Employees WHERE EmployeeID = @employeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@employeeID", employeeID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Employee()
                        {
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            LastName = Convert.ToString(dbReader["LastName"]),
                            FirstName = Convert.ToString(dbReader["FirstName"]),
                            Title = Convert.ToString(dbReader["Title"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            Notes = Convert.ToString(dbReader["Notes"]),
                            Email = Convert.ToString(dbReader["Email"]),
                            HomePhone = Convert.ToString(dbReader["HomePhone"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                            Password= Convert.ToString(dbReader["Password"]),
                            BirthDate= Convert.ToDateTime(dbReader["BirthDate"]),
                            HireDate = Convert.ToDateTime(dbReader["HireDate"]),
                            Roles= Convert.ToString(dbReader["Roles"])

                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

        /// <summary>
        /// Trả về danh sách các Employee với các tham số
        /// </summary>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Kích cở trang</param>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <returns></returns>
        public List<Employee> List(int page, int pageSize, string searchValue)
        {
            List<Employee> data = new List<Employee>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            //1.thiết lập kết nối
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //mở kết nối
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * 
                                    from (select ROW_NUMBER() over (order by LastName) as RowNumber, 
		                                    Employees.*
		                                    from Employees
		                                    where (@searchValue = N'') or (LastName like @searchValue) or                       (Email like @searchValue)
		                                        
	                                     ) as t

                                    where t.RowNumber between (@page-1)*@pageSize+1 and @page*@pageSize
                                    order by t.RowNumber
                                    ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                //ExecuteReader trả về nhiều giá trị
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Employee()
                        {
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            LastName = Convert.ToString(dbReader["LastName"]),
                            FirstName = Convert.ToString(dbReader["FirstName"]),
                            Title = Convert.ToString(dbReader["Title"]),
                            HireDate = Convert.ToDateTime(dbReader["HireDate"]),
                            BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                            Email = Convert.ToString(dbReader["Email"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country  = Convert.ToString(dbReader["Country"]),
                            HomePhone =  Convert.ToString(dbReader["HomePhone"]),
                            Notes  =  Convert.ToString(dbReader["Notes"]),
                            Password  =  Convert.ToString(dbReader["Password"]),
                            PhotoPath =  Convert.ToString(dbReader["PhotoPath"])
                        });
                    }
                }

                //đóng kết nối
                connection.Close();
            }

            return data;
        }

        public bool Update(Employee data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Employees
                                           SET
                                               LastName = @LastName 
                                              ,FirstName = @FirstName
                                              ,Title = @Title
                                              ,BirthDate = @BirthDate
                                              ,HireDate = @HireDate
                                              ,Email = @Email
                                              ,Address = @Address
                                              ,City = @City
                                              ,Country = @Country
                                              ,HomePhone=@HomePhone
	                                          ,Notes=@Notes
	                                          ,PhotoPath=@PhotoPath
	                                          ,Password=@Password


                                          WHERE EmployeeID = @EmployeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@BirthDate", data.BirthDate);
                cmd.Parameters.AddWithValue("@HireDate", data.HireDate);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@HomePhone", data.HomePhone);
                cmd.Parameters.AddWithValue("@Notes", data.Notes);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                cmd.Parameters.AddWithValue("@Password", data.Password);
                cmd.Parameters.AddWithValue("@Email", data.Email);
                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }
            // fasle
            return rowsAffected > 0;
        }
    }
}
