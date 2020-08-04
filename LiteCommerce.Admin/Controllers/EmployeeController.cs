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
    [Authorize(Roles = WebUserRoles.ADMINISTRATOR)]
    public class EmployeeController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
       
        public ActionResult Index(int page = 1, string searchValue = "")
        {

            int pageSize = 3;
            int rowCount = 0;
            //Muốn sử dụng out bắt buộc phải chỉ định giá trị trước khi sử dụng
            List<Employee> listOfEmployee = CatalogBLL.ListOfEmployees(page, pageSize, searchValue, out rowCount);
            var model = new Models.EmployeePaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SeachValue = searchValue,
                Data = listOfEmployee
            };
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       
        [AllowAnonymous]
        public ActionResult ViewDetails( string id="")
        {
            Employee detailEmployee = CatalogBLL.GetEmployee(Convert.ToInt32(id));
            if (detailEmployee == null)
            {
                return RedirectToAction("Index");
            }
            return View(detailEmployee);
        }

        /// <summary>
        /// Input
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
                    ViewBag.Title = "Create new a Employee";
                    Employee newEmployee = new Employee()
                    {
                        EmployeeID = 0 // do id tu sinh
                    };
                    return View(newEmployee);
                }
                else
                {
                    ViewBag.Title = "Edit a Employee";
                    Employee editEmployee = CatalogBLL.GetEmployee(Convert.ToInt32(id));
                    if (editEmployee == null)
                    {
                        return RedirectToAction("Index");
                    }
                    return View(editEmployee);
                }

            }
            catch (Exception ex)
            {
                // ko nen lam dieu nay => tranparent error
                return Content(ex.Message + " : " + ex.StackTrace);
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
        /// <summary>
        /// su dung ten lop lam tham so do cac fiel trung 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Input(Employee model , HttpPostedFileBase uploadFile , string detail, string updateProfile)
        {   // lam that nho them try catch

     
            if (string.IsNullOrEmpty(model.FirstName))
            {
                ModelState.AddModelError("FirstName", "**");
            }

            if (string.IsNullOrEmpty(model.LastName))
            {
                ModelState.AddModelError("LastName", "**");
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError("Email", "**");
            }

            if (string.IsNullOrEmpty(model.HomePhone))
            {
                ModelState.AddModelError("HomePhone", "**");
            }


            if ((model.BirthDate)==null)
            {
                ModelState.AddModelError("BirthDate", "**");
            }


            if ((model.HireDate) == null)
            {
                ModelState.AddModelError("HireDate", "**");
            }

            if (model.HireDate.Year < model.BirthDate.Year+18)
            {
                ModelState.AddModelError("HireDate", "HireDate must be at least 18 years older than  BirthDate");
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
            if (string.IsNullOrEmpty(model.Notes))
            {
                model.Notes = "";
            }
            if (string.IsNullOrEmpty(model.PhotoPath))
            {
                model.PhotoPath = "";
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                // dung ham sinh pass random => Cần mã hóa MD5
                model.Password = RandomString(8);
              
            }

       

            if (!ModelState.IsValid)
            {

                if (model.EmployeeID == 0)
                {
                    ViewBag.Title = "Create new a Employee";
                    SetAlert("Add new Employee Fail , Check again!", "warning");
                }

                else
                {
                    ViewBag.Title = "Update a Employee";
                    SetAlert("Update Employee Fail , Check again!", "warning");
                }
                return View(model);
            }

          
            if(uploadFile!=null)
            {
                //Tạo folder để upload ảnh lên Server
                string folder = Server.MapPath("~/Images/Upload");

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
                model.PhotoPath = fileName;
            }

            if (model.EmployeeID == 0 )
            {
                CatalogBLL.AddEmployee(model);
                SetAlert("Add new Employee success !", "success");
            }
            else if (detail == "detail")
            {
                CatalogBLL.UpdateEmployee(model);
                SetAlert("Update avatar success !", "success");
                return RedirectToAction("Index", "Account", new { id = @model.EmployeeID });
            }
            else if (updateProfile == "updateProfile")
            {
                CatalogBLL.UpdateEmployee(model);
                SetAlert("Update profile success !", "success");
                return RedirectToAction("Index", "Account", new { id = @model.EmployeeID });
            }
            else
            {
                SetAlert("Update Employee success !", "success");
                CatalogBLL.UpdateEmployee(model);
            }
            return RedirectToAction("Index");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierIDS"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int[] employeeIDs = null)
        {
            if (employeeIDs != null && CatalogBLL.DeleteEmployees(employeeIDs)>0)
            {
                // CatalogBLL.DeleteEmployees(employeeIDs);
                SetAlert("Delete Employees success !", "success");
            }
            else
            {
                SetAlert("Delete Employees fail, Because of the associated constraints  !", "danger");

            }
            return RedirectToAction("Index");
        }
    }
}