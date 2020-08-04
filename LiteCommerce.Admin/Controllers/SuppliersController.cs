using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [Authorize(Roles = WebUserRoles.ACCOUNTANT)]
    public class SuppliersController : BaseController
    {
        /// <summary>
        /// page nen co gia tri mac dinh
        /// </summary>
        /// <returns></returns>
     
        public ActionResult Index(int page=1, string searchValue = "")
        {

            int pageSize = 4;
            int rowCount = 0;
             //Muốn sử dụng out bắt buộc phải chỉ định giá trị trước khi sử dụng
            List<Supplier> listOfSupplier = CatalogBLL.ListOfSuppliers(page, pageSize, searchValue, out rowCount);
            var model = new Models.SupplierPaginationResult()
            {
                // set :
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SeachValue = searchValue,
                Data = listOfSupplier
            };
            return View(model);

            //int pageSize = 3;
            //int rowCount = 0;
            //List<Supplier> model = CatalogBLL.ListOfSuppliers(page, pageSize, searchValue, out rowCount);

            //ViewBag.RowCount = rowCount;
            //ViewData
            //// truyen model (list) ve View
            //return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Input(string id ="")
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    ViewBag.Title = "Create new a Supplier";
                    Supplier newSupplier = new Supplier()
                    {
                        SupplierID = 0 // do id tu sinh
                    };
                    return View(newSupplier);
                }
                else
                {
                    ViewBag.Title = "Edit a Supplier";
                    Supplier editSupplier = CatalogBLL.GetSupplier(Convert.ToInt32(id));
                    if(editSupplier==null)
                    {
                        return RedirectToAction("Index");
                    } return View(editSupplier);
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
        public ActionResult Input(Supplier model)
        {   // lam that nho them try cacht

            // : Kiểm tra tính hợp lệ của dữ liệu được nhập
            if (string.IsNullOrEmpty(model.CompanyName))
            {
                ModelState.AddModelError("CompanyName", "CompanyName expected");
            }

            if (string.IsNullOrEmpty(model.ContactTitle))
            {
                ModelState.AddModelError("ContactTitle", "ContactTitle expected");
            }

            if (string.IsNullOrEmpty(model.ContactName))
            {
                ModelState.AddModelError("ContactName", "ContactName expected");
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

            if (string.IsNullOrEmpty(model.HomePage))
            {
                model.HomePage = "";
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Create new a Supplier ";
                SetAlert("Add new Supplier Fail , Check again!", "danger");
                return View(model);
            }

            // : Lưu dữ liệu vào DB
            if (model.SupplierID == 0)
                {
                SetAlert("Add new Supplier success !", "success");
                CatalogBLL.AddSupplier(model);
                }
                else
            {
                SetAlert("Update Supplier success !", "success");
                CatalogBLL.UpdateSupplier(model);
                }
           
            return RedirectToAction("Index");
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierIDS"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete (int[] supplierIDs =null)
        {
            if (supplierIDs != null&& CatalogBLL.DeleteSuppliers(supplierIDs)>0)
            {
                //CatalogBLL.DeleteSuppliers(supplierIDs);
                SetAlert("Delete Suppliers success !", "success");
            }
            else
            {
                SetAlert("Delete Suppliers fail, Because of the associated constraints  !", "danger");

            }
            return RedirectToAction("Index");
        }
    }
}