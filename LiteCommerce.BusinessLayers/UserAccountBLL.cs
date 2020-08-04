using LiteCommerce.DataLayers;
using LiteCommerce.DataLayers.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
 /// <summary>
 /// Cung cap cac chung nang tac nghiep den TKND
 /// </summary>
    public static class UserAccountBLL
    {
        private static string _connectionString;

        public static object IUserAcconutDAL { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            _connectionString = connectionString;
            // Mình khai báo lên trên để dùng cho 2 method , Chưa đúng lắm , cần tối ưu vì tùy trường hợp trả về UA => mặc định là của nhân viên

            userAccountDB = new EmployeeUserAccountDAL(_connectionString);
            
        }
        private static IUserAccountDAL userAccountDB { get; set; }

        /// <summary>
        /// goi Cơ chế đăng nhập tuong ung => da hinh
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static UserAccount Authorize (string userName , string password , UserAccountTypes userType)
        {
            //IUserAccountDAL userAccountDB;
            switch (userType)
            {
                case UserAccountTypes.Employee:
                    userAccountDB = new EmployeeUserAccountDAL(_connectionString);
                    break;
                case UserAccountTypes.Customer:
                    userAccountDB = new CustomerUserAccountDAL(_connectionString);
                    break;
                default:
                    return null;
            }
            // co che da hinh => tuy tinh huong ma su dug

            return userAccountDB.Authorize(userName, password);
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public static bool UpdatePassword(string id, string oldPassword, string newPassword)
        {
            return userAccountDB.UpdatePassword(id, oldPassword, newPassword);
        }


    }
}
