using Newtonsoft.Json;

namespace InventoryHub.Client.Models;

/// <summary>
/// Represents an inventory item in the client application.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class InventoryItem
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("category")]
    public string? Category { get; set; }

    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    public override string ToString() => 
        $"[{Id}] {Name} - Qty: {Quantity}, Price: ${Price:F2}, Category: {Category}";
}

/// <summary>
/// Request model for creating an inventory item.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class CreateInventoryItemRequest
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("category")]
    public string? Category { get; set; }
}

/// <summary>
/// Request model for updating an inventory item.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class UpdateInventoryItemRequest
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("quantity")]
    public int? Quantity { get; set; }

    [JsonProperty("price")]
    public decimal? Price { get; set; }

    [JsonProperty("category")]
    public string? Category { get; set; }
}

/// <summary>
/// Generic API response wrapper.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class ApiResponse<T>
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("message")]
    public string? Message { get; set; }

    [JsonProperty("data")]
    public T? Data { get; set; }

    [JsonProperty("errorCode")]
    public int? ErrorCode { get; set; }
}

/// <summary>
/// Inventory summary statistics.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class InventorySummary
{
    [JsonProperty("totalItems")]
    public int TotalItems { get; set; }

    [JsonProperty("totalQuantity")]
    public int TotalQuantity { get; set; }

    [JsonProperty("totalValue")]
    public decimal TotalValue { get; set; }

    [JsonProperty("categories")]
    public List<string> Categories { get; set; } = new();

    public override string ToString() =>
        $"Total Items: {TotalItems}, Total Qty: {TotalQuantity}, Total Value: ${TotalValue:F2}";
}
