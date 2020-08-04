using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
    /// <summary>
    /// Danh sach khach hang
    /// </summary>
    public class CustomerPaginationResult:PaginationResult
    {
       public List<Customer> Data { get; set; }
    
       public string Country { get; set; }
        public string Address { get; set; }
    }
}