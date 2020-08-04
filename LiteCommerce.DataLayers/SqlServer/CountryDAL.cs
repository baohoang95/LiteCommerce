using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;

using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

namespace LiteCommerce.DataLayers.SqlServer
{

    public class CountryDAL : ICountryDAL
    {
        private string connectionString;

        /// <summary>
        /// Ham khoi tao voi chuoi ket noi
        /// </summary>
        /// <param name="connectionString"></param>
        public CountryDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int Add(Country data)
        {
            int countryId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Countries
                                          (
	                                          CountryName
	                                       
                                          )
                                          VALUES
                                          (
	                                          @CountryName
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CountryName", data.CountryName);


                countryId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return countryId;
        }

        public int Count(string searchValue)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            //1.thiết lập kết nối
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //1.1 mở kết nối
                connection.Open();

                //2. tạo và xử lý lệnh thông qua nghiệp vụ tương ứng
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select COUNT(*) from Countries where  @searchValue = N'' or CountryName like @searchValue"; ;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());
                //1.2 đóng kết nối
                connection.Close();
            }

            //3.tra ve kết quả thông đầu ra 
            return count;
        }

        public int Delete(int[] CountryId)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Countries
                                            WHERE(CountryId = @CountryId)
                                              AND(CountryName NOT IN(SELECT Country FROM Customers))
                                              AND(CountryName NOT IN(SELECT Country FROM Employees)) 
                                              AND(CountryName NOT IN(SELECT Country FROM Suppliers)) ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@CountryId", SqlDbType.Int);
                foreach (int countryId in CountryId)
                {
                    cmd.Parameters["@CountryId"].Value = countryId;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }

        public Country Get(int CountryId)
        {
            Country data1 = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Countries WHERE CountryId = @CountryId";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CountryId", CountryId);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data1 = new Country()
                        {

                            CountryName = Convert.ToString(dbReader["CountryName"]),
                            CountryId = Convert.ToInt32(dbReader["CountryId"])
                        };
                    }
                }

                connection.Close();
            }
            return data1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Country> List(int page, int pageSize, string searchValue)
        {
            List<Country> data = new List<Country>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //mở kết nối SELECT CountryName FROM Countries
                connection.Open();

                //2. tạo và xử lý lệnh thông qua nghiệp vụ tương ứng
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * 
                                    from (select ROW_NUMBER() over (order by CountryName) as RowNumber, 
		                                    Countries.*
		                                    from Countries
		                                    where (@searchValue = N'') or (CountryName like @searchValue)
		
	                                     ) as t

                                    where (@pageSize=-1) or t.RowNumber between (@page-1)*@pageSize+1 and @page*@pageSize
                                    order by t.RowNumber";

                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                //ExecuteReader trả về nhiều giá trị
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Country()
                        {
                            CountryName = Convert.ToString(dbReader["CountryName"]),
                            CountryId = Convert.ToInt32(dbReader["CountryId"])
                           
                        });
                    }
                }

                //đóng kết nối
                connection.Close();
            }

            return data;
        }

        public bool Update(Country data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Countries
                                           SET CountryName = @CountryName 
                                             
                                           
                                          WHERE CountryId = @CountryId";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CountryName", data.CountryName);
                cmd.Parameters.AddWithValue("@CountryId", data.CountryId);


                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }
            // fasle
            return rowsAffected > 0;
        }
    }
}
