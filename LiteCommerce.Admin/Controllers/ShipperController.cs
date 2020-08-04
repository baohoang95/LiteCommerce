using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles = WebUserRoles.ACCOUNTANT)]
    public class ShipperController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page =1 , string searchValue="")
        {
            int pageSize = 7;
            int rowCount = 0;
            //Muốn sử dụng out bắt buộc phải chỉ định giá trị trước khi sử dụng
            List<Shipper> listOfShipper = CatalogBLL.ListOfShippers(page, pageSize, searchValue, out rowCount);
            var model = new Models.ShipperPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SeachValue = searchValue,
                Data = listOfShipper
            };
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    ViewBag.Title = "Create new a Shipper";
                    Shipper newShipper = new Shipper()
                    {
                        ShipperID = 0 // do id tu sinh
                    };
                    return View(newShipper);
                }
                else
                {
                    ViewBag.Title = "Edit a Shipper";
                    Shipper editShipper = CatalogBLL.GetShipper(Convert.ToInt32(id));
                    if (editShipper == null)
                    {
                        return RedirectToAction("Index");
                    }
                    return View(editShipper);
                }

            }
            catch (Exception ex)
            {
                // ko nen lam dieu nay => tranparent error
                return Content(ex.Message + " : " + ex.StackTrace);
            }
        }


        /// <summary>
        /// su dung ten lop lam tham so do cac fiel trung 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Input(Shipper model)
        {   // lam that nho them try cacht

            // : Kiểm tra tính hợp lệ của dữ liệu được nhập
            if (string.IsNullOrEmpty(model.CompanyName))
            {
                ModelState.AddModelError("CompanyName", "*");
            }

            if (string.IsNullOrEmpty(model.Phone))
            {
                ModelState.AddModelError("Phone", "*");
            }

            if (!ModelState.IsValid)
            {
                if (model.ShipperID == 0)
                {
                    ViewBag.Title = "Create new a shipper";
                    SetAlert("Add new Shipper Fail , Check again!", "warning");
                }

                else
                {
                    ViewBag.Title = "Update a shipper";
                    SetAlert("Update Shipper Fail , Check again!", "warning");
                }
                   
                return View(model);
            }

            //: Lưu dữ liệu vào DB
            if (model.ShipperID == 0)
            {
                SetAlert("Add new Shipper success !", "success");
                CatalogBLL.AddShipper(model);
            }
            else
            {
                SetAlert("Update Shipper success !", "success");
                CatalogBLL.UpdateShipper(model);
            }
           
            return RedirectToAction("Index");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipperIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int[] shipperIDs = null)
        {
            if (shipperIDs != null&& CatalogBLL.DeleteShippers(shipperIDs)>0)
            {
                SetAlert("Delete shippers success !", "success");
            }
            else
            {
                SetAlert("Delete shippers fail, Because of the associated constraints  !", "danger");
            }
            
            return RedirectToAction("Index");
        }
    }
}