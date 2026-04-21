using System.Text.RegularExpressions;

namespace TienIchTaiXe.Libraries.Extensions;

// Các tiện ích mở rộng để chuẩn hóa và validate dữ liệu đầu vào (số điện thoại, biển số xe, email,...)
// Trả về (IsValid, Normalized, Error)
// Ví dụ: (ok, phoneNorm, err) = VnPhone.NormalizeAndValidate(dt.PhoneNumber);
// Nếu ok = true thì phoneNorm đã được chuẩn hóa, ngược lại err sẽ chứa thông báo lỗi chi tiết để hiển thị cho người dùng
// Lưu ý: Các quy tắc chuẩn hóa và validate có thể thay đổi theo yêu cầu thực tế của bạn, ví dụ:
// - Số điện thoại có thể cho phép 11 số nếu bạn muốn hỗ trợ cả số cố định (VD: 02812345678)
// - Biển số xe có thể có nhiều định dạng khác nhau tùy theo loại phương tiện và vùng miền
// Bạn có thể mở rộng thêm các tiện ích khác như email, địa chỉ, tên người dùng,... theo cùng pattern trả về (IsValid, Normalized, Error)

public static class VnPhoneExtension
{
    // Trả về (IsValid, Normalized, Error)
    public static (bool ok, string normalized, string error) NormalizeAndValidate(string? input)
    {
        var raw = (input ?? "").Trim();
        if (string.IsNullOrWhiteSpace(raw))
            return (false, "", "Số điện thoại trống.");

        // Bỏ ký tự ngăn cách
        var s = Regex.Replace(raw, @"[\s\.\-]", "");

        // Đổi +84 / 84 -> 0
        if (s.StartsWith("+84")) s = "0" + s.Substring(3);
        else if (s.StartsWith("84")) s = "0" + s.Substring(2);

        // Chỉ cho phép số
        if (!Regex.IsMatch(s, @"^\d+$"))
            return (false, "", "Số điện thoại chứa ký tự không hợp lệ.");

        // Di động VN chuẩn 10 số
        if (s.Length != 10)
            return (false, "", "Số điện thoại phải đủ 10 chữ số (di động).");

        // Bắt đầu bằng 0 + nhóm đầu phổ biến: 03/05/07/08/09
        if (!Regex.IsMatch(s, @"^0[35789]\d{8}$"))
            return (false, "", "Số điện thoại không đúng định dạng di động VN.");

        return (true, s, "");
    }
}

/* Dùng trong code
var (ok, phoneNorm, err) = VnPhone.NormalizeAndValidate(dt.PhoneNumber);
if (!ok) throw new Exception(err);

dt.PhoneNumber = phoneNorm;
 */

public static class VnPlateExtension
{
    // Ô tô phổ biến: 2 số + 1 chữ + 5 số (VD: 68E01334, 83F00148)
    private static readonly Regex CarPlateRegex =
        new(@"^\d{2}[A-Z]\d{5}$", RegexOptions.Compiled);

    public static (bool ok, string normalized, string error) NormalizeAndValidate(string? input)
    {
        var plate = (input ?? "").Trim();
        if (string.IsNullOrWhiteSpace(plate))
            return (false, "", "Vui lòng nhập biển số xe.");

        // Chỉ giữ chữ + số, bỏ khoảng trắng/ký tự lạ -> UPPER
        var raw = Regex.Replace(plate, @"[^A-Za-z0-9]", "").ToUpperInvariant();

        // Đúng 8 ký tự
        if (raw.Length != 8)
            return (false, "", "Biển số phải đủ 8 ký tự (không tính '-' và '.'). Ví dụ: 68E01334 / 83F00148.");

        // Pattern: 2 số + 1 chữ + 5 số
        if (!CarPlateRegex.IsMatch(raw))
            return (false, "", "Định dạng biển số không hợp lệ. Ví dụ đúng: 83F00148 (2 số, 1 chữ, 5 số).");

        return (true, raw, "");
    }
}