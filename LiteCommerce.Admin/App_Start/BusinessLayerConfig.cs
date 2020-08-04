using LiteCommerce.BusinessLayers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin
{
    public class BusinessLayerConfig
    {
        public static void Init()
        {
            // lay chuoi ket noi webconfig
            string connectionString = ConfigurationManager.ConnectionStrings["LiteCommerce"].ConnectionString;
            // Khoi tao cac DLL khi can su dung den
            CatalogBLL.Initialize(connectionString);

            // them khoi tao
            UserAccountBLL.Initialize(connectionString);

            //OrderBLL
            OrderBLL.Initialize(connectionString);

        }
    }
}