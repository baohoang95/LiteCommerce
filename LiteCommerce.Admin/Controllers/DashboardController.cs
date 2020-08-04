using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    /// <summary>
    /// mô tả các chức năng. 
    /// 
    /// </summary>
    
    [Authorize]
    // TAT CAC CAC HAM TRONG CLASS SE PHAI LOGIN MOI CHO PHEP THUC HIEN
    public class DashboardController : Controller
    {
        /// <summary>
        /// Trang tổng quan 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}