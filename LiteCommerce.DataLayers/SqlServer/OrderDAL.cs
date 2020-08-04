using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
using System.Data.SqlClient;
using System.Data;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class OrderDAL : IOrderDAL
    {
        private string connectionString;

        public OrderDAL (string _connectionString)
        {
            this.connectionString = _connectionString;
        }
        public int Add(Order data)
        {
            //TODO: Chưa làm hàm Add Order
            throw new NotImplementedException();
        }

        public int Count(string searchValue)
        {

            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*) from Orders 
                                    where @searchValue = N'' or OrderID like @searchValue";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return count;
        }

        public int Delete(int[] orderIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM OrderDetails
                                     WHERE OrderID IN (SELECT OrderID FROM Orders
                                                       WHERE OrderID = @orderID and Orders.Status=N'Delivered' )
                                     DELETE FROM Orders 
                                     WHERE OrderID = @orderID and Orders.Status=N'Delivered' ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@orderID", SqlDbType.Int);
                foreach (int orderId in orderIDs)
                {
                    cmd.Parameters["@orderID"].Value = orderId;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }

        public Order Get(int orderID)
        {
            Order data = null;
            DateTime date;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                 
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Orders WHERE OrderID = @orderID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@orderID", orderID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        if(dbReader["ShippedDate"]==null )
                        {
                            date = new DateTime(0001, 01, 01);
                        }
                        else
                        {
                            date = Convert.ToDateTime(dbReader["ShippedDate"]);
                        }
                        data = new Order()
                        {
                            OrderID = Convert.ToInt32(dbReader["OrderID"]),
                            CustomerID = Convert.ToString(dbReader["CustomerID"]),
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            OrderDate = Convert.ToDateTime(dbReader["OrderDate"]),
                            ShipAddress = Convert.ToString(dbReader["ShipAddress"]),
                            ShipCity= Convert.ToString(dbReader["ShipCity"]),
                            ShipCountry= Convert.ToString(dbReader["ShipCountry"]),
                            Status = Convert.ToString(dbReader["Status"]),
                            Freight = Convert.ToDecimal(dbReader["Freight"]),
                            RequiredDate = Convert.ToDateTime(dbReader["RequiredDate"]),
                            ShippedDate = date,
                            ShipperID = Convert.ToInt32(dbReader["ShipperID"])
                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

        public List<Order> List(int page, int pageSize, string searchValue)
        {

            List<Order> data = new List<Order>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //Tạo lệnh thực thi truy vấn dữ liệu | Không nên dùng select * => vì khi thay đổi dử liệu sẽ dễ bị lỗi + xu ly cau lenh sql lau
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                          from
                                          (
	                                            select	row_number() over(order by OrderID Desc) as RowNumber,
			                                          Orders.*
	                                            from	Orders
	                                            where	((@searchValue = N'') or (OrderID like @searchValue))
                                                
                                          ) as t
                                          where t.RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize
                                          order by t.RowNumber";
      
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Order()
                        {
                            OrderID = Convert.ToInt32(dbReader["OrderID"]),
                            CustomerID = Convert.ToString(dbReader["CustomerID"]),
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            OrderDate = Convert.ToDateTime(dbReader["OrderDate"]),
                            ShipAddress = Convert.ToString(dbReader["ShipAddress"]) +", "+ Convert.ToString(dbReader["ShipCity"]) + ", " + Convert.ToString(dbReader["ShipCountry"]), 
                            Status = Convert.ToString(dbReader["Status"]),
                            RequiredDate = Convert.ToDateTime(dbReader["RequiredDate"]),
                            ShippedDate = Convert.ToDateTime(dbReader["ShippedDate"]),
                            
                        });
                    }
                }
                connection.Close();
            }
            return data;
        }

        public bool Update(Order data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Orders
                                           SET EmployeeID = @EmployeeID 
                                              ,RequiredDate = @RequiredDate
                                              ,ShippedDate = @ShippedDate
                                              ,ShipperID = @ShipperID
                                              ,Freight = @Freight
                                              ,ShipAddress = @ShipAddress
                                              ,ShipCity = @ShipCity
                                              ,ShipCountry = @ShipCountry
                                              ,Status = @Status
                                             
                                          WHERE OrderID = @OrderID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
                cmd.Parameters.AddWithValue("@RequiredDate", data.RequiredDate);
                cmd.Parameters.AddWithValue("@ShippedDate", data.ShippedDate);
                cmd.Parameters.AddWithValue("@ShipperID", data.ShipperID);
                cmd.Parameters.AddWithValue("@Freight", data.Freight);
                cmd.Parameters.AddWithValue("@ShipAddress", data.ShipAddress);
                cmd.Parameters.AddWithValue("@ShipCity", data.ShipCity);
                cmd.Parameters.AddWithValue("@ShipCountry", data.ShipCountry);
                cmd.Parameters.AddWithValue("@OrderID", data.OrderID);
                cmd.Parameters.AddWithValue("@Status", data.Status);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }
            // fasle
            return rowsAffected > 0;
        }
    }
}
