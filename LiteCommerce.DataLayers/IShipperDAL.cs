using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// Định nghĩa các phương thức khuôn mẫu 
    /// </summary>
    public  interface IShipperDAL
    {
        /// <summary>
        /// Danh sách shipper với các tham số
        /// </summary>
        /// <param name="page">Số trang</param>
        /// <param name="pageSize">Kích cỡ trang</param>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <returns></returns>
        List<Shipper> List(int page, int pageSize, string searchValue);

        /// <summary>
        /// Hàm trả về số lượng bản ghi tương ứng với giá trị tìm kiếm
        /// </summary>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <returns></returns>
        int Count(string searchValue);

        /// <summary>
        /// Lấy về 1 shipper theo ID
        /// </summary>
        /// <param name="data">ID param </param>
        /// <returns></returns>
        Shipper Get(int data);

        /// <summary>
        /// Thêm mới 1 shipper trả về ID , else return a value <0
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Shipper data);

        /// <summary>
        /// Cập nhật lại Data shipper 
        /// </summary>
        /// <param name="data">Dữ liệu của shipper</param>
        /// <returns></returns>
        bool Update(Shipper data);

        /// <summary>
        /// Xóa nhiều shipper 
        /// </summary>
        /// <param name="shipperID">ID shipper</param>
        /// <returns></returns>
        int Delele(int[] shipperID);
    }
}
