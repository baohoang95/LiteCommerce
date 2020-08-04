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
    public class ProductDAL : IProductDAL
    {

        private string connectionString;
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public ProductDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        //TODO : Chưa thêm Attribute cho Product
        public int Add(Product data)
        {
            int productId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Products
                                          (
	                                          ProductName,
	                                          SupplierID,
	                                          CategoryID,
	                                          QuantityPerUnit,
	                                          UnitPrice,
	                                          Descriptions,
	                                          PhotoPath
                                          )
                                          VALUES
                                          (
	                                          @ProductName,
	                                          @SupplierID,
	                                          @CategoryID,
	                                          @QuantityPerUnit,
	                                          @UnitPrice,
	                                          @Descriptions,
	                                          @PhotoPath
	                                  
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@QuantityPerUnit", data.QuantityPerUnit);
                cmd.Parameters.AddWithValue("@UnitPrice", data.UnitPrice);
                cmd.Parameters.AddWithValue("@Descriptions", data.Descriptions);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);


                productId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return productId;
        }

        public int Count(string searchValue , string CategoryID, string SupplierID)
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
                cmd.CommandText = @"
                       select COUNT(*) from Categories INNER JOIN
                             Products ON Categories.CategoryID = Products.CategoryID INNER JOIN
                             Suppliers ON Products.SupplierID = Suppliers.SupplierID
				       where  ((@searchValue =N'') or (ProductName like @searchValue) )
                                and (@categoryID=N'' or @categoryID = Products.CategoryID) 
                                and (@suppID =N'' or @suppID = Products.SupplierID)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@categoryID", CategoryID);
                cmd.Parameters.AddWithValue("@suppID", SupplierID);

                count = Convert.ToInt32(cmd.ExecuteScalar());
                //đóng kết nối
                connection.Close();
            }

            //3.tra ve kết quả thông đầu ra 
            return count;
        }

        public int Delete(int[] productIds)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Products
                                            WHERE(ProductID = @productId)
                                              AND(ProductID NOT IN(SELECT ProductID FROM                                    OrderDetails) AND
                                               ProductID NOT IN(SELECT ProductID FROM                                    ProductAttributes))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@productId", SqlDbType.Int);
                foreach (int productId in productIds)
                {
                    cmd.Parameters["@productId"].Value = productId;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }

        public Product Get(int productId)
        {
            Product data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Products WHERE ProductID = @productID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@productID", productId);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Product()
                        {
                            ProductID = Convert.ToInt32(dbReader["ProductID"]),
                            ProductName = Convert.ToString(dbReader["ProductName"]),
                            SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                            CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                            QuantityPerUnit = Convert.ToString(dbReader["QuantityPerUnit"]),
                            UnitPrice = Convert.ToInt32(dbReader["UnitPrice"]),
                            Descriptions = Convert.ToString(dbReader["Descriptions"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"])

                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

      /// <summary>
      /// Trả về danh sách các Product với các tham số 
      /// </summary>
      /// <param name="page"></param>
      /// <param name="pageSize"></param>
      /// <param name="searchValue">Giá trị tìm kiếm</param>
      /// <returns></returns>
        public List<Product> List(int page, int pageSize, string searchValue, string CategoryID, string SupplierID)
        {
            List<Product> data = new List<Product>();

            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            //if (!string.IsNullOrEmpty(CategoryID))
            //    CategoryID = "%" + CategoryID + "%";

            //if (!string.IsNullOrEmpty(SupplierID))
            //    SupplierID = "%" + SupplierID + "%";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //: From noi voi hai bang categories , supp
                connection.Open();
                //Tạo lệnh thực thi truy vấn dữ liệu
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                          from
                                          (
	                                            select	row_number() over(order by ProductName) as RowNumber,
			                                          Products.*,Categories.CategoryName, Suppliers.CompanyName
	                                            from	Categories 
                                    INNER JOIN Products ON Categories.CategoryID = Products.CategoryID 
                                    INNER JOIN Suppliers ON Products.SupplierID = Suppliers.SupplierID

	                                  where	((@searchValue =N'') or (ProductName like @searchValue) )
                                            and (@categoryID=N'' or @categoryID = Products.CategoryID) 
                                            and (@suppID =N'' or @suppID = Products.SupplierID)
                                        ) as t
                                          where 	t.RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize
                                          order by t.RowNumber";
                //@pageSize =-1 => phaan chia truong hop phan trang
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@categoryID", CategoryID);
                cmd.Parameters.AddWithValue("@suppID", SupplierID);
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Product()
                        {
                            ProductID = Convert.ToInt32(dbReader["ProductID"]),
                            ProductName = Convert.ToString(dbReader["ProductName"]),
                            SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                            CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                            QuantityPerUnit = Convert.ToString(dbReader["QuantityPerUnit"]),
                            UnitPrice = Convert.ToDecimal(dbReader["UnitPrice"]),
                            Descriptions = Convert.ToString(dbReader["Descriptions"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"]),

                            CategoryName = Convert.ToString(dbReader["CategoryName"]),
                            SupplierName = Convert.ToString(dbReader["CompanyName"]),
                            


                        });
                    }
                }
                connection.Close();
            }
            return data;
        }

        public bool Update(Product data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Products
                                           SET
                                               ProductName = @ProductName 
                                              ,SupplierID = @SupplierID
                                              ,CategoryID = @CategoryID
                                              ,QuantityPerUnit = @QuantityPerUnit
                                              ,UnitPrice = @UnitPrice
                                              ,Descriptions = @Descriptions
                                              ,PhotoPath = @PhotoPath

                                          WHERE ProductID = @ProductID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductID", data.ProductID);
                cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@QuantityPerUnit", data.QuantityPerUnit);
                cmd.Parameters.AddWithValue("@UnitPrice", data.UnitPrice);
                cmd.Parameters.AddWithValue("@Descriptions", data.Descriptions);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
   
                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }
            // fasle
            return rowsAffected > 0;
        }
    }
}
