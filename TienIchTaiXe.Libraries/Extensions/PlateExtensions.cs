using System.Text.RegularExpressions;

namespace TienIchTaiXe.Libraries.Extensions;

public static class PlateExtensions
{
    // Regex: 2 số + 1 chữ + 5 số. Ví dụ: 83F00148
    private static readonly Regex PlateRegex =
        new(@"^[0-9]{2}[A-Z][0-9]{5}$", RegexOptions.Compiled);

    /// <summary>
    /// Kiểm tra biển số và chuẩn hóa:
    /// - Không được trống
    /// - Chỉ chữ + số
    /// - Đúng 8 ký tự
    /// - Đúng format 2 số + 1 chữ + 5 số
    /// Trả về: true nếu hợp lệ, kèm biển số đã chuẩn hóa (normalized).
    /// </summary>
    public static bool TryValidateAndNormalizePlate(
        this string? plate,
        out string normalized,
        out string error)
    {
        normalized = string.Empty;
        error = string.Empty;

        // 1. Không được trống
        if (string.IsNullOrWhiteSpace(plate))
        {
            error = "Vui lòng nhập biển số xe.";
            return false;
        }

        // 2. Chỉ giữ chữ + số, bỏ khoảng trắng, ký tự lạ → UPPER
        var raw = new string(plate
                .Where(char.IsLetterOrDigit)
                .ToArray())
            .ToUpperInvariant();

        // 3. Phải đủ đúng 8 ký tự
        if (raw.Length != 8)
        {
            error = "Biển số phải đủ 8 ký tự (chỉ chữ và số, không tính dấu '-' và '.').111";
            return false;
        }

        // 4. Đúng pattern: 2 số + 1 chữ + 5 số
        if (!PlateRegex.IsMatch(raw))
        {
            error = "Định dạng biển số không hợp lệ. Ví dụ đúng: 83F00148 (2 số, 1 chữ, 5 số).";
            return false;
        }

        // ✅ Hợp lệ
        normalized = raw;
        return true;
    }
}
