using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IAttributeDAL
    {
        /// <summary>
        /// Danh sach cac thuoc tinh cua Product neu co
        /// </summary>
        /// <returns></returns>
        List<ProductAttribute> List();
    }
}
