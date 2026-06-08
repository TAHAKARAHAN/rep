using InventoryHub.Client.Models;
using InventoryHub.Client.Services;

class Program
{
    private static IApiClient? _apiClient;
    private static ILogger? _logger;
    private const string ApiBaseUrl = "http://localhost:5000";

    static async Task Main(string[] args)
    {
        _logger = new ConsoleLogger();
        _apiClient = new ApiClient(ApiBaseUrl, _logger);

        Console.Clear();
        Console.WriteLine("╔═══════════════════════════════════════════════════════╗");
        Console.WriteLine("║        Welcome to InventoryHub Management System       ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════╝\n");

        bool running = true;
        while (running)
        {
            DisplayMenu();
            Console.Write("\nSelect an option (1-8): ");
            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await ViewAllItems();
                        break;
                    case "2":
                        await ViewItemById();
                        break;
                    case "3":
                        await CreateNewItem();
                        break;
                    case "4":
                        await UpdateItem();
                        break;
                    case "5":
                        await DeleteItem();
                        break;
                    case "6":
                        await SearchItems();
                        break;
                    case "7":
                        await ViewSummary();
                        break;
                    case "8":
                        running = false;
                        Console.WriteLine("\nGoodbye! Thank you for using InventoryHub.");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger!.LogError($"Unexpected error: {ex.Message}");
            }

            if (running && choice != "8")
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine("┌─────────────────────────────────────────────────────┐");
        Console.WriteLine("│              MAIN MENU - SELECT OPERATION            │");
        Console.WriteLine("├─────────────────────────────────────────────────────┤");
        Console.WriteLine("│ 1. View All Inventory Items                         │");
        Console.WriteLine("│ 2. View Item by ID                                  │");
        Console.WriteLine("│ 3. Create New Item                                  │");
        Console.WriteLine("│ 4. Update Existing Item                             │");
        Console.WriteLine("│ 5. Delete Item                                      │");
        Console.WriteLine("│ 6. Search Items                                     │");
        Console.WriteLine("│ 7. View Inventory Summary                           │");
        Console.WriteLine("│ 8. Exit                                             │");
        Console.WriteLine("└─────────────────────────────────────────────────────┘");
    }

    static async Task ViewAllItems()
    {
        Console.WriteLine("\n📦 Fetching all inventory items...\n");
        var result = await _apiClient!.GetAllItemsAsync();

        if (!result.Success || result.Data == null || result.Data.Count == 0)
        {
            Console.WriteLine($"❌ {result.Message}");
            return;
        }

        Console.WriteLine($"✓ {result.Message}\n");
        Console.WriteLine("╔════╦════════════════════════╦════════╦═════════╦═════════════════╦═════════════╗");
        Console.WriteLine("║ ID ║ Name                   ║ Qty    ║ Price   ║ Category        ║ Updated     ║");
        Console.WriteLine("╠════╬════════════════════════╬════════╬═════════╬═════════════════╬═════════════╣");

        foreach (var item in result.Data)
        {
            Console.WriteLine($"║ {item.Id,-2} ║ {item.Name,-22} ║ {item.Quantity,-6} ║ ${item.Price,-6:F2} ║ {item.Category,-15} ║ {item.UpdatedAt?.ToString("yyyy-MM-dd") ?? "N/A",-11} ║");
        }

        Console.WriteLine("╚════╩════════════════════════╩════════╩═════════╩═════════════════╩═════════════╝");
    }

