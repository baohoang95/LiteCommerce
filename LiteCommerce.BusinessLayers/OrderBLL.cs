using LiteCommerce.DataLayers;
using LiteCommerce.DataLayers.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public class OrderBLL
    {
        /// <summary>
        /// Hàm khởi tạo với chuổi kết nối trong BusinessLayerConfig
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            OrderDB = new DataLayers.SqlServer.OrderDAL(connectionString);
            OrderDetailDB = new DataLayers.SqlServer.OrderDetailDAL(connectionString);
        }

        public static IOrderDAL OrderDB { get; set; }
        public static IOrderDetailDAL OrderDetailDB { get; set; }

        public static List<Order> ListOfOrder(int page, int pageSize , string searchValue ,  out int rowCount)
        {

            rowCount = OrderDB.Count(searchValue);
            return OrderDB.List(page, pageSize, searchValue);
        }

        public static Order GetOrder(int id)
        {
            return OrderDB.Get(id);
        }

        public static bool UpdateOrder(Order data)
        {
            return OrderDB.Update(data);
        }

        /// <summary>
        /// Xóa các đơn hàng được chọn theo ID đơn hàng 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
         public static int DeleteOrders(int[] orderIDs )
        {
            return OrderDB.Delete(orderIDs);
        }

        public static OrderDetail GetOrderDetail(int id)
        {
            return OrderDetailDB.Get(id);
        }
        public static List<OrderDetail> ListProduct (int id)
        {
            return OrderDetailDB.ListProduct(id);
        }

    }
}
