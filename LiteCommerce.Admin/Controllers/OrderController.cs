using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles =WebUserRoles.STAFF)]
    public class OrderController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 

        public ActionResult Index(int page = 1,string searchValue = "" , int idUpdated = 0)
        {
            int pageSize = 20;
            int rowCount = 0;
            List<Order> ListOfOrder = OrderBLL.ListOfOrder(page,pageSize,searchValue , out rowCount);
            var model = new Models.OrderPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                SeachValue = searchValue,
                RowCount = rowCount,
                Data = ListOfOrder,
                IdUpdated = idUpdated
            };
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

       
        public ActionResult Details(string id = "")
        {
                OrderDetail l = OrderBLL.GetOrderDetail(Convert.ToInt32(id));
                     
                  
                    ViewBag.OrderID = l.OrderID;
                    ViewBag.OrderDate = l.OrderDate;
                    ViewBag.UnitPrice = l.UnitPrice;
                    ViewBag.Freight = l.Freight;
                    ViewBag.SupplierName = l.SupplierName;
                    ViewBag.AddressSupp = l.AddressSupp;
                    ViewBag.CitySupp = l.CitySupp;
                    ViewBag.CountrySupp = l.CountrySupp;
                    ViewBag.PhoneSupp = l.PhoneSupp;
                    ViewBag.CustomerName = l.CustomerName;
                    ViewBag.AddressCus = l.AddressCus;
                    ViewBag.PhoneCus = l.PhoneCus;
                    ViewBag.CityCus = l.CityCus;
                    ViewBag.CountryCus = l.CountryCus;
                    ViewBag.Discount = l.Discount;
                    ViewBag.Status = l.Status;
               
                    

                List<OrderDetail> listProduct = OrderBLL.ListProduct(Convert.ToInt32(id));
                if (listProduct == null)
                {
                    return RedirectToAction("Index");
                }
                var model = new Models.OrderDetailPaginationResutl()
                {                 
                    Data = listProduct
                };
                return View(model);      


        }


        [HttpGet]
        public ActionResult Input(string id = "" , string status = "" , int pageCurrent=0)
        {
            try
            {
                if (status == "Shipped" || status == "Delivered")
                {

                    SetAlert("Cannot Update this order , Because status was " + status, "danger");
                    return RedirectToAction("Index");
                }
                else
                {
                    Order editOrder = OrderBLL.GetOrder(Convert.ToInt32(id));
                    if (editOrder == null)
                    {
                        return RedirectToAction("Index");
                    }
                    ViewBag.PageCurrent = pageCurrent;
                    return View(editOrder);
                }
               
               
            }
            catch (Exception ex)
            {
                // ko nen lam dieu nay => tranparent error
                return Content(ex.Message + " : " + ex.StackTrace);
            }

        }
        

        [HttpPost]
        public ActionResult Input(Order model , int pageCurrent=1)
        {
            try
            {
                //Kiểm tra tính hợp lệ của dử liệu
                if (string.IsNullOrEmpty(model.ShipAddress))
                {
                    ModelState.AddModelError("ShipAddress", "*");
                }

                if (string.IsNullOrEmpty(model.ShipCity))
                {
                    ModelState.AddModelError("ShipCity", "*");
                }
                if (string.IsNullOrEmpty(model.ShipCountry))
                {
                    ModelState.AddModelError("ShipCountry", "*");
                }
                if (string.IsNullOrEmpty(model.Status))
                {

                    ModelState.AddModelError("Status", "*");
                }
                if (model.Freight <=-1)
                {
                    ModelState.AddModelError("Freight", "*");
                }
                if (string.IsNullOrEmpty((model.ShipperID).ToString()))
                {
                    ModelState.AddModelError("ShipperID", "*");
                }

                if (!ModelState.IsValid)
                {
                    SetAlert("Update order fail , pls check fields ", "warning");
                    return View(model);
                }

                // : Lưu dữ liệu vào DB
                 OrderBLL.UpdateOrder(model);
                SetAlert("Updated order #" +model.OrderID+ " success !", "success");
            
                return RedirectToAction("Index", "Order", new {idUpdated = model.OrderID ,page= @pageCurrent});

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
        /// <param name="categoryIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int[] orderIDs = null)
        {
            if(orderIDs != null)
            {
                int count = orderIDs.GetLength(0);
                int countIdDeleted = OrderBLL.DeleteOrders(orderIDs);

                if (countIdDeleted == count)
                {

                    SetAlert("Deleted " + @count + " Orders success !", "success");
                }
                else if (countIdDeleted > 0 && countIdDeleted < count)
                {
                    SetAlert("Deleted " + @countIdDeleted + "/" + @count + " Orders success !", "success");
                }
                else
                {
                    SetAlert("Delete " + @count + " Orders fail, Because of status this orders not 'DELIVERED' !", "danger");
                }
                return RedirectToAction("Index");
            }
            else
            {
                SetAlert("Please select order to delete !", "warning");
                return RedirectToAction("Index");
            }
          
        }

    }
}