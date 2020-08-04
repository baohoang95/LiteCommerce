using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
    /// <summary>
    /// Danh sach shipper
    /// </summary>
    public class ShipperPaginationResult:PaginationResult
    {
       public List<Shipper> Data { get; set; }
    }
}