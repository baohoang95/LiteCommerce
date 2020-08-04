using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// Interface of Country
    /// </summary>
    public interface ICountryDAL
    {
        /// <summary>
        /// Return list countries
        /// </summary>
        List<Country> List(int page, int pageSize, string searchValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);

        /// <summary>
        /// Lấy về 1 Country theo ID
        /// </summary>
        /// <param name="data">ID param </param>
        /// <returns></returns>
        Country Get(int  id);

        /// <summary>
        /// Thêm mới 1 Country trả về ID , else return a value <0
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Country data);

        /// <summary>
        /// Cập nhật lại Data Country 
        /// </summary>
        /// <param name="data">Dữ liệu của shipper</param>
        /// <returns></returns>
        bool Update(Country data);

        /// <summary>
        /// Xóa nhiều CategoryID 
        /// </summary>
        /// <param name="CoutriesName">ID Country</param>
        /// <returns>Số lượng Country đã xóa</returns>
        int Delete(int[] CountryId);

    }
}
