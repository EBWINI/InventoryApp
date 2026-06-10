using System.Net;
using System.Net.Http.Json;
using InventoryApp.Models;

namespace InventoryApp.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private string _baseUrl = "";

    public ApiService()
    {
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = true,
            UseCookies = true,
            CookieContainer = new CookieContainer()
        };
        _http = new HttpClient(handler);
    }

    public string BaseUrl
    {
        get => _baseUrl;
        set => _baseUrl = value.TrimEnd('/');
    }

    public bool IsLoggedIn => !string.IsNullOrEmpty(_baseUrl);

    public async Task<bool> LoginAsync(string username, string password, string? shopName = null)
    {
        try
        {
            // First GET the login page to capture anti-forgery cookie + token
            var loginPage = await _http.GetAsync($"{_baseUrl}/Account/Login");
            var html = await loginPage.Content.ReadAsStringAsync();
            var token = ExtractAntiforgeryToken(html);

            var formData = new Dictionary<string, string>
            {
                { "Username", username },
                { "Password", password }
            };
            if (!string.IsNullOrEmpty(shopName))
                formData.Add("ShopName", shopName);
            if (token != null)
                formData.Add("__RequestVerificationToken", token);

            var content = new FormUrlEncodedContent(formData);
            var response = await _http.PostAsync($"{_baseUrl}/Account/Login", content);

            var finalUrl = response.RequestMessage?.RequestUri?.AbsolutePath ?? "";
            return !finalUrl.Contains("/Account/Login", StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            LastError = ex.Message;
            return false;
        }
    }

    public string? LastError { get; private set; }

    private static string? ExtractAntiforgeryToken(string html)
    {
        var marker = "name=\"__RequestVerificationToken\"";
        var idx = html.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (idx < 0) return null;
        var valueStart = html.IndexOf("value=\"", idx + marker.Length, StringComparison.Ordinal);
        if (valueStart < 0) return null;
        valueStart += 7;
        var valueEnd = html.IndexOf("\"", valueStart, StringComparison.Ordinal);
        if (valueEnd < 0) return null;
        return html.Substring(valueStart, valueEnd - valueStart);
    }

    public async Task<List<InventorySession>> GetSessionsAsync()
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<InventorySession>>($"{_baseUrl}/api/inventory/sessions");
            return result ?? new();
        }
        catch { return new(); }
    }

    public async Task<SessionDetail?> GetSessionAsync(int id)
    {
        try
        {
            return await _http.GetFromJsonAsync<SessionDetail>($"{_baseUrl}/api/inventory/sessions/{id}");
        }
        catch { return null; }
    }

    public async Task<int?> CreateSessionAsync(int? warehouseId, string? notes)
    {
        try
        {
            var parts = new List<string>();
            if (warehouseId.HasValue)
                parts.Add($"warehouseId={warehouseId.Value}");
            if (!string.IsNullOrEmpty(notes))
                parts.Add($"notes={Uri.EscapeDataString(notes)}");
            var qs = string.Join("&", parts);
            var url = qs.Length > 0 ? $"{_baseUrl}/api/inventory/sessions?{qs}" : $"{_baseUrl}/api/inventory/sessions";
            var response = await _http.PostAsync(url, null);
            var data = await response.Content.ReadFromJsonAsync<CreateSessionResponse>();
            return data?.Success == true ? data.SessionId : null;
        }
        catch { return null; }
    }

    public async Task<bool> UpdateItemCountAsync(int itemId, int quantity)
    {
        try
        {
            var url = $"{_baseUrl}/api/inventory/items/{itemId}/count?quantity={quantity}";
            var response = await _http.PostAsync(url, null);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<ScanResult?> ScanBarcodeAsync(int sessionId, string barcode)
    {
        try
        {
            var url = $"{_baseUrl}/api/inventory/scan?sessionId={sessionId}&barcode={Uri.EscapeDataString(barcode)}";
            var response = await _http.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ScanResult>();
            var errorBody = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return new ScanResult { Success = false, Message = errorBody?.Message ?? "Item not found" };
        }
        catch (Exception ex)
        {
            LastError = ex.Message;
            return null;
        }
    }

    public async Task<bool> CloseSessionAsync(int id)
    {
        try
        {
            var response = await _http.PostAsync($"{_baseUrl}/api/inventory/sessions/{id}/close", null);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<List<Warehouse>> GetWarehousesAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<Warehouse>>($"{_baseUrl}/api/inventory/warehouses") ?? new();
        }
        catch { return new(); }
    }

    private class CreateSessionResponse
    {
        public bool Success { get; set; }
        public int SessionId { get; set; }
    }
}
