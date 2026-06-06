namespace TienIchTaiXe.Libraries.Models;

public class DeductionItem
{
    public string Name { get; set; } = string.Empty;
    public string NameAlias { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
}

public class Salary
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string userId { get; set; } = string.Empty; //Mã tài xế
    public decimal? revenue { get; set; } //Doanh thu
    public int tripsTotal { get; set; } //Số cuốc
    public string salaryType { get; set; } = string.Empty; // Loại hình kinh doanh LƯƠNG NGÀY - LƯƠNG THÁNG
    public int businessDays { get; set; } //Số ngày KD
    public decimal? salaryBase { get; set; } //Sau mức ăn chia || Lương cơ bản
    public decimal? deductTotal { get; set; } //Tổng trừ
    public decimal? salaryNet { get; set; } //Lương thực nhận
    public decimal? deductForDeposit { get; set; } //1. Trừ ký quỹ
    public decimal? deductForSalaryAdvance { get; set; } //2. Trừ lương ứng
    public decimal? deductForNegativeSalary { get; set; } //3. Trừ âm lương: Nợ tiền tháng trước, qua tháng này trừ lại vào lương (công ty)
    public decimal? deductForViolationReport { get; set; } //4. Trừ vi phạm biên bản  
    public decimal? no_sua_chua { get; set; } //5. Nợ sửa chữa
    public decimal? haomon_voxe { get; set; } //6. Hao mòn vỏ xe
    public decimal? deductForCharging { get; set; } //7. Sat pin
    public decimal? deductForChargingPenalty { get; set; } //8. phạt sạt: Charging Penalty
    public decimal? deductForTollPayment { get; set; } //9. Trừ tiền qua trạm : Deduction for Toll Payment
    public decimal? deductForSocialInsurance { get; set; } //10. Trừ BHXH
    public decimal? deductForNegativeSalaryPartner { get; set; } //11. Trừ âm lương: Nợ tiền tháng trước, qua tháng này trừ lại vào lương (Thương quyền)
    public decimal? deductForPIT { get; set; } //12. Trừ TNCN - Personal Income Tax Deduction 
    public decimal? deductForOrder { get; set; } //13. Trừ khác
    public string? noteDeductOrder { get; set; } = string.Empty; //14. Ghi chú trừ khác
    public string? salaryDate { get; set; } = string.Empty; //Tháng/năm
    public string? area { get; set; } = string.Empty; // Khu vực
    public DateTime createdAt { get; set; }
    // Liên kết 1-n với chi tiết lương
    public List<SalaryDetails>? Details { get; set; }
}

public class SalaryDetails
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string userId { get; set; } = string.Empty; // Mã tài xế
    public decimal? revenue { get; set; } // Doanh thu
    public decimal? revenueAC { get; set; } // Doanh thu AC giảm 5% GTGT
    public string type { get; set; } = string.Empty; // Loại hình kinh doanh LƯƠNG NGÀY - LƯƠNG THÁNG
    public decimal? salaryBase { get; set; } // Sau mức ăn chia
    public string daterevenues { get; set; } = string.Empty; // Ngày doanh thu chi tiết

    // Foreign key đến bảng Salaries
    public string? salaryId { get; set; }

    public Salary? Salary { get; set; }
    public string? salaryDate { get; set; } = string.Empty; //Tháng/năm
    public string? area { get; set; } = string.Empty; // Khu vực
    public DateTime createdAt { get; set; }
}

public class UpsertSalariesResult
{
    public List<Salary> InsertResults { get; set; } = new();
    public List<Salary> UpdateResults { get; set; } = new();
    public List<string> Errors { get; set; } = new();
    public int InsertedCounts => InsertResults.Count;
    public int UpdatedCounts => UpdateResults.Count;
    public int ErrorCounts => Errors.Count;
}
