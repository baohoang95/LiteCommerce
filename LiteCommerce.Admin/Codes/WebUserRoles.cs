﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin
{
      /// <summary>
      /// Định nghĩa danh sách các Role của user
      /// </summary>
      public class WebUserRoles
      {
            /// <summary>
            /// Không xác định
            /// </summary>
            public const string ANONYMOUS = "anonymous";
            /// <summary>
            /// Nhân viên
            /// </summary>
            public const string STAFF = "Saleman";
            /// <summary>
            /// Quản trị hệ thống
            /// </summary>
            public const string ADMINISTRATOR = "Administrator";

            public const string ACCOUNTANT = "Accountant";
        // khai bao hang de tai su dung =\> cleanCode >2lan , emnum dang thu tu
      }
}