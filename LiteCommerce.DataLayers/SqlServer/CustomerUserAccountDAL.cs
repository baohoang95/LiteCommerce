using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class CustomerUserAccountDAL : IUserAccountDAL
    {

        private string connectionString;

        /// <summary>
        /// Ham khoi tao voi chuoi ket noi
        /// </summary>
        /// <param name="connectionString"></param>
        public CustomerUserAccountDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public UserAccount Authorize(string userName, string passWord)
        {
            //co the dung cho GoogleGDAL , FaceBookDAL .... => tien cho viec mo rong  , nang cap,
            return new UserAccount()
            {
                UserID = userName,
                FullName = "Hoàng Anh Bảo",
                Photo = "ab.jpg"
                // them Title
            };
        }

     
        public bool UpdatePassword(string id, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }
        //TODO Authorize Cho Customer
    }
}
