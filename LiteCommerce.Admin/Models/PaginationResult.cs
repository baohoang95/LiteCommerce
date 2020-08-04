using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
    /// <summary>
    /// Lớp định nghĩa các phương thức dùng chung 
    /// </summary>
    public abstract class PaginationResult
    {
        //Vì sao lại dùng lớp kiểu abstract (trừu tượng)
        //=> vì giúp các lớp các tái sử dụng mà ko cần phải follow theo
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
    
        /// <summary>
        /// gia tri tim kiems
        /// </summary>
        public string SeachValue { get; set; }

        /// <summary>
        /// tong so trang
        /// </summary>
        public int PageCount
        {
            get
            {
                int p = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                {
                    p += 1;
                }
                return p;

            }
        }
    }
}