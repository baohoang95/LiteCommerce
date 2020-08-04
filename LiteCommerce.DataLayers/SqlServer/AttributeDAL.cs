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
    public class AttributeDAL : IAttributeDAL
    {
        private string connectionString;

        /// <summary>
        /// Ham khoi tao voi chuoi ket noi
        /// </summary>
        /// <param name="connectionString"></param>
        public AttributeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }


        
        /// <summary>
        /// Danh sach cac thuoc tinh cua Product | DAL
        /// </summary>
        /// <returns></returns>
        public List<ProductAttribute> List()
        {
            List<ProductAttribute> data = new List<ProductAttribute>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //mở kết nối
                connection.Open();

                //2. tạo và xử lý lệnh thông qua nghiệp vụ tương ứng
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @" SELECT AttributeName FROM ProductAttributes";

                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;

                //ExecuteReader trả về nhiều giá trị
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new ProductAttribute()
                        {
                            AttributeName = Convert.ToString(dbReader["AttributeName"])

                        });
                    }
                }

                //đóng kết nối
                connection.Close();
            }

            return data;
        }
    }
}
