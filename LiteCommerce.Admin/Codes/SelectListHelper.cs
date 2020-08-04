using LiteCommerce.BusinessLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin
{
   /// <summary>
   /// Danh sách các bảng sử dụng chung  , Dùng cho các DropDown 
   /// </summary>
    public static class SelectListHelper
    {
        /// <summary>
        /// Danh sách các Quốc Gia
        /// </summary>
        /// <param name="allowSelectAll"></param>
        /// <returns></returns>
        public static List<SelectListItem> Contriers(bool allowSelectAll = true )
        {
            //chuyen sang CSDL
            List<SelectListItem> list = new List<SelectListItem>();
            if (allowSelectAll)
            {
                list.Add(new SelectListItem() { Value = "", Text = "----All Countries----" });

            }

            foreach ( var country  in CatalogBLL.ListCountry())
            {
                list.Add(new SelectListItem() { Value = @country.CountryName, Text = @country.CountryName });
            }
           

            return list;
        }

        /// <summary>
        /// Danh sách các Thể Loại của Sản Phẩm
        /// </summary>
        /// <param name="allowSelectAll"></param>
        /// <returns></returns>
        public static List<SelectListItem> Categories (bool allowSelectAll = true)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (allowSelectAll)
            {
                list.Add(new SelectListItem() { Value = "", Text = "----All Categories----" });
                
            }
            //db pageSize=-1 co che Overload
            foreach (var category in CatalogBLL.ListOfCategories())
            {
                list.Add(new SelectListItem()
                {
                    Value = category.CategoryID.ToString(),
                    Text = category.CategoryName
                });
            }


            return list;
        }


        /// <summary>
        /// Danh sách các Nhà Cung Cấp của bảng Product
        /// </summary>
        /// <param name="allowSelectAll"></param>
        /// <returns></returns>
        public static List<SelectListItem> Suppliers(bool allowSelectAll = true)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (allowSelectAll)
            {
                list.Add(new SelectListItem() { Value = "", Text = "----All Suppliers----" });
                
            }
            //db
            foreach (var supplier in  CatalogBLL.ListOfSuppliers())
            {
                list.Add(new SelectListItem() {
                    Value = supplier.SupplierID.ToString(), Text = supplier.CompanyName
                });
            }
               
            return list;
        }

        public static List<SelectListItem> Attributes(bool allowSelectAll = true)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (allowSelectAll)
            {
                list.Add(new SelectListItem() { Value = "0", Text = "---All Attribute Name---" });

            }

            //db
            foreach (var att in CatalogBLL.ListAttributeName())
            {
                list.Add(new SelectListItem()
                {
                    Value = att.AttributeName,
                    Text = att.AttributeName
                });
            }

            return list;
        }

        public static List<SelectListItem> Status(bool allowSelectAll = true)
        {
            //chuyen sang CSDL
            List<SelectListItem> list = new List<SelectListItem>();
            if (allowSelectAll)
            {
                list.Add(new SelectListItem() { Value = "Pending", Text = "Pending" });

            }

                list.Add(new SelectListItem() { Value = "Processing", Text = "Processing" });
                list.Add(new SelectListItem() { Value = "Shipped", Text = "Shipped" });
                list.Add(new SelectListItem() { Value = "Delivered", Text = "Delivered" });



            return list;
        }


        public static List<SelectListItem> Roles(bool allowSelectAll = true)
        {
            //chuyen sang CSDL
            List<SelectListItem> list = new List<SelectListItem>();
            if (allowSelectAll)
            {
                list.Add(new SelectListItem() { Value = "", Text = "---All Roles---" });

            }

            list.Add(new SelectListItem() { Value = "Saleman", Text = "Saleman" });
            list.Add(new SelectListItem() { Value = "Accountant", Text = "Accountant" });
            list.Add(new SelectListItem() { Value = "Administrator", Text = "Administrator" });



            return list;
        }
    }
}