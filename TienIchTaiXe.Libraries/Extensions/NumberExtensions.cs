using System.Globalization;


namespace TienIchTaiXe.Libraries.Extensions;

public static class NumberExtensions
{
    public static string ToViewString(this int views)
    {
        // Dưới 1.000: giữ nguyên
        if (views < 1_000)
            return views.ToString("N0", CultureInfo.InvariantCulture);

        // 1.000 → < 1.000.000 : dùng k
        if (views < 1_000_000)
        {
            double value = views / 1000d;
            return value % 1 == 0
                ? $"{value:0}k"
                : $"{value:0.#}k";   // vd: 1.2k
        }

        // >= 1.000.000 : dùng M
        double valueM = views / 1_000_000d;
        return valueM % 1 == 0
            ? $"{valueM:0}M"
            : $"{valueM:0.#}M";      // vd: 1.5M
    }
}
