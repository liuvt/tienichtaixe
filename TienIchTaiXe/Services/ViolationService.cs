using System.Net;
using TienIchTaiXe.Libraries.Models;
using TienIchTaiXe.Services.Interfaces;

namespace TienIchTaiXe.Services;

public class ViolationService : IViolationService
{
    private readonly HttpClient httpClient;

    //Constructor
    public ViolationService(IHttpClientFactory httpClientFactory)
    {
        this.httpClient = httpClientFactory.CreateClient("n8n");
    }

    public async Task<List<Violation>> Gets(string licensePlate)
    {
        try
        {
            var response = await httpClient.GetAsync($"webhook/api-tcpn-tvtteam?bks={licensePlate}");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return new List<Violation>();

                var result = await response.Content.ReadFromJsonAsync<List<Violation>>();

                if (result == null)
                    return new List<Violation>();

                return result;
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"API Error: {error}");
        }
        catch (Exception ex)
        {
            // Có thể log ex ở đây
            throw new HttpRequestException($"Lỗi không load được data tư server --{ex}");

        }
    }
}
