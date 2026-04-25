using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using TienIchTaiXe.Libraries.Extensions;
using TienIchTaiXe.Libraries.Entities;
using TienIchTaiXe.Libraries.Models.GGSheets;
using TienIchTaiXe.Services.Interfaces;

namespace TienIchTaiXe.Services;

public class AuthService : IAuthService
{
    #region Constructor 
    //For Connection to Spread
    private SheetsService sheetsService;
    private readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
    private readonly string CredentialGGSheetService = "ggsheetaccount.json";
    private readonly string AppName = "TASK YOUTUBE MMO TVT";
    private readonly string SpreadSheetId = "1eTktK8xSN8J1Gexk9qcTAv0QE_JENUkiNRblWVhkL0U";

    // For Sheet
    private readonly string sheetUSERS = "dbUsers";

    public AuthService()
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

    #region TAI_KHOAN
    // Lấy toàn bộ danh sách
    private async Task<List<GGSUser>> GetsGGSUser()
    {
        var dts = new List<GGSUser>();
        var range = $"{sheetUSERS}!A2:F";
        var values = await sheetsService.ltvGetSheetValuesAsync(SpreadSheetId, range);
        if (values == null || values.Count == 0)
        {
            throw new Exception("Không có dữ liệu sheet.");
        }

        foreach (var item in values)
        {

            dts.Add(new GGSUser
            {
                Id = item.ltvGetValueString(0),
                UserName = item.ltvGetValueString(1),
                FullName = item.ltvGetValueString(2),
                Email = item.ltvGetValueString(3),
                Password = item.ltvGetValueString(4),
                HashPassword = item.ltvGetValueString(5),
            });
        }

        return dts;
    }

    // Lọc lại danh sách theo mã msnv 
    public async Task<GGSUser> GetGGSUser(string _id)
    {
        var dts = await GetsGGSUser() ?? new List<GGSUser>();
        return dts.FirstOrDefault(e => e.Id.Equals(_id, StringComparison.OrdinalIgnoreCase)) ?? new GGSUser();
    }
    #endregion


    #region Cập nhật lại token trên sheet sau khi đăng nhập thành công
    public async Task<bool> UpdateTokenUser(string _token)
    {
        try
        {
            var dts = new List<GGSUser>();
            var range = $"{sheetUSERS}!F2";

            // Tạo ValueRange (1 hàng, 1 cột)
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object> { _token }  // tương ứng ô F2
                }
            };

            if (!string.IsNullOrEmpty(_token))
            {
                await sheetsService.ltvUpdateSheetValuesAsync(valueRange, SpreadSheetId, range);
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }
    #endregion

    #region LOGIN
    public async Task<GGSUser> GGSLogin(GGSUserLoginDto model)
    {
        var listUsers = await GetsGGSUser() ?? new List<GGSUser>();

        var user = listUsers.FirstOrDefault(u =>
            u.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase));

        if (user == null || user.Password != model.Password)
        {
            throw new Exception("Tên đăng nhập hoặc mật khẩu không đúng.");
        }

        return user;
    }
    #endregion
}
