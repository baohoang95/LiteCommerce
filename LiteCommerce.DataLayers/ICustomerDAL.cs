using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomerDAL
    {
        /// <summary>
        /// Hiển thị danh sách Customer có phân trang , tìm kiếm
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Customer> List(int page, int pageSize, string searchValue, string Country ,string address);

        /// <summary>
        /// Đếm số lượng bản ghi (customer)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue, string Country ,string address);

        /// <summary>
        /// Lấy một customer theo ID string
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        Customer Get(string customerID);

        /// <summary>
        /// Hàm thêm một customer , return customerID được thêm 
        /// Nếu lỗi thì trả về 1 giá trị <= 0
        /// </summary>
        /// <param name="data">ID Customer</param>
        /// <returns></returns>
        string Add(Customer data);

        /// <summary>
        /// Hàm cập nhật lại 1 Customer , return true nếu Update thành công
        /// Nếu lỗi thì trả về fasle
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Customer data);

        /// <summary>
        /// Trả về số Customer đã bị xóa
        /// </summary>
        /// <param name="customerIDs"></param>
        /// <returns></returns>
        int Delete(string[] customerIDs);


    }
}
