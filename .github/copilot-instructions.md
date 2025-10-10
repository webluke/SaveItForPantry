# SaveItForPantry - AI Coding Guidelines

## Architecture Overview
This is a Blazor Server application built with .NET 9 for pantry inventory management. The app integrates UPC barcode lookup, location-based organization, and shopping list generation.

**Core Components:**
- **Data Layer**: Entity Framework Core with MySQL (Pomelo provider) - see `Data/ApplicationDbContext.cs`
- **Service Layer**: Business logic in `Services/` (ItemService, LocationService, ShoppingListService)
- **UI Layer**: MudBlazor components organized by feature in `Components/`

**Key Domain Models:**
- `ItemData`: UPC-looked up product information (title, brand, prices, images)
- `Location`: Physical storage locations (pantry shelves, cabinets)
- `LocationItem`: Junction table with quantity and expiration tracking
- `ShoppingList`/`ShoppingListItem`: Generated shopping lists

## Development Patterns

### Service Layer Pattern
All business logic goes through services injected with `IHttpClientFactory` and `ApplicationDbContext`. Example:
```csharp
public class ItemService(IHttpClientFactory httpFactory, ApplicationDbContext db)
{
    // Service methods here
}
```
Services are registered as scoped in `Program.cs`.

### UPC API Integration
UPC lookups use a named HttpClient configured in `Program.cs`:
```csharp
builder.Services.AddHttpClient("upc", client => {
    client.BaseAddress = new Uri("https://api.upcitemdb.com/");
});
```
API responses are deserialized with case-insensitive JSON options.

### Database Operations
- Use EF Core async methods (`FirstOrDefaultAsync`, `SaveChangesAsync`)
- Cache UPC data locally to avoid repeated API calls
- Migrations are applied with `dotnet ef database update`

### UI Components
- Use MudBlazor components throughout (`MudTable`, `MudDialog`, `MudAutocomplete`)
- Components are feature-organized in `Components/{Feature}/`
- Inject services and use `@attribute [Authorize]` for protected pages
- Navigation uses `NavigationManager`, feedback via `ISnackbar`

### Configuration
- Connection strings in `appsettings.json`
- User secrets for development (`dotnet user-secrets`)
- Environment-specific settings in `appsettings.{Environment}.json`

## Common Workflows

### Adding New Items
1. Check local `ItemData` cache first
2. Call UPC API if not found
3. Store API response in database
4. Create `LocationItem` with quantity

### Running the App
```bash
dotnet run  # Development on http://localhost:5295
dotnet ef database update  # Apply migrations
```

### Database Schema Changes
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

## Code Style Notes
- Nullable enabled with implicit usings
- Async/await throughout for I/O operations
- Custom exceptions like `NoApiResultException` for API failures
- Use `Nanoid` for generating short IDs (see `LocationItem.ShortId`)
- Date/time stored as UTC (`DateTime.UtcNow`)

## Key Files to Reference
- `Program.cs`: DI configuration and service registration
- `Data/ApplicationDbContext.cs`: Database schema and relationships
- `Services/ItemService.cs`: UPC lookup and caching logic
- `Components/Items/ItemList.razor`: MudBlazor table patterns