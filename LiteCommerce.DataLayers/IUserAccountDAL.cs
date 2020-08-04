using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// Khai bao cac tinh nang doi voi mot UA (UserAccount)
    /// Co che da hinh : gmail. fb ,....
    /// </summary>
    public interface IUserAccountDAL
    {
        /// <summary>
        /// Kiem tra thong tin UA co hop le ko ?
        /// Neu hop le thi tra ve thong tin UserAcc
        /// el return Null
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        UserAccount Authorize(string userName, string passWord);

        /// <summary>
        /// Cập nhật lại mật khẩu người dùng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool UpdatePassword(string id , string oldPassword ,string newPassword );

    }
}
