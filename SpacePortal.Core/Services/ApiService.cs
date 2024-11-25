using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace SpacePortal.Core.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUri;
    private string _jwtToken;

    public ApiService(string baseUri)
    {
        _httpClient = new HttpClient();
        _baseUri = baseUri;
    }

    // Login to retrieve JWT token
    public bool Login(string username, string password)
    {
        var loginData = new { username, password };
        var response = Post<string>("/rpc/login", loginData);

        if (response != null && !string.IsNullOrEmpty(response))
        {
            _jwtToken = response; // Store the JWT token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
            return true; // Login successful
        }

        return false; // Login failed
    }

    // Generic synchronous GET method
    public T Get<T>(string endpoint)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUri}{endpoint}");
        var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();

        if (response.IsSuccessStatusCode)
        {
            var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        else
        {
            // Handle error (e.g., throw an exception or return default value)
            return default;
        }
    }

    // Generic synchronous POST method
    public T Post<T>(string endpoint, object data)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUri}{endpoint}");
        request.Content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
        var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();

        if (response.IsSuccessStatusCode)
        {
            var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        else
        {
            // Handle error
            return default;
        }
    }

    // Generic synchronous PUT method
    public T Put<T>(string endpoint, object data)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"{_baseUri}{endpoint}");
        request.Content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
        var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();

        if (response.IsSuccessStatusCode)
        {
            var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        else
        {
            // Handle error
            return default;
        }
    }

    // Generic synchronous DELETE method
    public T Delete<T>(string endpoint)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUri}{endpoint}");
        var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();

        if (response.IsSuccessStatusCode)
        {
            var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        else
        {
            // Handle error
            return default;
        }
    }

    public bool LoginWithOutlook(string acessToken)
    {
        var loginData = new { v_token= acessToken };
        var response = Post<string>("/rpc/login_with_outlook", loginData);

        if (response != "")
        {
            _jwtToken = response; // Store the JWT token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
            return true; // Login successful
        }
        return false; // Login failed
    }
}
