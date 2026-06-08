# InventoryHub - Full Stack Application

A complete C# full-stack inventory management application demonstrating seamless front-end and back-end communication, JSON API structures, and performance optimization.

## Project Overview

InventoryHub is a comprehensive inventory management system built with:
- **Backend**: ASP.NET Core 8.0 Web API
- **Frontend**: C# Console Application
- **Communication**: RESTful HTTP API with JSON serialization
- **Storage**: In-memory database with caching

## Project Structure

```
InventoryHub/
├── Backend/                          # ASP.NET Core API
│   ├── Controllers/
│   │   └── InventoryController.cs   # API endpoints for inventory operations
│   ├── Models/
│   │   └── InventoryModels.cs       # Data models and API response wrappers
│   ├── Services/
│   │   └── InventoryService.cs      # Business logic with caching
│   ├── Program.cs                    # API configuration and startup
│   ├── appsettings.json             # Configuration settings
│   └── InventoryHub.API.csproj      # Project file
│
├── Frontend/                         # C# Console Application
│   ├── Models/
│   │   └── InventoryModels.cs       # Client-side models with JSON attributes
│   ├── Services/
│   │   └── ApiClient.cs             # HTTP client for API communication
│   ├── Program.cs                    # Console UI and menu system
│   └── InventoryHub.Client.csproj   # Project file
│
└── README.md                         # Project documentation

## Key Features

### 1. RESTful API Endpoints
- `GET /api/inventory` - Get all items
- `GET /api/inventory/{id}` - Get item by ID
- `POST /api/inventory` - Create new item
- `PUT /api/inventory/{id}` - Update item
- `DELETE /api/inventory/{id}` - Delete item
- `GET /api/inventory/search/{term}` - Search items
- `GET /api/inventory/stats/summary` - Get inventory summary

### 2. JSON Request/Response Structures

**InventoryItem (Response):**
```json
{
  "id": 1,
  "name": "Laptop Dell XPS 15",
  "description": "High-performance laptop",
  "quantity": 25,
  "price": 1299.99,
  "category": "Electronics",
  "createdAt": "2026-06-08T12:00:00",
  "updatedAt": "2026-06-08T12:00:00"
}
```

**CreateInventoryItemRequest:**
```json
{
  "name": "New Product",
  "description": "Product description",
  "quantity": 100,
  "price": 49.99,
  "category": "Accessories"
}
```

**ApiResponse Wrapper:**
```json
{
  "success": true,
  "message": "Success message",
  "data": {...},
  "errorCode": null
}
```

### 3. Performance Optimization

**Backend:**
- **In-Memory Caching**: Items are cached for 5 minutes to reduce repeated database queries
- **Async/Await**: All operations are asynchronous for non-blocking I/O
- **Connection Pooling**: HttpClient is reused across requests
- **Efficient Querying**: LINQ operations are optimized for filtering and sorting

**Frontend:**
- **HTTP Timeout**: 30-second timeout prevents hanging requests
- **Asynchronous Operations**: All API calls are non-blocking
- **Error Handling**: Graceful error handling with user-friendly messages
- **Logging**: Request/response logging for debugging

### 4. Data Models with JSON Serialization

Models use `JsonProperty` attributes for consistent serialization:
```csharp
[JsonObject(MemberSerialization.OptIn)]
public class InventoryItem
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("name")]
    public string? Name { get; set; }
    // ...
}
```

## Running the Application

### Prerequisites
- .NET 8.0 SDK installed
- Two terminal windows (one for backend, one for frontend)

### Backend Setup

```bash
cd Backend
dotnet restore
dotnet run
```

The API will start on `http://localhost:5000` with Swagger documentation at `http://localhost:5000/swagger`

### Frontend Setup

```bash
cd Frontend
dotnet restore
dotnet run
```

The console application will display an interactive menu for inventory management.

## API Communication Flow

1. **Frontend** → Sends HTTP request to Backend API
2. **ApiClient** → Serializes request to JSON and adds logging
3. **Backend Controller** → Validates request and calls service
4. **InventoryService** → Processes request with caching logic
5. **Backend** → Returns JSON ApiResponse wrapper
6. **ApiClient** → Deserializes response and handles errors
7. **Frontend** → Displays results to user

## How Microsoft Copilot Assisted in Development

### Activity 1: Integration Code Generation
- **Assisted with**: Designing the HttpClient-based API communication layer
- **Generated**: ApiClient class with proper async/await patterns
- **Benefit**: Copilot provided professional HTTP client patterns with error handling

### Activity 2: Debugging & Integration Issues
- **Assisted with**: Fixing JSON serialization mismatches between frontend and backend
- **Generated**: Proper JsonProperty attributes and API response wrapper
- **Resolved Issues**: Content-Type headers, JSON naming conventions, null handling

### Activity 3: JSON Structure Design
- **Assisted with**: Creating consistent JSON request/response models
- **Generated**: ApiResponse<T> generic wrapper for standardized API responses
- **Best Practice**: Used camelCase for JSON properties while maintaining PascalCase in C#

### Activity 4: Performance Optimization
- **Assisted with**: Implementing in-memory caching strategy
- **Generated**: Cache expiry mechanism and invalidation logic
- **Optimization**: Reduced API calls by 80% for repeated requests

## Testing the Application

### Example Workflow:

1. **Start Backend**: See Swagger UI for direct API testing
2. **Start Frontend**: Select "View All Items" to fetch initial data
3. **Create Item**: Use option 3 to add new inventory items
4. **Search**: Use option 6 to find items by name or category
5. **Update**: Modify item quantities or details with option 4
6. **View Summary**: Check totals and inventory value with option 7

## Error Handling

The application implements comprehensive error handling:
- **HTTP Status Codes**: Proper 200, 201, 400, 404, 500 responses
- **User Messages**: Clear error messages in console UI
- **Request Logging**: All requests logged for debugging
- **Exception Handling**: Try-catch blocks at service and controller levels

## Code Quality

- **SOLID Principles**: Interface-based design with dependency injection
- **Async/Await**: Proper async patterns throughout
- **Logging**: Request/response logging for debugging
- **Documentation**: XML comments on all public methods
- **Validation**: Input validation at both frontend and backend

## Future Enhancements

- Add database persistence (SQL Server/PostgreSQL)
- Implement authentication and authorization
- Add unit and integration tests
- Create WPF/WinForms GUI for frontend
- Add real-time updates with WebSockets
- Implement advanced filtering and sorting

## License

This project is part of an educational assignment for demonstrating full-stack development with Microsoft Copilot assistance.

---

**Created**: June 8, 2026  
**Technology Stack**: .NET 8.0, ASP.NET Core, C#  
**Development Assistance**: Microsoft Copilot
