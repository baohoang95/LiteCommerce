using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IOrderDAL
    {
        /// <summary>
        /// Hiển thị danh sách các Orders có phân trang và tìm kiếm
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Order> List(int page, int pageSize, string searchValue);
        // TeamLeader => Prototype => Client

        /// <summary>
        /// Đếm số lượng các Orders thỏa mãn điều kiện tìm kiếm
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        Order Get(int orderID);

        /// <summary>
        /// Bổ sung 1 Order  Hàm trả về ID của Order được bổ sung
        /// Nếu lỗi thì hàm trả về giá trị nhỏ hơn hoặc bằng 0
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Order data);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Order data);

        /// <summary>
        /// Xoa nhieu supp
        /// tra ve so luong order da xoa
        /// </summary>
        /// <param name="orderIDs"></param>
        /// <returns></returns>
        int Delete(int[] orderIDs);

    }
}
