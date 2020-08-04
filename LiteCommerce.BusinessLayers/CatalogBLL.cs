using LiteCommerce.DataLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DataLayers.SqlServer;

namespace LiteCommerce.BusinessLayers
{
    /// <summary>
    /// => Cung cấp các chứng năng các xử lý nghiệp vụ quản lý như : supp , cus , prod....
    /// co the nhóm business : tương ứng 1 nghiệp vụ
    /// </summary>
    public class CatalogBLL
    {

        /// <summary>
        /// Hàm khởi tạo với chuổi kết nối | cơ chế đa hình
        /// Nguyen tac D
        /// => Co cau du lieu : Da hình ex fake , sql , mysql ..etc... dùng cái gì thì new cái đó
        /// dùng new pram dbType
        /// </summary>
        /// <param name="connectionString">Chuổi kết nối truyền từ lớp trên</param>
        public static void Initialize(string connectionString)
        {
            SupplierDB = new DataLayers.SqlServer.SupplierDAL(connectionString);
            CustomerDB = new DataLayers.SqlServer.CustomerDAL(connectionString);
            ShipperDB = new DataLayers.SqlServer.ShipperDAL(connectionString);
            CategoryDB = new DataLayers.SqlServer.CategoryDAL(connectionString);
            EmployeeDB = new DataLayers.SqlServer.EmployeeDAL(connectionString);
            ProductDB = new DataLayers.SqlServer.ProductDAL(connectionString);
            CountryDB = new DataLayers.SqlServer.CountryDAL(connectionString);
            AttributeDB = new DataLayers.SqlServer.AttributeDAL(connectionString);
        }

        //#region Khai báo các chức năng xử lý nghiệp vụ

