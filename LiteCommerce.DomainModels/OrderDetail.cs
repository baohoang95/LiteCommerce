using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    /// <summary>
    /// Khai báo các thuộc tính của chi tiết đơn hàng
    /// </summary>
    public class OrderDetail
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        //nhà cung cấp
        public string SupplierName { get; set; }
        public string AddressSupp { get; set; }
        public string CitySupp { get; set; }
        public string CountrySupp { get; set; }
        public string PhoneSupp { get; set; }
      

        //Khach Hang
        public string CustomerName { get; set; }
        public string AddressCus { get; set; }
        public string CityCus { get; set; }
        public string CountryCus { get; set; }
        public string PhoneCus { get; set; }
  

        //San Pham

        public string ProductName { get; set; }
        public string Descriptions { get; set; }
        public decimal SubTotal { get; set; }


        public decimal Total { get; set; }

        public decimal Freight { get; set; }

    }
}
