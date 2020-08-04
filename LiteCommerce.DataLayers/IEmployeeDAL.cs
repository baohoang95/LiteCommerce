using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// Định nghĩa các phương thức khuôn mẫu của Employee
    /// </summary>
    public interface IEmployeeDAL
    {
        /// <summary>
        /// Danh sách Employee với các tham số
        /// </summary>
        /// <param name="page">Số trang</param>
        /// <param name="pageSize">Kích cỡ trang</param>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <returns></returns>
        List<Employee> List(int page, int pageSize, string searchValue);

        /// <summary>
        /// Hàm trả về số lượng bản ghi tương ứng với giá trị tìm kiếm
        /// </summary>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <returns></returns>
        int Count(string searchValue);

        /// <summary>
        /// Lấy về 1 Employee theo ID
        /// </summary>
        /// <param name="data">ID param </param>
        /// <returns></returns>
        Employee Get(int data);

        /// <summary>
        /// Thêm mới 1 Employee trả về ID , else return a value <0
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Employee data);

        /// <summary>
        /// Cập nhật lại Data Employee 
        /// </summary>
        /// <param name="data">Dữ liệu của Employee</param>
        /// <returns></returns>
        bool Update(Employee data);

        /// <summary>
        /// Xóa nhiều Employee 
        /// </summary>
        /// <param name="EmployeeID">ID Employee</param>
        /// <returns></returns>
        int Delete(int[] EmployeeID);

    }
}
