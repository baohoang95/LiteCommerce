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
    /// 
    /// </summary>
    public class CategoryDAL : ICategoryDAL
    {
        private string connectionString;

        /// <summary>
        /// Ham khoi tao voi chuoi ket noi
        /// </summary>
        /// <param name="connectionString"></param>
        public CategoryDAL (string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int Add(Category data)
        {
            int categoryId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Categories
                                          (
	                                          CategoryName,
	                                          Description
	                                       
                                          )
                                          VALUES
                                          (
	                                          @CategoryName,
	                                          @Description
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CategoryName", data.CategoryName);
                cmd.Parameters.AddWithValue("@Description", data.Description);

                categoryId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return categoryId;
        }

        public int Count(string searchValue)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            //1.thiết lập kết nối
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //1.1 mở kết nối
                connection.Open();

                //2. tạo và xử lý lệnh thông qua nghiệp vụ tương ứng
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select COUNT(*) from Categories where  @searchValue = N'' or CategoryName like @searchValue"; ;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());
                //1.2 đóng kết nối
                connection.Close();
            }

            //3.tra ve kết quả thông đầu ra 
            return count;
        }

        public int Delete(int[] categoryIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Categories
                                            WHERE(CategoryID = @categoryId)
                                              AND(CategoryID NOT IN(SELECT CategoryID FROM Products))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@categoryId", SqlDbType.Int);
                foreach (int categoryId in categoryIDs)
                {
                    cmd.Parameters["@categoryId"].Value = categoryId;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }

        public Category Get(int categoryID)
        {
            Category data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Categories WHERE CategoryID = @categoryID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@categoryID", categoryID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Category()
                        {
                            CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                            CategoryName = Convert.ToString(dbReader["CategoryName"]),
                            Description = Convert.ToString(dbReader["Description"]),
                          
                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

        /// <summary>
        /// Trả về danh sách các Category
        /// </summary>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Kích cở trang</param>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <returns></returns>
        public List<Category> List(int page, int pageSize, string searchValue)
        {
            List<Category> data = new List<Category>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            //1.thiết lập kết nối
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //mở kết nối
                connection.Open();

                //2. tạo và xử lý lệnh thông qua nghiệp vụ tương ứng
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * 
                                    from (select ROW_NUMBER() over (order by CategoryName) as RowNumber, 
		                                    Categories.*
		                                    from Categories
		                                    where (@searchValue = N'') or (CategoryName like @searchValue)
		
	                                     ) as t

                                    where (@pageSize =-1) or

                                        t.RowNumber between (@page-1)*@pageSize+1 and @page*@pageSize
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
                        data.Add(new Category()
                        {
                            CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                            CategoryName = Convert.ToString(dbReader["CategoryName"]),
                            Description = Convert.ToString(dbReader["Description"])
                        });
                    }
                }

                //đóng kết nối
                connection.Close();
            }

            return data;
        }

        public bool Update(Category data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Categories
                                           SET CategoryName = @CategoryName 
                                              ,Description = @Description
                                              
                                          WHERE CategoryID = @CategoryID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@CategoryName", data.CategoryName);
                cmd.Parameters.AddWithValue("@Description", data.Description);
           

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }
            // fasle
            return rowsAffected > 0;
        }
    }
}
