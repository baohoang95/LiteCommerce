using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
using System.Data.SqlClient;
using System.Data;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class OrderDetailDAL : IOrderDetailDAL
    {
        private string connectionString;

        public OrderDetailDAL (string _connectionString)
        {
            this.connectionString = _connectionString;
        }

        public OrderDetail Get(int orderID)
        {
            OrderDetail data = null;
          
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                           cmd.CommandText = @"SELECT        Orders.OrderID,OrderDate,Freight,Status,Suppliers.CompanyName,Suppliers.Address as SuppAdd,Suppliers.City as CitySupp,Suppliers.Country as CountrySupp,
				                    Suppliers.Phone , Customers.ContactName,Customers.Address as CusAdd ,Customers.City as CusCity, Customers.Country as CusCountry , Customers.Phone as CusPhone,
				                    Products.ProductID,ProductName,Descriptions,
				                    OrderDetails.UnitPrice,Quantity,Discount,OrderDetails.OrderID as ID,
                               (OrderDetails.UnitPrice*OrderDetails.Quantity) as SubTotal
                    FROM            Customers INNER JOIN
                                             Orders ON Customers.CustomerID = Orders.CustomerID INNER JOIN
                                             OrderDetails ON Orders.OrderID = OrderDetails.OrderID INNER JOIN
                                             Products ON OrderDetails.ProductID = Products.ProductID INNER JOIN
                                             Suppliers ON Products.SupplierID = Suppliers.SupplierID
                    WHERE OrderDetails.OrderID = @orderID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@orderID", orderID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                      
                        data = new OrderDetail()
                        {
                            //Order
                            OrderID = Convert.ToInt32(dbReader["ID"]),
                            OrderDate = Convert.ToDateTime(dbReader["OrderDate"]),
                           // UnitPrice= Convert.ToDecimal(dbReader["UnitPrice"]),
                            Freight= Convert.ToDecimal(dbReader["Freight"]),
                            Status = Convert.ToString(dbReader["Status"]),
                            //Suppliers
                            SupplierName = Convert.ToString(dbReader["CompanyName"]),
                            AddressSupp = Convert.ToString(dbReader["SuppAdd"]),
                            CitySupp = Convert.ToString(dbReader["CitySupp"]),
                            CountrySupp = Convert.ToString(dbReader["CountrySupp"]),
                            PhoneSupp = Convert.ToString(dbReader["Phone"]),

                            //Customer
                            CustomerName = Convert.ToString(dbReader["ContactName"]),
                            AddressCus = Convert.ToString(dbReader["CusAdd"]),
                            PhoneCus = Convert.ToString(dbReader["CusPhone"]),
                            CityCus = Convert.ToString(dbReader["CusCity"]),
                            CountryCus = Convert.ToString(dbReader["CusCountry"]),
                            // Products* => fixed
                            //ProductID = Convert.ToInt32(dbReader["ProductID"]),
                            //ProductName = Convert.ToString(dbReader["ProductName"]),
                            //Descriptions = Convert.ToString(dbReader["Descriptions"]),
                           
                           // SubTotal =Convert.ToDecimal(dbReader["SubTotal"]),
                           // Discount = Convert.ToDecimal(dbReader["Discount"])
                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

        public List<OrderDetail> ListProduct(int orderID)
        {
            List<OrderDetail> data = new List<OrderDetail>();
          
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT OrderDetails.Quantity,OrderDetails.UnitPrice,  Products.ProductID, Products.ProductName, Products.Descriptions, OrderDetails.UnitPrice * OrderDetails.Quantity AS SubTotal
                                FROM   OrderDetails INNER JOIN
                                                         Products ON OrderDetails.ProductID = Products.ProductID
                                       
                                WHERE  OrderID = @orderID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@orderID", orderID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {

                        data.Add(new OrderDetail()
                        {
                            Quantity = Convert.ToInt32(dbReader["Quantity"]),
                            ProductID = Convert.ToInt32(dbReader["ProductID"]),
                            ProductName = Convert.ToString(dbReader["ProductName"]),
                            Descriptions = Convert.ToString(dbReader["Descriptions"]),
                            SubTotal = Convert.ToDecimal(dbReader["SubTotal"]),
                            UnitPrice= Convert.ToDecimal(dbReader["UnitPrice"]),
                            

                        });
                    }
                }

                connection.Close();
            }

            return data;
         
        }
    }
}
