using TienIchTaiXe.Libraries.Models;

namespace TienIchTaiXe.Libraries.Entities;
public class CheckerSalaryDto
{
    /// <summary>
    /// Thông tin tổng hợp lương (bản ghi chính)
    /// </summary>
    public Salary Salary { get; set; } = new();

    /// <summary>
    /// Danh sách chi tiết lương theo ngày (daily breakdown)
    /// </summary>
    public List<SalaryDetails> Details { get; set; } = new();
}