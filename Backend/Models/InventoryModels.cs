namespace InventoryHub.API.Models;

/// <summary>
/// Represents an inventory item in the system.
/// </summary>
public class InventoryItem
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public required string Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Request model for creating a new inventory item.
/// </summary>
public class CreateInventoryItemRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required int Quantity { get; set; }
    public required decimal Price { get; set; }
    public required string Category { get; set; }
}

/// <summary>
/// Request model for updating an existing inventory item.
/// </summary>
public class UpdateInventoryItemRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Quantity { get; set; }
    public decimal? Price { get; set; }
    public string? Category { get; set; }
}

/// <summary>
/// API response wrapper for consistent JSON responses.
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public int? ErrorCode { get; set; }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Success")
        => new() { Success = true, Data = data, Message = message };

    public static ApiResponse<T> ErrorResponse(string message, int errorCode = 400)
        => new() { Success = false, Message = message, ErrorCode = errorCode };
}

/// <summary>
/// Summary statistics for inventory.
/// </summary>
public class InventorySummary
{
    public int TotalItems { get; set; }
    public int TotalQuantity { get; set; }
    public decimal TotalValue { get; set; }
    public List<string> Categories { get; set; } = new();
}
