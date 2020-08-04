using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    /// <summary>
    /// Luu cac info lien quan den tai khoan dang nhap he thong
    /// da hinh : emloy, shipp , customer
    /// </summary>
   public class UserAccount
    {
        /// <summary>
        /// ID cua TK dn he thong
        /// </summary>
       public string UserID { get; set; }
        /// <summary>
        /// first + last Name
        /// </summary>
       public string FullName { get; set; }

        /// <summary>
        /// ten file anh cua User
        /// </summary>
       public string Photo { get; set; }

        //=> : Bo sung them neu can 

        /// <summary>
        /// Vai tro cua nguoi dung
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Quyền của người dùng với hệ thống
        /// Dùng mảng để lưu các quyền 
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// Email người dùng 
        /// </summary>
        public string Email { get; set; }
    }
}
