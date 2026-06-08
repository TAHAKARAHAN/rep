using Newtonsoft.Json;
using InventoryHub.Client.Models;

namespace InventoryHub.Client.Services;

/// <summary>
/// Interface for HTTP-based API client communication.
/// </summary>
public interface IApiClient
{
    Task<ApiResponse<List<InventoryItem>>> GetAllItemsAsync();
    Task<ApiResponse<InventoryItem>> GetItemByIdAsync(int id);
    Task<ApiResponse<InventoryItem>> CreateItemAsync(CreateInventoryItemRequest request);
    Task<ApiResponse<InventoryItem>> UpdateItemAsync(int id, UpdateInventoryItemRequest request);
    Task<ApiResponse<string>> DeleteItemAsync(int id);
    Task<ApiResponse<List<InventoryItem>>> SearchItemsAsync(string searchTerm);
    Task<ApiResponse<InventorySummary>> GetSummaryAsync();
}

/// <summary>
/// HTTP client for communicating with the InventoryHub API.
/// Includes error handling, timeouts, and request/response logging.
/// </summary>
public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly ILogger _logger;

    public ApiClient(string baseUrl, ILogger logger)
    {
        _baseUrl = baseUrl.TrimEnd('/');
        _logger = logger;
        
        var httpClientHandler = new HttpClientHandler();
        _httpClient = new HttpClient(httpClientHandler)
        {
            Timeout = TimeSpan.FromSeconds(30)
        };
    }

    /// <summary>
    /// Retrieves all inventory items from the API.
    /// </summary>
    public async Task<ApiResponse<List<InventoryItem>>> GetAllItemsAsync()
    {
        try
        {
            var url = $"{_baseUrl}/api/inventory";
            _logger.Log($"GET: {url}");

            var response = await _httpClient.GetAsync(url);
            return await HandleResponse<List<InventoryItem>>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching all items: {ex.Message}");
            return new ApiResponse<List<InventoryItem>>
            {
                Success = false,
                Message = $"Error: {ex.Message}",
                ErrorCode = 500
            };
        }
    }

    /// <summary>
    /// Retrieves a specific inventory item by ID.
    /// </summary>
    public async Task<ApiResponse<InventoryItem>> GetItemByIdAsync(int id)
    {
        try
        {
            var url = $"{_baseUrl}/api/inventory/{id}";
            _logger.Log($"GET: {url}");

            var response = await _httpClient.GetAsync(url);
            return await HandleResponse<InventoryItem>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching item {id}: {ex.Message}");
            return new ApiResponse<InventoryItem>
            {
                Success = false,
                Message = $"Error: {ex.Message}",
                ErrorCode = 500
            };
        }
    }

    /// <summary>
    /// Creates a new inventory item.
    /// </summary>
    public async Task<ApiResponse<InventoryItem>> CreateItemAsync(CreateInventoryItemRequest request)
    {
        try
        {
            var url = $"{_baseUrl}/api/inventory";
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            _logger.Log($"POST: {url}");
            _logger.Log($"Body: {json}");

            var response = await _httpClient.PostAsync(url, content);
            return await HandleResponse<InventoryItem>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating item: {ex.Message}");
            return new ApiResponse<InventoryItem>
            {
                Success = false,
                Message = $"Error: {ex.Message}",
                ErrorCode = 500
            };
        }
    }

    /// <summary>
    /// Updates an existing inventory item.
    /// </summary>
    public async Task<ApiResponse<InventoryItem>> UpdateItemAsync(int id, UpdateInventoryItemRequest request)
    {
        try
        {
            var url = $"{_baseUrl}/api/inventory/{id}";
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            _logger.Log($"PUT: {url}");
            _logger.Log($"Body: {json}");

            var response = await _httpClient.PutAsync(url, content);
            return await HandleResponse<InventoryItem>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating item {id}: {ex.Message}");
            return new ApiResponse<InventoryItem>
            {
                Success = false,
                Message = $"Error: {ex.Message}",
                ErrorCode = 500
            };
        }
    }

    /// <summary>
    /// Deletes an inventory item.
    /// </summary>
    public async Task<ApiResponse<string>> DeleteItemAsync(int id)
    {
        try
        {
            var url = $"{_baseUrl}/api/inventory/{id}";
            _logger.Log($"DELETE: {url}");

            var response = await _httpClient.DeleteAsync(url);
            return await HandleResponse<string>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting item {id}: {ex.Message}");
            return new ApiResponse<string>
            {
                Success = false,
                Message = $"Error: {ex.Message}",
                ErrorCode = 500
            };
        }
    }

    /// <summary>
    /// Searches inventory items by keyword.
    /// </summary>
    public async Task<ApiResponse<List<InventoryItem>>> SearchItemsAsync(string searchTerm)
    {
        try
        {
            var url = $"{_baseUrl}/api/inventory/search/{Uri.EscapeDataString(searchTerm)}";
            _logger.Log($"GET: {url}");

            var response = await _httpClient.GetAsync(url);
            return await HandleResponse<List<InventoryItem>>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching items: {ex.Message}");
            return new ApiResponse<List<InventoryItem>>
            {
                Success = false,
                Message = $"Error: {ex.Message}",
                ErrorCode = 500
            };
        }
    }

    /// <summary>
    /// Retrieves inventory summary statistics.
    /// </summary>
    public async Task<ApiResponse<InventorySummary>> GetSummaryAsync()
    {
        try
        {
            var url = $"{_baseUrl}/api/inventory/stats/summary";
            _logger.Log($"GET: {url}");

            var response = await _httpClient.GetAsync(url);
            return await HandleResponse<InventorySummary>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching summary: {ex.Message}");
            return new ApiResponse<InventorySummary>
            {
                Success = false,
                Message = $"Error: {ex.Message}",
                ErrorCode = 500
            };
        }
    }

    /// <summary>
    /// Handles HTTP response and deserializes JSON.
    /// </summary>
    private async Task<ApiResponse<T>> HandleResponse<T>(HttpResponseMessage response) where T : class
    {
        var content = await response.Content.ReadAsStringAsync();
        _logger.Log($"Status: {response.StatusCode}");
        _logger.Log($"Response: {content}");

        try
        {
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(content);
            return apiResponse ?? new ApiResponse<T> 
            { 
                Success = false, 
                Message = "Failed to deserialize response",
                ErrorCode = 500
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deserializing response: {ex.Message}");
            return new ApiResponse<T> 
            { 
                Success = false, 
                Message = ex.Message,
                ErrorCode = 500
            };
        }
    }
}

/// <summary>
/// Simple logger interface for client-side logging.
/// </summary>
public interface ILogger
{
    void Log(string message);
    void LogError(string message);
}

/// <summary>
/// Console-based logger implementation.
/// </summary>
public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
    }

    public void LogError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        Console.ResetColor();
    }
}
