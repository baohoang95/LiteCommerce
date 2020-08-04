using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
    /// <summary>
    /// Danh sach supplier
    /// </summary>
    public class SupplierPaginationResult : PaginationResult
    {
        public List<Supplier> Data { get; set; }
    }
}