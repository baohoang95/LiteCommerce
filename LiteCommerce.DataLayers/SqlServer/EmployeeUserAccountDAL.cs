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
    public class EmployeeUserAccountDAL : IUserAccountDAL
    {
      
        private string connectionString;

        /// <summary>
        /// Ham khoi tao voi chuoi ket noi
        /// </summary>
        /// <param name="connectionString"></param>
        public EmployeeUserAccountDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public UserAccount Authorize(string userName, string passWord)
        {
            // : cai dat cac chung nang  Authorize
            UserAccount data = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //mở kết nối
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select EmployeeID , LastName, FirstName, Title, PhotoPath ,Roles,Email
                                    from Employees 
                                    where Email=@userName and Password = @passWord";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@userName", userName);
                cmd.Parameters.AddWithValue("@passWord", passWord);

                //ExecuteReader trả về nhiều giá trị
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        //Dữ liệu trả về controller sau khi đang nhập thành công 
                        // dựa vào Roles để phân quyền cho người dùng ở Controller
                        data = new UserAccount()
                        {
                            UserID = dbReader["EmployeeID"].ToString(),
                            FullName = $"{dbReader["FirstName"]} {dbReader["LastName"]}",
                            Photo = dbReader["PhotoPath"].ToString(),
                            Title = dbReader["Title"].ToString(),
                            Roles = dbReader["Roles"].ToString(),
                            Email = dbReader["Email"].ToString()
                            
                        };
                    }
                }

                //đóng kết nối
                connection.Close();

            }
                return data;

                //return new UserAccount()
                //{
                //    UserID = userName,
                //    FullName = "Hoàng Anh Bảo",
                //    Photo = "ab.jpg",
                //    Title = "Employee"
                //};
            
        }


        /// <summary>
        /// Cập nhật mật khẩu mới 
        /// trả về số dòng bị tác động > 0 True | Fasle
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdatePassword(string id, string oldPassword, string newPassword )
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Employees
                                           SET Password=@NewPassword
                                           WHERE EmployeeID = @EmployeeID
                                                AND Password= @OldPassword  ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@EmployeeID", Convert.ToInt32(id));
                cmd.Parameters.AddWithValue("@NewPassword", newPassword);
                cmd.Parameters.AddWithValue("@OldPassword", oldPassword);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }
           
            return rowsAffected > 0;
        }
    }
}
