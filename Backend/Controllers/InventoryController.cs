using InventoryHub.API.Models;
using InventoryHub.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryHub.API.Controllers;

/// <summary>
/// API Controller for managing inventory items.
/// Provides CRUD operations and search functionality.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    private readonly ILogger<InventoryController> _logger;

    public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
    {
        _inventoryService = inventoryService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all inventory items.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<InventoryItem>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllItems()
    {
        try
        {
            _logger.LogInformation("Fetching all inventory items");
            var items = await _inventoryService.GetAllItemsAsync();
            return Ok(ApiResponse<List<InventoryItem>>.SuccessResponse(items, $"Retrieved {items.Count} items"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching inventory items");
            return StatusCode(500, ApiResponse<List<InventoryItem>>.ErrorResponse("Internal server error", 500));
        }
    }

    /// <summary>
    /// Gets a specific inventory item by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<InventoryItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<InventoryItem>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetItemById(int id)
    {
        try
        {
            _logger.LogInformation("Fetching inventory item with ID: {ItemId}", id);
            var item = await _inventoryService.GetItemByIdAsync(id);
            
            if (item == null)
                return NotFound(ApiResponse<InventoryItem>.ErrorResponse("Item not found", 404));

            return Ok(ApiResponse<InventoryItem>.SuccessResponse(item));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching item {ItemId}", id);
            return StatusCode(500, ApiResponse<InventoryItem>.ErrorResponse("Internal server error", 500));
        }
    }

    /// <summary>
    /// Creates a new inventory item.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<InventoryItem>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<InventoryItem>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateItem([FromBody] CreateInventoryItemRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<InventoryItem>.ErrorResponse("Invalid request data", 400));

            _logger.LogInformation("Creating new inventory item: {ItemName}", request.Name);
            var item = await _inventoryService.CreateItemAsync(request);
            
            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, 
                ApiResponse<InventoryItem>.SuccessResponse(item, "Item created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating inventory item");
            return StatusCode(500, ApiResponse<InventoryItem>.ErrorResponse("Internal server error", 500));
        }
    }

    /// <summary>
    /// Updates an existing inventory item.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<InventoryItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<InventoryItem>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItem(int id, [FromBody] UpdateInventoryItemRequest request)
    {
        try
        {
            _logger.LogInformation("Updating inventory item with ID: {ItemId}", id);
            var item = await _inventoryService.UpdateItemAsync(id, request);
            
            if (item == null)
                return NotFound(ApiResponse<InventoryItem>.ErrorResponse("Item not found", 404));

            return Ok(ApiResponse<InventoryItem>.SuccessResponse(item, "Item updated successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating item {ItemId}", id);
            return StatusCode(500, ApiResponse<InventoryItem>.ErrorResponse("Internal server error", 500));
        }
    }

    /// <summary>
    /// Deletes an inventory item.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteItem(int id)
    {
        try
        {
            _logger.LogInformation("Deleting inventory item with ID: {ItemId}", id);
            var deleted = await _inventoryService.DeleteItemAsync(id);
            
            if (!deleted)
                return NotFound(ApiResponse<string>.ErrorResponse("Item not found", 404));

            return Ok(ApiResponse<string>.SuccessResponse($"Item {id} deleted", "Item deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting item {ItemId}", id);
            return StatusCode(500, ApiResponse<string>.ErrorResponse("Internal server error", 500));
        }
    }

    /// <summary>
    /// Searches inventory items by keyword.
    /// </summary>
    [HttpGet("search/{searchTerm}")]
    [ProducesResponseType(typeof(ApiResponse<List<InventoryItem>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchItems(string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest(ApiResponse<List<InventoryItem>>.ErrorResponse("Search term cannot be empty", 400));

            _logger.LogInformation("Searching for items with term: {SearchTerm}", searchTerm);
            var results = await _inventoryService.SearchItemsAsync(searchTerm);
            
            return Ok(ApiResponse<List<InventoryItem>>.SuccessResponse(results, $"Found {results.Count} items"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching items");
            return StatusCode(500, ApiResponse<List<InventoryItem>>.ErrorResponse("Internal server error", 500));
        }
    }

    /// <summary>
    /// Gets inventory summary statistics.
    /// </summary>
    [HttpGet("stats/summary")]
    [ProducesResponseType(typeof(ApiResponse<InventorySummary>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary()
    {
        try
        {
            _logger.LogInformation("Fetching inventory summary");
            var items = await _inventoryService.GetAllItemsAsync();
            
            var summary = new InventorySummary
            {
                TotalItems = items.Count,
                TotalQuantity = items.Sum(x => x.Quantity),
                TotalValue = items.Sum(x => x.Price * x.Quantity),
                Categories = items.Select(x => x.Category).Distinct().ToList()
            };

            return Ok(ApiResponse<InventorySummary>.SuccessResponse(summary, "Summary retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching summary");
            return StatusCode(500, ApiResponse<InventorySummary>.ErrorResponse("Internal server error", 500));
        }
    }
}
