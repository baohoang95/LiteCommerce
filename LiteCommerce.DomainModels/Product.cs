using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    /// <summary>
    /// Khai báo thông tin ( thuộc tính ) của một Product
    /// </summary>
    public class Product
    {
        /// <summary>
        /// ID của một Sản phẩm
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Mã nhà cung cấp ứng với sản phẩm
        /// </summary>
        public int SupplierID { get; set; }
        /// <summary>
        /// Mã thể loại ứng với sản phẩm
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// Số lượng tính theo đơn vị nào
        /// </summary>
        public string QuantityPerUnit { get; set; }
        /// <summary>
        /// Đơn vị tính của sản phẩm
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// Mô tả chi tiết cho sản phẩm
        /// </summary>
        public string Descriptions { get; set; }
        /// <summary>
        /// Đường dẫn đến ảnh của sản phẩm
        /// </summary>
        public string PhotoPath { get; set; }
        /// <summary>
        /// Kiểu category để đưa dữ liệu ra list
        /// </summary>
        /// 

        public string CategoryName { get; set; }
        public string SupplierName { get; set; }


        //dung cho co che lazyloading : entityFW
        /// <summary>
        /// Danh sách các thuộc tính có thể có của Product
        /// </summary>
        public virtual List<ProductAttribute> Attributes {get;set;}

    }
}
