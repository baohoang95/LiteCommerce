using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LiteCommerce.Admin.Codes;
namespace LiteCommerce.Admin.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
       /// <summary>
       /// Profile 
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       [HttpGet]
        public ActionResult Index(string id = "")
        {
            
            var userData = User.GetUserData();
            if ( userData.UserID == id)
            {
                Employee detailEmployee = CatalogBLL.GetEmployee(Convert.ToInt32(id));
                if (detailEmployee == null)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                return View(detailEmployee);
            }
            else
            {
                SetAlert("You do not have access this content", "warning");
                return RedirectToAction("Index", "Account" , new {id= userData.UserID});

            }
        }
  
        /// <summary>
        /// View change password
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Change password with method post 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="reType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePassword(string id = "", string oldPassword = "", string newPassword = "", string reType = "")
        {
             if (newPassword == reType && newPassword == oldPassword)
            {
                SetAlert("New password must not duplicate Old password", "warning");
            }

           else if (newPassword == reType) 
            {
                if(UserAccountBLL.UpdatePassword(id, EncodeMD5.EncodeMD5B(oldPassword), EncodeMD5.EncodeMD5B(newPassword)))
                {
                    SetAlert("Change password success !", "success");
                }
                else
                {
                    SetAlert("Old password do not match !", "danger");
                }
            }
            
            else
            {
                SetAlert("Retype password do not match", "warning");
            }
         
            return View();
        }


        /// <summary>
        /// Sign out , clear session in cookie on web browser client
        /// </summary>
        /// <returns></returns>
        public ActionResult SignOut()
        {
            Session.Abandon();
            Session.Clear();

            // xoa trong he thong
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("SignIn");
        }


        /// <summary>
        /// Sign in  , Allow Anonymuos
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SignIn()
        {
            // neuw da dang nhap thi chuyen ve dashboard = true
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(string email ="" , string password ="")
        {
            // chưa mã hóa MD5 cho password => đã mã hóa
            // kiem tra thong tin tk UAT = EMPLOYEE 
           // Codes.EncodeMD5.EncodeMD5B(password);

            UserAccount user = UserAccountBLL.Authorize(email, Codes.EncodeMD5.EncodeMD5B(password), UserAccountTypes.Employee);
                {
                    if (user != null) // dang nhap thanh cong => ghi nhan cookies 
                    {
                        WebUserData userData = new WebUserData()
                        {
                            UserID = user.UserID,
                            FullName = user.FullName,
                            GroupName =user.Roles, // Sua lai cho dung  ex Sale,Admin,... => fixed
                            LoginTime = DateTime.Now,
                            SessionID = Session.SessionID,
                            ClientIP = Request.UserHostAddress,
                            Photo = user.Photo,
                            Title = user.Title,
                            Email= user.Email
                            
                        };
                     FormsAuthentication.SetAuthCookie(userData.ToCookieString(), false);
                      return RedirectToAction("Index", "Dashboard");

                    }

                    else
                    {
                        ModelState.AddModelError("LoginError", "Đăng nhập thất bại");
                        ViewBag.Email = email;
                         return View();
                  }

            }


            //if(email=="baoha@gmail.com" && password == "123")
            //{
            //    //ghi nnận phiên đăng nhập của tài khoản
            //    System.Web.Security.FormsAuthentication.SetAuthCookie(email, false);
            //    //chuyển về trang Dashboard
            //   return RedirectToAction("Index", "Dashboard");
            //}
            //else
            //{
            //    // trả về thông báo lỗi : danh sach cac thong bao key : [value]
            //    ModelState.AddModelError("LoginError", "Đăng nhập thất bại");
            //    ViewBag.Email = email;
            //    // tro ve trang hien tai
            //    return View();
            //}
           
        }


        [AllowAnonymous]
        // ALLOW ACTION WITHOUT LOGIN
        public ActionResult ForgotPassword()
        {
            //TODO : Chưa làm Quyên Mật Khẩu
            return View();
        }
    }
}