using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    public class CountryController : BaseController
    {

        // GET: Country
        [Authorize(Roles = WebUserRoles.ACCOUNTANT)]
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 2;
            int rowCount = 0;
            searchValue = searchValue.Trim();
            //Muốn sử dụng out bắt buộc phải chỉ định giá trị trước khi sử dụng
            List<Country> listOfCountry = CatalogBLL.ListCountry(page, pageSize, searchValue, out rowCount);
            var model = new Models.CountryPaginationResutl()
            {
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SeachValue = searchValue,
                Data = listOfCountry
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Input(string id = "")
        {
            //{controller}/{action}/{id} : default para name is "id" , if change then has problem
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    ViewBag.Title = "Create new a Country";
                    ViewBag.Method = "Add";
                    Country country = new Country()
                    {
                        CountryId = 0 // do id tu sinh
                    };
                    return View(country);
                }
                else
                {
                    ViewBag.Title = "Edit a Country";
                    ViewBag.Method = "Update";
                    Country editCountry = CatalogBLL.GetCountry(Convert.ToInt32(id));
                    if (editCountry == null)
                    {
                        return RedirectToAction("Index");
                    }
                    return View(editCountry);
                }

            }
            catch (Exception ex)
            {
                // ko nen lam dieu nay => tranparent error
                return Content(ex.Message + " : " + ex.StackTrace);
            }
        }

        [HttpPost]
        public ActionResult Input(Country model ,string Method)
        {   // lam that nho them try catch

            // : Kiểm tra tính hợp lệ của dữ liệu được nhập
            if (string.IsNullOrEmpty(model.CountryName))
            {
                ModelState.AddModelError("CountryName", "*");
            }


            if (Method == "Add"&& ! string.IsNullOrEmpty(model.CountryName))
            {
                foreach (var x in CatalogBLL.ListCountry())
                {

                    if ((x.CountryName).Contains(Convert.ToString(model.CountryName)))
                    {
                        ModelState.AddModelError("CountryName", "Duplicate Country Name , Please enter another Country Name");

                    }
                };
            }


            if (Method == "Update")
            {
                foreach (var x in CatalogBLL.ListCountry())
                {

                    if ((x.CountryName).Contains(Convert.ToString(model.CountryName)))
                    {
                        ModelState.AddModelError("CountryName", "Duplicate Country Name , Please enter another Country Name");

                    }
                };
            }
            if (!ModelState.IsValid)
            {
                
                if (Method == "Add")
                {
                    ViewBag.Title = "Create new a Country ";
                    SetAlert("Add new Country Fail , Check again!", "danger");
                    //Khi bị lỗi thì hắn đã xóa method nên nó ko phân biệt được
                    ViewBag.Method = "Add";
                }
                else
                {
                    ViewBag.Title = "Edit a Country";
                    SetAlert("Update new Country Fail , Check again!", "danger");
                    ViewBag.Method = "Update";
                }


                return View(model);
            }

            //: Lưu dữ liệu vào DB
            if (Method == "Add")
            {
                CatalogBLL.AddCountry(model);
                SetAlert("Add new Country success !", "success");
            }
            if(Method == "Update")
            {
                CatalogBLL.UpdateCountry(model);
                SetAlert("Update Country success !", "success");
            }
            return RedirectToAction("Index");

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int[] countryIDs = null)
        {
            if (countryIDs != null&& CatalogBLL.DeleteCountry(countryIDs) > 0)
            {
                //CatalogBLL.DeleteCountry(CountryId);
                SetAlert("Delete Countries success !", "success");
            }
             //if(CountryId != null && CatalogBLL.DeleteCountry(CountryId)<=0)
             else
            {
                SetAlert("Delete Countries fail, Because of the associated constraints  !", "danger");
                
            }
            return RedirectToAction("Index");
        }
    }


}