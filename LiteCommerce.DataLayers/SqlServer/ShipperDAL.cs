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
    /// Giao tiếp với table Shipper
    /// </summary>
    public class ShipperDAL : IShipperDAL
    {
        private string connectionString;

        /// <summary>
        /// Hàm tạo set kết nối nội tuyến
        /// </summary>
        /// <param name="connectionString">Chuổi kết nối được truyền từ lớp trên Business</param>
        public ShipperDAL (string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Thêm một Shipper
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Shipper data)
        {
            int shipperId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Shippers
                                          (
	                                          CompanyName,
	                                          Phone
	                                       
                                          )
                                          VALUES
                                          (
	                                          @CompanyName,
	                                          @Phone
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CompanyName", data.CompanyName);
                cmd.Parameters.AddWithValue("@Phone", data.Phone);

                shipperId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return shipperId;
        }

        /// <summary>
        /// Đếm số bản ghi tương ướng với searchValue
        /// </summary>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <returns></returns>
        public int Count(string searchValue)
        {
            int count =0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            //1.thiết lập kết nối
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //mở kết nối
                connection.Open();

                //2.tạo và xử lý lệnh thông qua nghiệp vụ tương ứng
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select COUNT(*) from Shippers where  @searchValue = N'' or CompanyName like @searchValue or Phone like @searchValue"; ;
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

        /// <summary>
        /// Xóa nhiều shipper
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public int Delele(int[] shipperIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Shippers
                                            WHERE(ShipperID = @ShipperId)
                                              AND(ShipperID NOT IN(SELECT ShipperID FROM Orders))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@shipperId", SqlDbType.Int);
                foreach (int shipperId in shipperIDs)
                {
                    cmd.Parameters["@shipperId"].Value = shipperId;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }

        public Shipper Get(int shipperID)
        {
            Shipper data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Shippers WHERE ShipperID = @shipperID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@shipperID", shipperID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Shipper()
                        {
                            ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                            CompanyName = Convert.ToString(dbReader["CompanyName"]),
                            Phone = Convert.ToString(dbReader["Phone"])

                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

        /// <summary>
        /// Trả về danh sách các shippers với các tham số
        /// </summary>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Kích cở trang</param>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <returns></returns>
        public List<Shipper> List(int page, int pageSize, string searchValue)
        {
            List<Shipper> data = new List<Shipper>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            //1.thiết lập kết nối
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //mở kết nối
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * 
                                    from (select ROW_NUMBER() over (order by CompanyName) as RowNumber, 
		                                    Shippers.*
		                                    from Shippers
		                                    where (@searchValue = N'') or (CompanyName like @searchValue) or (Phone like @searchValue)
		
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
                        data.Add(new Shipper()
                        {
                            ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                            CompanyName = Convert.ToString(dbReader["CompanyName"]),
                            Phone = Convert.ToString(dbReader["Phone"])
                        });
                    }
                }

                //đóng kết nối
                connection.Close();
            }

            return data;
        }

        /// <summary>
        /// Cập nhật lại thông tin 1 shipper
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Shipper data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Shippers
                                           SET CompanyName = @CompanyName 
                                              ,Phone = @Phone
                                              
                                          WHERE ShipperID = @ShipperID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ShipperID", data.ShipperID);
                cmd.Parameters.AddWithValue("@CompanyName", data.CompanyName);
                cmd.Parameters.AddWithValue("@Phone", data.Phone);


                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }
            // fasle
            return rowsAffected > 0;
        }
    }
}
