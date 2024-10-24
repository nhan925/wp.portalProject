using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SpacePortal.Core.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUri;
    private string _jwtToken;

    // Private constructor
    public ApiService(string baseUri)
    {
        _httpClient = new HttpClient();
        _baseUri = baseUri;
    }

    // Login to retrive JWT token
    // TODO: Modify when implement login feature
    public async Task<bool> LoginAsync(string username, string password)
    {
        var loginData = new { username, password };
        var response = await PostAsync<string>(@"/rpc/login", loginData);

        if (response != null && !string.IsNullOrEmpty(response))
        {
            _jwtToken = response; // Store the JWT token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
            return true; // Login successful
        }

        return false; // Login failed
    }


    // Generic GET method
    public async Task<T> GetAsync<T>(string endpoint)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUri}{endpoint}");
        var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        else
        {
            // Handle error (e.g., throw an exception or return default value)
            return default;
        }
    }

    // Generic POST method
    public async Task<T> PostAsync<T>(string endpoint, object data)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUri}{endpoint}");
        request.Content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        else
        {
            // Handle error
            return default;
        }
    }

    // Generic PUT method
    public async Task<T> PutAsync<T>(string endpoint, object data)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"{_baseUri}{endpoint}");
        request.Content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        else
        {
            // Handle error
            return default;
        }
    }

    // Generic DELETE method
    public async Task<T> DeleteAsync<T>(string endpoint)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUri}{endpoint}");
        var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        else
        {
            // Handle error
            return default;
        }
    }
}
