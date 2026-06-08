using System.Collections.Concurrent;
using InventoryHub.API.Models;

namespace InventoryHub.API.Services;

/// <summary>
/// Service for managing inventory items with caching support for performance optimization.
/// </summary>
public interface IInventoryService
{
    Task<List<InventoryItem>> GetAllItemsAsync();
    Task<InventoryItem?> GetItemByIdAsync(int id);
    Task<InventoryItem> CreateItemAsync(CreateInventoryItemRequest request);
    Task<InventoryItem?> UpdateItemAsync(int id, UpdateInventoryItemRequest request);
    Task<bool> DeleteItemAsync(int id);
    Task<List<InventoryItem>> SearchItemsAsync(string searchTerm);
}

/// <summary>
/// Implementation of inventory service with in-memory storage and caching.
/// </summary>
public class InventoryService : IInventoryService
{
    private static readonly ConcurrentDictionary<int, InventoryItem> _items = new();
    private static int _nextId = 1;
    private static readonly ConcurrentDictionary<string, (List<InventoryItem> data, DateTime expiry)> _cache = new();
    private static readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(5);

    public InventoryService()
    {
        InitializeDefaultData();
    }

    /// <summary>
    /// Initializes the service with default inventory items for demonstration.
    /// </summary>
    private void InitializeDefaultData()
    {
        if (_items.IsEmpty)
        {
            _items.TryAdd(1, new InventoryItem 
            { 
                Id = 1, 
                Name = "Laptop Dell XPS 15", 
                Description = "High-performance laptop",
                Quantity = 25,
                Price = 1299.99m,
                Category = "Electronics",
                CreatedAt = DateTime.UtcNow
            });
            
            _items.TryAdd(2, new InventoryItem 
            { 
                Id = 2, 
                Name = "Wireless Mouse", 
                Description = "Ergonomic wireless mouse",
                Quantity = 150,
                Price = 49.99m,
                Category = "Accessories",
                CreatedAt = DateTime.UtcNow
            });
            
            _items.TryAdd(3, new InventoryItem 
            { 
                Id = 3, 
                Name = "USB-C Cable", 
                Description = "High-speed USB-C cable",
                Quantity = 500,
                Price = 9.99m,
                Category = "Cables",
                CreatedAt = DateTime.UtcNow
            });

            _nextId = 4;
        }
    }

    /// <summary>
    /// Retrieves all inventory items with cache support.
    /// </summary>
    public async Task<List<InventoryItem>> GetAllItemsAsync()
    {
        const string cacheKey = "all_items";
        
        // Check cache first
        if (_cache.TryGetValue(cacheKey, out var cached) && cached.expiry > DateTime.UtcNow)
        {
            return await Task.FromResult(cached.data);
        }

        var items = _items.Values.OrderBy(x => x.Id).ToList();
        
        // Update cache
        _cache.AddOrUpdate(cacheKey, (items, DateTime.UtcNow.Add(_cacheExpiry)), (_, _) => (items, DateTime.UtcNow.Add(_cacheExpiry)));

        return await Task.FromResult(items);
    }

    /// <summary>
    /// Retrieves an inventory item by ID with performance optimization.
    /// </summary>
    public async Task<InventoryItem?> GetItemByIdAsync(int id)
    {
        await Task.Delay(10); // Simulate async operation
        return _items.TryGetValue(id, out var item) ? item : null;
    }

    /// <summary>
    /// Creates a new inventory item and invalidates cache.
    /// </summary>
    public async Task<InventoryItem> CreateItemAsync(CreateInventoryItemRequest request)
    {
        var newItem = new InventoryItem
        {
            Id = _nextId++,
            Name = request.Name,
            Description = request.Description,
            Quantity = request.Quantity,
            Price = request.Price,
            Category = request.Category,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _items.TryAdd(newItem.Id, newItem);
        InvalidateCache();

        return await Task.FromResult(newItem);
    }

    /// <summary>
    /// Updates an existing inventory item and invalidates cache.
    /// </summary>
    public async Task<InventoryItem?> UpdateItemAsync(int id, UpdateInventoryItemRequest request)
    {
        if (!_items.TryGetValue(id, out var item))
            return null;

        item.Name = request.Name ?? item.Name;
        item.Description = request.Description ?? item.Description;
        item.Quantity = request.Quantity ?? item.Quantity;
        item.Price = request.Price ?? item.Price;
        item.Category = request.Category ?? item.Category;
        item.UpdatedAt = DateTime.UtcNow;

        InvalidateCache();
        return await Task.FromResult(item);
    }

    /// <summary>
    /// Deletes an inventory item and invalidates cache.
    /// </summary>
    public async Task<bool> DeleteItemAsync(int id)
    {
        var deleted = _items.TryRemove(id, out _);
        if (deleted)
            InvalidateCache();

        return await Task.FromResult(deleted);
    }

    /// <summary>
    /// Searches inventory items by name or category.
    /// </summary>
    public async Task<List<InventoryItem>> SearchItemsAsync(string searchTerm)
    {
        var results = _items.Values
            .Where(x => x.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       x.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       x.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .OrderBy(x => x.Id)
            .ToList();

        return await Task.FromResult(results);
    }

    /// <summary>
    /// Invalidates all cache entries to ensure data consistency.
    /// </summary>
    private void InvalidateCache()
    {
        _cache.Clear();
    }
}
