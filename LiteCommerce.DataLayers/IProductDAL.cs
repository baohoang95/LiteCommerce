using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// Chứa các method liên quan đến Product
    /// </summary>
    public  interface IProductDAL
    {

         /// <summary>
        /// Danh sách Product với các tham số
        /// </summary>
        /// <param name="page">Số trang</param>
        /// <param name="pageSize">Kích cỡ trang</param>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <returns></returns>
        List<Product> List(int page, int pageSize, string searchValue , string CategoryID ,string SupplierID);

    
        /// <summary>
        /// Hàm trả về số lượng bản ghi tương ứng với giá trị tìm kiếm
        /// </summary>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <returns></returns>
        int Count(string searchValue , string CategoryID, string SupplierID);

        /// <summary>
        /// Lấy về 1 Product theo ID
        /// </summary>
        /// <param name="data">ID param </param>
        /// <returns></returns>
        Product Get(int data);

        /// <summary>
        /// Thêm mới 1 Product trả về ID , else return a value <0
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Product data);

        /// <summary>
        /// Cập nhật lại Data Product 
        /// </summary>
        /// <param name="data">Dữ liệu của Product</param>
        /// <returns></returns>
        bool Update(Product data);

        /// <summary>
        /// Xóa nhiều Product 
        /// </summary>
        /// <param name="ProductID">ID Product</param>
        /// <returns></returns>
        int Delete(int[] ProductID);

    }
    
}
