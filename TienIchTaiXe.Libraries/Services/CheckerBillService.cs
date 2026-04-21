using System.Net;
using System.Net.Http.Json;
using TienIchTaiXe.Libraries.Entities;
using TienIchTaiXe.Libraries.Services.Interfaces;

namespace TienIchTaiXe.Libraries.Services;

public class CheckerBillService : ICheckerBillService
{
    private readonly HttpClient httpClient;

    /*
     * //Constructor
    public CheckerBillService(HttpClient _httpClient)
    {
        this.httpClient = _httpClient;
    }
    */
    public CheckerBillService(IHttpClientFactory httpClientFactory)
    {
        this.httpClient = httpClientFactory.CreateClient("taxinamthang");
    }

    public async Task<ShiftWorkDto> Get(string userId, string? date)
    {
        try
        {
            HttpResponseMessage response;
            if (string.IsNullOrWhiteSpace(date))
                response = await httpClient.GetAsync($"api/ShiftWork/get-crypto?cryptoAES={userId}");
            else
                response = await httpClient.GetAsync($"api/ShiftWork/get-crypto?cryptoAES={userId}&date={date}");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return new ShiftWorkDto();

                var result = await response.Content.ReadFromJsonAsync<ShiftWorkDto>();

                if (result == null)
                    return new ShiftWorkDto();


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

    //Tạm thời không dùng
    public async Task<ShiftWorkDto> Get2(string userId, string? date)
    {
        try
        {
            HttpResponseMessage response;
            if (string.IsNullOrWhiteSpace(date))
                response = await httpClient.GetAsync($"api/ShiftWork?userId={userId}");
            else
                response = await httpClient.GetAsync($"api/ShiftWork?userId={userId}&date={date}");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return new ShiftWorkDto();

                var result = await response.Content.ReadFromJsonAsync<ShiftWorkDto>();

                if (result == null)
                    return new ShiftWorkDto();


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
