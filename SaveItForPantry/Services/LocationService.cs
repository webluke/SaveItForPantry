using Microsoft.EntityFrameworkCore;
using SaveItForPantry.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

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
            return await _db.Locations.Include(l => l.LocationItems).ThenInclude(li => li.UpcData).ToListAsync();
        }

        public async Task<Location> GetLocationByIdAsync(int id)
        {
            return await _db.Locations.Include(l => l.LocationItems).ThenInclude(li => li.UpcData).FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Location> CreateLocationAsync(Location location)
        { 
            _db.Locations.Add(location);
            await _db.SaveChangesAsync();
            return location;
        }

        public async Task AddUpcToLocationAsync(int locationId, int upcDataId, int quantity, DateTime? expirationDate)
        {
            var location = await GetLocationByIdAsync(locationId);
            var upcData = await _db.UpcData.FindAsync(upcDataId);

            if (location != null && upcData != null)
            {
                var locationItem = new LocationItem
                {
                    LocationId = locationId,
                    UpcDataId = upcDataId,
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
            return Guid.NewGuid().ToString("n").Substring(0, 4);
        }
    }
}