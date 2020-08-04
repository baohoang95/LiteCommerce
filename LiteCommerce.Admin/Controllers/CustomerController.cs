using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles =WebUserRoles.ACCOUNTANT)]

    public class CustomerController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        
        public ActionResult Index(int page = 1, string searchValue = "" ,string Country ="" ,
            string address="")
        {

            int pageSize = 7;
            int rowCount = 0;
            List<Customer> listOfCustomer = CatalogBLL.ListOfCustomers(page, pageSize, searchValue, out rowCount , Country, address );
            var model = new Models.CustomerPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SeachValue = searchValue,
                Data = listOfCustomer,
                Country = Country,
                Address = address
            };
            return View(model);
            //int rowCount = 0;
            //List<Customer> model = CatalogBLL.ListOfCustomers(1, 10, "", out rowCount);

            //ViewBag.RowCount = rowCount;
            //// truyen model (list) ve View
            //return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            // truyen viewbag.method add/edit 
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    ViewBag.Title = "Create new a Customer";
                    ViewBag.Method ="Add";
                    Customer newCustomer = new Customer()
                    {
                        CustomerID = "" // do id phai nhap
                    };
                    return View(newCustomer);
                }
                else
                {
                    ViewBag.Title = "Edit a Customer";
                    ViewBag.Method = "Update";
                    Customer editCustomer = CatalogBLL.GetCustomer(id);
                    if (editCustomer == null)
                    {
                        return RedirectToAction("Index");
                    }
                    return View(editCustomer);
                }

            }
            catch (Exception ex)
            {
                // ko nen lam dieu nay => tranparent error
                return Content(ex.Message + " : " + ex.StackTrace);
            }
        }

        /// <summary>
        /// su dung ten lop lam tham so do cac field trung 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Input(Customer model , string Method)
        {   // lam that nho them try cacht

            if (string.IsNullOrEmpty(model.CustomerID))
            {
                ModelState.AddModelError("CustomerID", "*");
            }

            if (string.IsNullOrEmpty(model.CompanyName))
            {
                ModelState.AddModelError("CompanyName", "*");
            }

            if (string.IsNullOrEmpty(model.ContactTitle))
            {
                ModelState.AddModelError("ContactTitle", "*");
            }

            if (string.IsNullOrEmpty(model.ContactName))
            {
                ModelState.AddModelError("ContactName", "*");
            }


            if (string.IsNullOrEmpty(model.Address))
            {
                model.Address = "";
            }
            if (string.IsNullOrEmpty(model.City))
            {
                model.City = "";
            }
            if (string.IsNullOrEmpty(model.Country))
            {
                model.Country = "";
            }
            if (string.IsNullOrEmpty(model.Phone))
            {
                model.Phone = "";
            }
            if (string.IsNullOrEmpty(model.Fax))
            {
                model.Fax = "";
            }

        // Chưa tối ưu phần trùng ID=> fixed , check again
            if (Method == "Add")
            {
                foreach (var x in CatalogBLL.ListOfCustomerID())
                {

                    if ((x.CustomerID).Contains(Convert.ToString(model.CustomerID)))
                    {
                        ModelState.AddModelError("CustomerID", "Duplicate ID , Please enter another ID");

                    }
                };
            }


            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Create new a Customer ";
                if (Method == "Add")
                {
                    SetAlert("Add new Customer Fail , Check again!", "danger");
                    //Khi bị lỗi thì hắn đã xóa method nên nó ko phân biệt được
                    ViewBag.Method = "Add";
                }
                else
                {
                    SetAlert("Update new Customer Fail , Check again!", "danger");
                    ViewBag.Method = "Update";
                }
               
                return View(model);
            }


            //: Add dữ liệu vào bị chuyển sang Update, do 
            // =>dung viewbag.method
            if (Method =="Add")
            {
                CatalogBLL.AddCustomer(model);
                SetAlert("Add new Customer success !", "success");
            }
            if (Method == "Update")
            {
                CatalogBLL.UpdateCustomer(model);
                SetAlert("Update Customer success !", "success");
            }
           
            return RedirectToAction("Index");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerIDS"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(string[] customerIDs = null)
        {
            if (customerIDs != null&& CatalogBLL.DeleteCustomers(customerIDs)>0)
            {
                
                SetAlert("Delete Customers success !", "success");
            }
            else
            {
                SetAlert("Delete Customers fail, Because of the associated constraints  !", "danger");

            }
            return RedirectToAction("Index");
        }
    }
}