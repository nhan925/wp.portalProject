using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Core.Services;
public class ImgurService
{
    private readonly HttpClient _httpClient;
    private readonly string _clientId;

    public ImgurService(string clientId)
    {
        _clientId = clientId;
        _httpClient = new HttpClient();
    }

    public async Task<string> UploadImageAsync(string filePath)
    {
        // Read image file as bytes
        var imageBytes = File.ReadAllBytes(filePath);

        // Get MIME type based on file extension
        var mimeType = GetMimeType(filePath);

        using var content = new MultipartFormDataContent();
        using var imageContent = new ByteArrayContent(imageBytes);

        // Set the appropriate MIME type
        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(mimeType);
        content.Add(imageContent, "image");

        // Add Imgur authorization header with Client ID
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", _clientId);

        // Send POST request to Imgur API
        var response = await _httpClient.PostAsync("https://api.imgur.com/3/image", content);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);
            return result.data.link; // Link to the uploaded image
        }
        return null; // Upload failed
    }

    private static string GetMimeType(string filePath)
    {
        var extension = Path.GetExtension(filePath)?.ToLower();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".tiff" => "image/tiff",
            _ => throw new NotSupportedException("Unsupported image format")
        };
    }
}
