using System.Text.Json.Serialization;

namespace TienIchTaiXe.Libraries.Models;

public class Violation
{
    [JsonPropertyName("bien_kiem_sat")]
    public string? licensePlate { get; set; }

    [JsonPropertyName("mau_bien")]
    public string? plateColor { get; set; }

    [JsonPropertyName("loai_phuong_tien")]
    public string? vehicleType { get; set; }

    [JsonPropertyName("thoi_gian_vi_pham")]
    public string? violationTime { get; set; }

    [JsonPropertyName("dia_diem_vi_pham")]
    public string? violationLocation { get; set; }

    [JsonPropertyName("hanh_vi_vi_pham")]
    public string? violationBehavior { get; set; }

    [JsonPropertyName("trang_thai")]
    public string? status { get; set; }

    [JsonPropertyName("don_vi_phat_hien_vi_pham")]
    public string? detectingUnit { get; set; }

    [JsonPropertyName("noi_giai_quyet_vu_viec")]
    public string? resolutionPlaceText { get; set; }


    // JSON không có => property sẽ là null
    public string? caseHandlingUnit { get; set; }
}

public class ViolationV2
{
    [JsonPropertyName("BKS")]
    public string? licensePlate { get; set; }

    [JsonPropertyName("Màu biển")]
    public string? plateColor { get; set; }

    [JsonPropertyName("Loại phương tiện")]
    public string? vehicleType { get; set; }

    [JsonPropertyName("Thời gian vi phạm")]
    public string? violationTime { get; set; }

    [JsonPropertyName("Địa điểm vi phạm")]
    public string? violationLocation { get; set; }

    [JsonPropertyName("Hành vi vi phạm")]
    public string? violationBehavior { get; set; }

    [JsonPropertyName("Trạng thái")]
    public string? status { get; set; }

    [JsonPropertyName("Đơn vị phát hiện vi phạm")]
    public string? detectingUnit { get; set; }

    [JsonPropertyName("Nơi giải quyết vụ việc")]
    public string? resolutionPlaceText { get; set; }


    // JSON không có => property sẽ là null
    public string? caseHandlingUnit { get; set; }
}

/*
 * 
licensePlate
→ Biển số xe của phương tiện vi phạm.

plateColor
→ Màu nền và màu chữ của biển số.

vehicleType
→ Loại phương tiện (ô tô, xe máy, xe tải…).

violationTime
→ Thời gian xảy ra vi phạm (ngày/giờ).

violationLocation
→ Địa điểm cụ thể nơi ghi nhận hành vi vi phạm.

violationBehavior
→ Mã và mô tả hành vi vi phạm theo quy định.

status
→ Tình trạng hồ sơ vi phạm (đã xử phạt / chưa xử phạt).

detectingUnit
→ Đơn vị nghiệp vụ phát hiện và lập biên bản vi phạm.
 
caseHandlingUnit
→ Đơn vị hoặc cơ quan có trách nhiệm giải quyết hồ sơ vi phạm, xử lý thủ tục, ra quyết định xử phạt.
*/

public class ViolationNoItemException
{
    [JsonPropertyName("code")] //0: không tìm thấy
    public string? code { get; set; }
    [JsonPropertyName("message")] // No item to return was found
    public string? message { get; set; }
}

public class TrafficViolationLookupResult
{
    /// <summary>
    /// Biển kiểm soát: 94H-010.48
    /// </summary>
    public string PlateNumber { get; set; } = string.Empty;
    /// <summary>
    /// Màu biển: "Nền màu vàng, chữ và số màu đen"
    /// </summary>
    public string? PlateColorDescription { get; set; }
    /// <summary>
    /// Loại phương tiện: "Ô tô"
    /// </summary>
    public string? VehicleType { get; set; }
    /// <summary>
    /// Thời gian vi phạm (raw text): "12:07, 04/08/2025"
    /// </summary>
    public string? ViolationTimeText { get; set; }
    /// <summary>
    /// Thời gian vi phạm parse được (nếu parse được)
    /// </summary>
    public DateTime? ViolationTime { get; set; }
    /// <summary>
    /// Địa điểm vi phạm
    /// </summary>
    public string? ViolationLocation { get; set; }
    /// <summary>
    /// Hành vi vi phạm (có thể kèm mã điều)
    /// </summary>
    public string? ViolationBehavior { get; set; }
    /// <summary>
    /// Trạng thái (raw): "Chưa xử phạt"
    /// </summary>
    public string? StatusText { get; set; }
    /// <summary>
    /// Có thể map thêm mã màu badge nếu bạn muốn
    /// Ví dụ: "text-danger", "text-success"...
    /// </summary>
    public string? StatusCssClass { get; set; }
    /// <summary>
    /// Đơn vị phát hiện vi phạm
    /// </summary>
    public string? DetectingUnit { get; set; }
    /// <summary>
    /// Nơi giải quyết vụ việc (ô chính trong form; đôi khi rỗng)
    /// </summary>
    public string? ResolutionPlaceText { get; set; }
    /// <summary>
    /// Danh sách đơn vị giải quyết được liệt kê bên dưới (1., 2., ...)
    /// </summary>
    public List<ResolutionUnit> ResolutionUnits { get; set; } = new();
    /// <summary>
    /// Dùng để debug/troubleshoot nếu cần lưu HTML gốc
    /// </summary>
    public string? RawHtml { get; set; }
}

/// <summary>
/// Một đơn vị giải quyết vụ việc được liệt kê
/// </summary>
public class ResolutionUnit
{
    /// <summary>
    /// Số thứ tự: 1, 2, ...
    /// </summary>
    public int Index { get; set; }
    /// <summary>
    /// Tên đơn vị:
    /// "Đội CSGT đường bộ - Phòng Cảnh sát giao thông - Công an Tỉnh Vĩnh Long"
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Địa chỉ: "114A Phú Chiến, phường Phú Khương, Vĩnh Long"
    /// </summary>
    public string? Address { get; set; }
    /// <summary>
    /// Số điện thoại liên hệ: "02703.833939"
    /// </summary>
    public string? Phone { get; set; }
}