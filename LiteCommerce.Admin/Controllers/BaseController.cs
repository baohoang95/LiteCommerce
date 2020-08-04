using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        //public ActionResult Index()
        //{
        //    return View();
        //}

        protected void SetAlert (string messenge , string type)
        {
            TempData["AlertMessage"] = messenge;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "info")
            {
                TempData["AlertType"] = "alert-info";
            }
           else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}