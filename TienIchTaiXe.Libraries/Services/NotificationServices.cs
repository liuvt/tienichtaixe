using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using TienIchTaiXe.Extensions;
using TienIchTaiXe.Libraries.Extensions;
using TienIchTaiXe.Libraries.Models.GGSheets;
using TienIchTaiXe.Libraries.Services.Interfaces;

namespace TienIchTaiXe.Libraries.Services;

public class NotificationServices : INotificationServices
{
    #region Constructor 
    //For Connection to Spread
    private SheetsService sheetsService;
    private readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
    private readonly string CredentialGGSheetService = "ggsheetaccount.json";
    private readonly string AppName = "tienichtaixe-notify"; // <-- ASCII, không dấu
    private readonly string SpreadSheetId = "1yNpYJOfXqLOinjKpwgAwVyyi5V8KEF9fbEtlewrGLlY";

    // For Sheet
    private readonly string sheetRegister = "Đăng ký";
    private readonly string sheetFbMessage = "FbMessage";

    public NotificationServices()
    {
        //File xác thực google tài khoản
        GoogleCredential credential;
        using (var stream = new FileStream(CredentialGGSheetService, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream)
                .CreateScoped(Scopes);
        }

        // Đăng ký service
        sheetsService = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = AppName,
        });
    }
    #endregion

    #region GET FULL DATAS
    // Lấy toàn bộ danh sách
    public async Task<List<GGNotification>> GetsGGNotification()
    {
        var dts = new List<GGNotification>();
        var range = $"{sheetRegister}!F2:G";
        var values = await sheetsService.ltvGetSheetValuesAsync(SpreadSheetId, range);
        if (values == null || values.Count == 0)
        {
            throw new Exception("Không có dữ liệu sheet.");
        }

        foreach (var item in values)
        {

            dts.Add(new GGNotification
            {
                Plate = item.ltvGetValueString(0),
                PhoneNumber = item.ltvGetValueString(1)
            });
        }

        return dts;
    }

    // Lọc lại danh sách theo mã 
    public async Task<GGNotification?> GetGGNotification(string plate)
    {
        if (string.IsNullOrWhiteSpace(plate)) return null;

        var dts = await GetsGGNotification() ?? new List<GGNotification>();

        return dts.FirstOrDefault(e =>
            !string.IsNullOrWhiteSpace(e?.Plate) &&
            string.Equals(e.Plate.Trim(), plate.Trim(), StringComparison.OrdinalIgnoreCase)
        );
    }
    #endregion


    #region Cập nhật 
    public async Task<string> PostGGNotification(GGNotification dt)
    {
        try
        {
            var plate = (dt.Plate ?? "").Trim();
            var phone = (dt.PhoneNumber ?? "").Trim();

            if (string.IsNullOrWhiteSpace(plate) || string.IsNullOrWhiteSpace(phone))
                throw new Exception("Thiếu biển số xe hoặc số điện thoại.");

            var exist = await GetGGNotification(plate);
            if (exist != null)
                throw new Exception("Biển số xe đã được đăng ký nhận tin nhắn!");

            //Kiểm tra biển số
            var (okPlate, plateNorm, plateErr) = VnPlateExtension.NormalizeAndValidate(dt.Plate);
            if (!okPlate) throw new Exception(plateErr);
            dt.Plate = plateNorm; // lưu/so trùng bằng plateNorm

            //Kiểm tra số điện thoại
            var (ok, phoneNorm, err) = VnPhoneExtension.NormalizeAndValidate(dt.PhoneNumber);
            if (!ok) throw new Exception(err);
            dt.PhoneNumber = phoneNorm;

            // 1) Đọc cột F từ F2 để biết dòng tiếp theo
            var readRange = $"{sheetRegister}!F2:F";
            var getReq = sheetsService.Spreadsheets.Values.Get(SpreadSheetId, readRange);
            var getRes = await getReq.ExecuteAsync();

            var count = getRes?.Values?.Count ?? 0;
            var nextRow = 2 + count; // F2 là dòng bắt đầu

            // 2) Ghi đúng F-G ở dòng nextRow
            var updateRange = $"{sheetRegister}!F{nextRow}:G{nextRow}";
            var valueRange = new ValueRange
            {
                MajorDimension = "ROWS",
                Values = new List<IList<object>>
        {
            new List<object> { plateNorm, phoneNorm }
        }
            };
            var updateReq = sheetsService.Spreadsheets.Values.Update(valueRange, SpreadSheetId, updateRange);
            updateReq.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            await updateReq.ExecuteAsync();

            return "Đăng ký nhận thông báo phạt nguội thành công!";
        }
        catch (Exception ex)
        {
            // Nếu muốn hiển thị đúng lý do trùng/thiếu dữ liệu:
            throw new Exception(ex.Message);
        }
    }
    #endregion

}