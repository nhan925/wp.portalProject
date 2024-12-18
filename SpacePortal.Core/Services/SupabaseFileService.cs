using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

public class SupabaseFileService
{
    private readonly string _supabaseUrl;
    private readonly string _supabaseKey;
    private readonly string _bucketName;
    private readonly HttpClient _httpClient;

    public SupabaseFileService(string supabaseUrl, string supabaseKey, string bucketName)
    {
        _supabaseUrl = supabaseUrl ?? throw new ArgumentNullException(nameof(supabaseUrl));
        _supabaseKey = supabaseKey ?? throw new ArgumentNullException(nameof(supabaseKey));
        _bucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _supabaseKey);
        _httpClient.DefaultRequestHeaders.Add("apikey", _supabaseKey);
    }

    public async Task<string> UploadFileAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found", filePath);

        // Generate a unique file name
        var uniqueFileName = Guid.NewGuid() + Path.GetExtension(filePath);

        // Read the file content
        var fileBytes = await File.ReadAllBytesAsync(filePath);

        // Determine the MIME type based on the file extension
        var mimeType = GetMimeType(filePath);

        // Prepare the API endpoint
        var endpoint = $"{_supabaseUrl}/storage/v1/object/{_bucketName}/{uniqueFileName}";

        // Create the content for the request
        var content = new ByteArrayContent(fileBytes);
        content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

        // Send the request
        var response = await _httpClient.PostAsync(endpoint, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"File upload failed: {response.StatusCode} - {error}");
        }

        // Return the public URL
        return $"{_supabaseUrl}/storage/v1/object/public/{_bucketName}/{uniqueFileName}";
    }

    private string GetMimeType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLower();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".pdf" => "application/pdf",
            ".txt" => "text/plain",
            _ => "application/octet-stream", // Fallback type
        };
    }
}
