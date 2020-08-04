﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    /// <summary>
    /// Khai báo các thuộc tính của đơn hàng (Orders)
    /// </summary>
    public class Order
    {
        public int OrderID {get;set;}
        public string  CustomerID {get;set; }
        public int EmployeeID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public int ShipperID { get; set; }
        public decimal Freight { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set;}
        public string ShipCountry { get; set; }
        public string Status { get; set; }
    }
}