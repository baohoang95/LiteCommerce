using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IOrderDetailDAL
    {
        OrderDetail Get(int id);
        List<OrderDetail> ListProduct(int id);
    }
}
