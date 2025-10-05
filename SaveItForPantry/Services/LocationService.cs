using Microsoft.EntityFrameworkCore;
using NanoidDotNet;
using SaveItForPantry.Data;

namespace SaveItForPantry.Services
{
    public class LocationService
    {
        private readonly ApplicationDbContext _db;

        public LocationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Location>> GetLocationsAsync()
        {
            return await _db.Locations.Include(l => l.LocationItems).ThenInclude(li => li.ItemData).ToListAsync();
        }

        public async Task<Location> GetLocationByIdAsync(int id)
        {
            return await _db.Locations.Include(l => l.LocationItems).ThenInclude(li => li.ItemData).FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Location> CreateLocationAsync(Location location)
        { 
            _db.Locations.Add(location);
            await _db.SaveChangesAsync();
            return location;
        }

        public async Task AddItemToLocationAsync(int locationId, int itemDataId, int quantity, DateTime? expirationDate)
        {
            var locationItem = await _db.LocationItems.FirstOrDefaultAsync(li => li.LocationId == locationId && li.ItemDataId == itemDataId && li.ExpirationDate == expirationDate);

            if (locationItem != null)
            {
                locationItem.Quantity += quantity;
            }
            else
            {
                locationItem = new LocationItem
                {
                    LocationId = locationId,
                    ItemDataId = itemDataId,
                    Quantity = quantity,
                    ExpirationDate = expirationDate,
                    ShortId = GenerateShortId()
                };
                _db.LocationItems.Add(locationItem);
            }

            await _db.SaveChangesAsync();
        }

        public async Task AddUpcToLocationAsync(int locationId, int itemDataId, int quantity, DateTime? expirationDate)
        {
            var location = await GetLocationByIdAsync(locationId);
            var upcData = await _db.ItemData.FindAsync(itemDataId);

            if (location != null && upcData != null)
            {
                var locationItem = new LocationItem
                {
                    LocationId = locationId,
                    ItemDataId = itemDataId,
                    Quantity = quantity,
                    ExpirationDate = expirationDate,
                    ShortId = GenerateShortId()
                };

                location.LocationItems.Add(locationItem);
                await _db.SaveChangesAsync();
            }
        }

        public async Task UpdateLocationAsync(Location location)
        {
            _db.Locations.Update(location);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteLocationAsync(int locationId)
        {
            var location = await _db.Locations.Include(l => l.LocationItems).FirstOrDefaultAsync(l => l.Id == locationId);
            if (location != null)
            {
                _db.LocationItems.RemoveRange(location.LocationItems);
                _db.Locations.Remove(location);
                await _db.SaveChangesAsync();
            }
        }

        public async Task UpdateLocationItemAsync(LocationItem locationItem)
        {
            _db.LocationItems.Update(locationItem);
            await _db.SaveChangesAsync();
        }

        public async Task DecrementItemQuantityAsync(int locationItemId)
        {
            var locationItem = await _db.LocationItems.FindAsync(locationItemId);
            if (locationItem != null)
            { 
                locationItem.Quantity--;
                await _db.SaveChangesAsync();
            }
        }

        public async Task RemoveUpcFromLocationAsync(int locationItemId)
        {
            var locationItem = await _db.LocationItems.FindAsync(locationItemId);
            if (locationItem != null)
            {
                _db.LocationItems.Remove(locationItem);
                await _db.SaveChangesAsync();
            }
        }

        private string GenerateShortId()
        {
            return Nanoid.Generate(Nanoid.Alphabets.NoLookAlikes, 4);
        }
    }
}