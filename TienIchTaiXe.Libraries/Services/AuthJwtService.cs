using TienIchTaiXe.Libraries.Entities;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using TienIchTaiXe.Libraries.Services.Interfaces;
using System.Net.Http.Json;
using TienIchTaiXe.Libraries.Extensions;

namespace TienIchTaiXe.Libraries.Services;

// Dùng cho Blazor WebAssembly, lưu token trong localStorage và set vào header của HttpClient để dùng cho các request sau
// Lưu ý: Cần đảm bảo đã load getStorage JS trước khi gọi các method của AuthJwtService
public class AuthJwtService : IAuthJwtService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;
    private const string Key = "_tokenJwt";

    public AuthJwtService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    // Lấy token từ localStorage, nếu có thì set vào header của HttpClient để dùng cho các request sau
    public async Task<string> LoginTokenAsync(AppLoginDTO dto)
    {
        var res = await _http.PostAsJsonAsync("api/auth/token", dto);
        res.EnsureSuccessStatusCode();

        var token = (await res.Content.ReadAsStringAsync()).Trim('"');
        await _js.SetFromLocalStorage(Key, token);
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return token;
    }

    // Lấy token từ localStorage (nếu có) và set vào header của HttpClient
    public async Task LogoutTokenAsync()
    {
        await _js.RemoveFromLocalStorage(Key);
        _http.DefaultRequestHeaders.Authorization = null;
    }
}
