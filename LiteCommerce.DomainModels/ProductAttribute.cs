using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    /// <summary>
    /// Khai báo các thuộc tính cung cấp cho Product
    /// </summary>
    public class ProductAttribute
    {
        /// <summary>
        /// Mã thuộc tính
        /// </summary>
        public int AttributeID { get; set; }
       /// <summary>
       /// Mã sản phẩm
       /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Tên thuộc tính
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// Giá trị thuộc tính
        /// </summary>
        public string AttributeValues { get; set; }

        /// <summary>
        /// Hiển thị đơn hàng
        /// </summary>
        public int DisplayOrder { get; set; }

    }
}