    static async Task ViewItemById()
    {
        Console.Write("\nEnter Item ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var result = await _apiClient!.GetItemByIdAsync(id);

            if (!result.Success || result.Data == null)
            {
                Console.WriteLine($"❌ {result.Message}");
                return;
            }

            var item = result.Data;
            Console.WriteLine($"\n✓ Item Found:\n");
            Console.WriteLine($"  ID:          {item.Id}");
            Console.WriteLine($"  Name:        {item.Name}");
            Console.WriteLine($"  Description: {item.Description}");
            Console.WriteLine($"  Quantity:    {item.Quantity}");
            Console.WriteLine($"  Price:       ${item.Price:F2}");
            Console.WriteLine($"  Category:    {item.Category}");
            Console.WriteLine($"  Created:     {item.CreatedAt:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"  Updated:     {item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Never"}");
        }
        else
        {
            Console.WriteLine("❌ Invalid ID format.");
        }
    }

    static async Task CreateNewItem()
    {
        Console.WriteLine("\n📝 Create New Inventory Item\n");

        Console.Write("Name: ");
        var name = Console.ReadLine();

        Console.Write("Description: ");
        var description = Console.ReadLine();

        Console.Write("Quantity: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity))
        {
            Console.WriteLine("❌ Invalid quantity.");
            return;
        }

        Console.Write("Price: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            Console.WriteLine("❌ Invalid price.");
            return;
        }

        Console.Write("Category: ");
        var category = Console.ReadLine();

        var request = new CreateInventoryItemRequest
        {
            Name = name,
            Description = description,
            Quantity = quantity,
            Price = price,
            Category = category
        };

        var result = await _apiClient!.CreateItemAsync(request);

        if (!result.Success || result.Data == null)
        {
            Console.WriteLine($"❌ {result.Message}");
            return;
        }

        Console.WriteLine($"\n✓ {result.Message}");
        Console.WriteLine($"  New Item ID: {result.Data.Id}");
    }

    static async Task UpdateItem()
    {
        Console.Write("\nEnter Item ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("❌ Invalid ID format.");
            return;
        }

        var getResult = await _apiClient!.GetItemByIdAsync(id);
        if (!getResult.Success || getResult.Data == null)
        {
            Console.WriteLine($"❌ {getResult.Message}");
            return;
        }

        var currentItem = getResult.Data;
        Console.WriteLine($"\nCurrent Item: {currentItem.Name}\n");

        Console.Write($"New Name (leave blank to keep '{currentItem.Name}'): ");
        var name = Console.ReadLine();

        Console.Write($"New Quantity (leave blank to keep {currentItem.Quantity}): ");
        var qtyStr = Console.ReadLine();
        int? quantity = string.IsNullOrEmpty(qtyStr) ? null : int.Parse(qtyStr);

        var request = new UpdateInventoryItemRequest
        {
            Name = string.IsNullOrEmpty(name) ? null : name,
            Quantity = quantity
        };

        var result = await _apiClient!.UpdateItemAsync(id, request);

        if (!result.Success)
        {
            Console.WriteLine($"❌ {result.Message}");
            return;
        }

        Console.WriteLine($"\n✓ {result.Message}");
    }

    static async Task DeleteItem()
    {
        Console.Write("\nEnter Item ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("❌ Invalid ID format.");
            return;
        }

        Console.Write("Are you sure? (y/n): ");
        if (Console.ReadLine()?.ToLower() != "y")
        {
            Console.WriteLine("Cancelled.");
            return;
        }

        var result = await _apiClient!.DeleteItemAsync(id);

        if (!result.Success)
        {
            Console.WriteLine($"❌ {result.Message}");
            return;
        }

        Console.WriteLine($"\n✓ {result.Message}");
    }

    static async Task SearchItems()
    {
        Console.Write("\nEnter search term: ");
        var searchTerm = Console.ReadLine();

        if (string.IsNullOrEmpty(searchTerm))
        {
            Console.WriteLine("❌ Search term cannot be empty.");
            return;
        }

        Console.WriteLine("\n🔍 Searching...\n");
        var result = await _apiClient!.SearchItemsAsync(searchTerm);

        if (!result.Success || result.Data == null || result.Data.Count == 0)
        {
            Console.WriteLine($"❌ {result.Message}");
            return;
        }

        Console.WriteLine($"✓ {result.Message}\n");
        foreach (var item in result.Data)
        {
            Console.WriteLine($"  • {item}");
        }
    }

    static async Task ViewSummary()
    {
        Console.WriteLine("\n📊 Fetching inventory summary...\n");
        var result = await _apiClient!.GetSummaryAsync();

        if (!result.Success || result.Data == null)
        {
            Console.WriteLine($"❌ {result.Message}");
            return;
        }

        var summary = result.Data;
        Console.WriteLine($"✓ {result.Message}\n");
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine($"║ Total Items:      {summary.TotalItems,-30} ║");
        Console.WriteLine($"║ Total Quantity:   {summary.TotalQuantity,-30} ║");
        Console.WriteLine($"║ Total Value:      ${summary.TotalValue,-29:F2} ║");
        Console.WriteLine($"║ Categories:       {string.Join(", ", summary.Categories),-30} ║");
        Console.WriteLine("╚════════════════════════════════════════════╝");
    }
}
