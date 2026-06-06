using TienIchTaiXe.Libraries.Entities;
using TienIchTaiXe.Services.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace TienIchTaiXe.Libraries.Services;

public class CheckerSalaryService : ICheckerSalaryService
{
    #region Constructor 

    private readonly HttpClient httpClient;
    public CheckerSalaryService(IHttpClientFactory httpClientFactory)
    {
        this.httpClient = httpClientFactory.CreateClient("taxinamthang");
    }
    #endregion

    public async Task<CheckerSalaryDto> Get(string userId, string? date = null)
    {
        try
        {
            HttpResponseMessage response;
            if (string.IsNullOrWhiteSpace(date))
                response = await httpClient.GetAsync($"api/Salary/get-salary-crypto?cryptoAES={userId}");
            else
                response = await httpClient.GetAsync($"api/Salary/get-salary-crypto?cryptoAES={userId}&date={date}");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return new CheckerSalaryDto();

                var result = await response.Content.ReadFromJsonAsync<CheckerSalaryDto>();

                if (result == null)
                    return new CheckerSalaryDto();


                return result;
            }

            var error = await response.Content.ReadAsStringAsync();

            throw new HttpRequestException($"API Error: {response.StatusCode} - {error}");
        }
        catch (Exception ex)
        {
            // Có thể log ex ở đây
            throw new HttpRequestException($"Lỗi không load được data tư server --{ex}");
        }
    }
}