        /// <summary>
        /// khai báo đối tương SupplierDB có kiểu ISupplierDAL
        /// Dùng để truyền kết nối đến lớp DAL
        /// </summary>
        private static ISupplierDAL SupplierDB { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private static ICustomerDAL CustomerDB { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private static IShipperDAL ShipperDB { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private static ICategoryDAL CategoryDB { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private static IEmployeeDAL EmployeeDB { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private static IProductDAL ProductDB { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private static ICountryDAL CountryDB { get; set; }

        /// <summary>
        /// lấy danh sách các Attribute Name 
        /// </summary>
        private static IAttributeDAL AttributeDB { get; set; }

        public static List<ProductAttribute> ListAttributeName()
        {
            return AttributeDB.List();
        }

        /// <summary>
        /// Gọi tới lớp DAL để thực thi nghiệp vụ trả về danh sách Country
        /// </summary>
        /// <returns></returns>
        public static List<Country> ListCountry (int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;


            rowCount = CountryDB.Count(searchValue);
            return CountryDB.List(page, pageSize, searchValue);
        }
        public static List<Country> ListCountry()
        {
            return CountryDB.List(1, -1, "");
        }

        public static Country GetCountry(int supplierID)
        {
            return CountryDB.Get(supplierID);
        }

        /// <summary>
        /// Thêm 1 Customer, trả về id string Customer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCountry(Country data)
        {
            return CountryDB.Add(data);
        }


        /// <summary>
        /// Cập nhật lại Customer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCountry(Country data)
        {
            return CountryDB.Update(data);
        }

        /// <summary>
        /// Xoa nhieu theo iD, tra ve số country đã xóa
        /// </summary>
        /// <param name="countryIDs"></param>
        /// <returns></returns>
        public static int DeleteCountry(int[] countryIDs)
        {
            return CountryDB.Delete(countryIDs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// rowCount tham tri
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount">Tham chiếu (có thay đổi giá trị sau khi kết thúc hàm)</param>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;


            rowCount = SupplierDB.Count(searchValue);
            return SupplierDB.List(page, pageSize, searchValue);
        }

        /// <summary>
        /// Co che nap chong , overload => tranh loi khi pageSize =-1
        /// </summary>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers()
        {
            return SupplierDB.List(1, -1, "");
        }

        /// <summary>
        /// Lấy 1 Supplier
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public static Supplier GetSupplier(int supplierID)
        {
            return SupplierDB.Get(supplierID);
        }

        /// <summary>
        /// Thêm 1 Supplier
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddSupplier(Supplier data)
        {
            return SupplierDB.Add(data);
        }


        /// <summary>
        /// Cập nhật lại Supplier
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateSupplier(Supplier data)
        {
            return SupplierDB.Update(data);
        }
        /// <summary>
        /// Xoa nhieu theo iD
        /// </summary>
        /// <param name="supplierIDs"></param>
        /// <returns></returns>
        public static int DeleteSuppliers(int[] supplierIDs)
        {
            return SupplierDB.Delete(supplierIDs);
        }



        /// <summary>
        /// 
        /// </summary>
        /// rowCount tham tri
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount">Tham chiếu (có thay đổi giá trị sau khi kết thúc hàm)</param>
        /// <returns></returns>
        public static List<Product> ListOfProducts(int page , int pageSize, string searchValue, out int rowCount,
            string CategoryID, string SupplierID)
        {
            if (page < 1)
                page = 1;
            if (pageSize<0)
                pageSize = 20;
           
            //Tham chiếu : thay đổi giá trị RowCount để lớp phía trên Sử dụng lại
            rowCount = ProductDB.Count(searchValue ,CategoryID,SupplierID);
            return ProductDB.List(page, pageSize, searchValue, CategoryID, SupplierID);
        }


        /// <summary>
        /// Lấy 1 Product
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public static Product GetProduct(int ProductID)
        {
            return ProductDB.Get(ProductID);
        }

        /// <summary>
        /// Thêm 1 Product
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int  AddProduct(Product data)
        {
            return ProductDB.Add(data);
        }


        /// <summary>
        /// Cập nhật lại Product
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateProduct(Product data)
        {
            return ProductDB.Update(data);
        }
        /// <summary>
        /// Xoa nhieu theo iD
        /// </summary>
        /// <param name="ProductIDs"></param>
        /// <returns></returns>
        public static int DeleteProducts(int[] ProductIDs)
        {
            return ProductDB.Delete(ProductIDs);
        }


        /// <summary>
        /// Danh sách Customer
        /// </summary>
        /// <param name="page">Số trang</param>
        /// <param name="pageSize">Kích cở trang</param>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <param name="rowCount">Số bản ghi ứng với searchValue</param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(int page, int pageSize, string searchValue, out int rowCount , string Country , string address)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = CustomerDB.Count(searchValue ,Country ,address);
            return CustomerDB.List(page, pageSize, searchValue , Country ,address);
        }
        public static List<Customer> ListOfCustomerID()
        {
            return CustomerDB.List(1, -1, "","","");
        }
        /// <summary>
        /// Lấy 1 Customer 
        /// </summary>
        /// <param name="customerID">String</param>
        /// <returns></returns>
        public static Customer GetCustomer(string customerID)
        {
            return CustomerDB.Get(customerID);
        }

        /// <summary>
        /// Thêm 1 Customer, trả về id string Customer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string AddCustomer(Customer data)
        {
            return CustomerDB.Add(data);
        }


        /// <summary>
        /// Cập nhật lại Customer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer data)
        {
            return CustomerDB.Update(data);
        }

        /// <summary>
        /// Xoa nhieu theo iD, tra ve số Customer đã xóa
        /// </summary>
        /// <param name="customerIDs"></param>
        /// <returns></returns>
        public static int DeleteCustomers(string[] customerIDs)
        {
            return CustomerDB.Delete(customerIDs);
        }



        /// <summary>
        /// Danh sách Shippers
        /// </summary>
        /// <param name="page">Số trang</param>
        /// <param name="pageSize">Kích cở trang</param>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <param name="rowCount">Số bản ghi ứng với searchValue</param>
        /// <returns></returns>
        public static List<Shipper> ListOfShippers(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = ShipperDB.Count(searchValue);
            return ShipperDB.List(page, pageSize, searchValue);
        }

        /// <summary>
        /// Lấy 1 Shipper
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public static Shipper GetShipper(int shipperID)
        {
            return ShipperDB.Get(shipperID);
        }

        /// <summary>
        /// Thêm 1 Shipper
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddShipper(Shipper data)
        {
            return ShipperDB.Add(data);
        }


        /// <summary>
        /// Cập nhật lại Shipper
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateShipper(Shipper data)
        {
            return ShipperDB.Update(data);
        }
        /// <summary>
        /// Xoa nhieu theo iD
        /// </summary>
        /// <param name="ShipperIDs"></param>
        /// <returns></returns>
        public static int DeleteShippers(int[] shipperIDs)
        {
            return ShipperDB.Delele(shipperIDs);
        }

        /// <summary>
        /// Danh sách Categories
        /// </summary>
        /// <param name="page">Số trang</param>
        /// <param name="pageSize">Kích cở trang</param>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <param name="rowCount">Số bản ghi ứng với searchValue</param>
        /// <returns></returns>
        public static List<Category> ListOfCategories(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = CategoryDB.Count(searchValue);
            return CategoryDB.List(page, pageSize, searchValue);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Category> ListOfCategories()
        {
            return CategoryDB.List(1, -1, "");
        }
      

        /// <summary>
        /// Lấy 1 Category
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <returns></returns>
        public static Category GetCategory(int CategoryID)
        {
            return CategoryDB.Get(CategoryID);
        }

        /// <summary>
        /// Thêm 1 Category
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCategory(Category data)
        {
            return CategoryDB.Add(data);
        }


        /// <summary>
        /// Cập nhật lại Category
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCategory(Category data)
        {
            return CategoryDB.Update(data);
        }
        /// <summary>
        /// Xoa nhieu theo iD
        /// </summary>
        /// <param name="CategoryIDs"></param>
        /// <returns></returns>
        public static int DeleteCategoris(int[] CategoryIDs)
        {
            return CategoryDB.Delete(CategoryIDs);
        }




        /// <summary>
        /// Danh sách Employees
        /// </summary>
        /// <param name="page">Số trang</param>
        /// <param name="pageSize">Kích cở trang</param>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <param name="rowCount">Số bản ghi ứng với searchValue</param>
        /// <returns></returns>
        public static List<Employee> ListOfEmployees(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = EmployeeDB.Count(searchValue);
            return EmployeeDB.List(page, pageSize, searchValue);
        }

        /// <summary>
        /// Lấy 1 Employee
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public static Employee GetEmployee(int employeeID)
        {
            return EmployeeDB.Get(employeeID);
        }

        /// <summary>
        /// Thêm 1 Employee
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddEmployee(Employee data)
        {
            return EmployeeDB.Add(data);
        }


        /// <summary>
        /// Cập nhật lại Employee
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateEmployee(Employee data)
        {
            return EmployeeDB.Update(data);
        }
        /// <summary>
        /// Xoa nhieu theo iD
        /// </summary>
        /// <param name="employeeIDs"></param>
        /// <returns></returns>
        public static int DeleteEmployees(int[] employeeIDs)
        {
            return EmployeeDB.Delete(employeeIDs);
        }
    }
}
