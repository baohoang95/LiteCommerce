using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles = WebUserRoles.ACCOUNTANT)]
    public class ProductController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult Index(string Category = "", string Supplier = "", string SeachValue = "", int page = 1)
        {
           
            int pageSize = 5;
            int rowCount = 0;
         // Class CatalogBLL is static => dont need new instance class
            List<Product> listOfProduct = CatalogBLL.ListOfProducts(page, pageSize, SeachValue, out rowCount , Category, Supplier);
            // class ProductPaginationResult is normar class => must be new instance
            Models.ProductPaginationResult model = new Models.ProductPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SeachValue = SeachValue,
                Category = Category,
                Supplier = Supplier,
                Data = listOfProduct

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
                    ViewBag.Title = "Create new a Product";
                    Product newProduct = new Product()
                    {
                        ProductID = 0 // do id tu sinh
                    };
                    return View(newProduct);
                }
                else
                {
                    ViewBag.Title = "Edit a Product";
                    Product editProduct = CatalogBLL.GetProduct(Convert.ToInt32(id));
                    if (editProduct == null)
                    {
                        return RedirectToAction("Index");
                    }
                    return View(editProduct);
                }

            }
            catch (Exception ex)
            {
                // ko nen lam dieu nay => tranparent error
                return Content(ex.Message + " : " + ex.StackTrace);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Input (Product data , HttpPostedFileBase uploadFile)
        {
            //Kiểm tra dữ liệu đầu vào
            if (data.CategoryID.ToString()=="0")
            {
                ModelState.AddModelError("CategoryID", "*");
            }

            if (data.SupplierID.ToString() == "0")
            {
                ModelState.AddModelError("SupplierID", "*");
            }

            if (string.IsNullOrEmpty(data.ProductName))
            {
                ModelState.AddModelError("ProductName", "**");
            }
            if (string.IsNullOrEmpty(data.QuantityPerUnit))
            {
                ModelState.AddModelError("QuantityPerUnit", "**");
            }

            //UnitPrice kiểu decimal
            if (string.IsNullOrEmpty(data.UnitPrice.ToString()))
            {
                ModelState.AddModelError("UnitPrice", "**");
            }
            if (string.IsNullOrEmpty(data.PhotoPath))
            {
                data.PhotoPath = "";
            }
            if (string.IsNullOrEmpty(data.Descriptions))
            {
                data.Descriptions = "";
            }
            //Nếu nó chưa thành công thì ở lại trang hiện tại
            if (!ModelState.IsValid)
            {
                if (data.ProductID == 0)
                {
                    ViewBag.Title = "Create new a Product";
                    SetAlert("Add new Product Fail , Check again!", "warning");
                }

                else
                {
                    ViewBag.Title = "Update a Product";
                    SetAlert("Update Product Fail , Check again!", "warning");
                }
             
                return View(data);
            }
            //: new update
            if (uploadFile != null)
            {
                //Tạo folder để upload ảnh lên Server
                string folder = Server.MapPath("~/Images/ProductUpload");

                // string fileName = uploadFile.FileName;  => dễ bị trùng khi client chọn lại
                // => Solution : Sinh fileName theo giây phút để tránh trùng lặp
                //c1 : Cách ghép chuổi fileName
                string fileName = $"{DateTime.Now.Ticks}{Path.GetExtension(uploadFile.FileName)}";
                //cach 2
                // string fileName1 = string.Format("{0}{1}" ,DateTime.Now.Ticks,Path.GetExtension(uploadFile.FileName));

                string filePath = Path.Combine(folder, fileName);

                //Upload ảnh lên Server
                uploadFile.SaveAs(filePath);

                //  return content|redirectoaction()
                // return Json (model, JsonRequestBehavior.AllowGet);

                //Gán dữ liệu cho PhotoPath
                data.PhotoPath = fileName;
            }

            if (data.ProductID == 0)
            {
                CatalogBLL.AddProduct(data);
                SetAlert("Add new Product success !", "success");
            }
            else
            {

                CatalogBLL.UpdateProduct(data);
                SetAlert("Update Product success !", "success");
            }
          


            return RedirectToAction("Index");
        }

        /// <summary>
        /// Xoa nhieu products
        /// </summary>
        /// <param name="productIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete (int[] productIDs=null)
        {
            if (productIDs != null&& CatalogBLL.DeleteProducts(productIDs)>0)
            {
                SetAlert("Delete Products success !", "success");
            }
            else
            {
                SetAlert("Delete Products fail, Because of the associated constraints  !", "danger");
            }
            return RedirectToAction("Index");
        }

    }
}