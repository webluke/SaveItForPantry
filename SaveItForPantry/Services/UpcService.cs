using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SaveItForPantry.Data;

namespace SaveItForPantry.Services
{
    public class UpcService
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly ApplicationDbContext _db;
        private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web) { PropertyNameCaseInsensitive = true };

        public UpcService(IHttpClientFactory httpFactory, ApplicationDbContext db)
        {
            _httpFactory = httpFactory;
            _db = db;
        }

        public async Task<UpcData?> GetUpcDataByUpcAsync(string upc)
        {
            if (string.IsNullOrWhiteSpace(upc))
            {
                return null;
            }

            var existing = await _db.UpcData.FirstOrDefaultAsync(x => x.Upc == upc);
            if (existing != null)
            {
                return existing;
            }

            try
            {
                var client = _httpFactory.CreateClient("upc");
                var url = $"prod/trial/lookup?upc={Uri.EscapeDataString(upc)}";
                var resp = await client.GetFromJsonAsync<UPCLookUpResponse>(url, _jsonOptions);

                if (resp?.items is null || resp.items.Length == 0)
                {
                    throw new NoApiResultException("No results from the API");
                }

                var apiItem = resp.items[0];

                var newUpcData = new UpcData
                {
                    Upc = apiItem.upc ?? upc,
                    Ean = apiItem.ean,
                    Title = apiItem.title,
                    Description = apiItem.description,
                    Brand = apiItem.brand,
                    Model = apiItem.model,
                    Dimension = apiItem.dimension,
                    Weight = apiItem.weight,
                    Category = apiItem.category,
                    Currency = apiItem.currency,
                    LowestRecordedPrice = apiItem.lowest_recorded_price,
                    HighestRecordedPrice = apiItem.highest_recorded_price,
                    RetrievedAt = DateTime.UtcNow
                };

                if (apiItem.images != null && apiItem.images.Length > 0)
                {
                    newUpcData.ImageUrl = apiItem.images[0];
                }

                if (apiItem.offers != null && apiItem.offers.Length > 0)
                {
                    var offer = apiItem.offers[0];
                    newUpcData.OfferMerchant = offer.merchant;
                    newUpcData.OfferDomain = offer.domain;
                    newUpcData.OfferTitle = offer.title;
                    newUpcData.OfferCurrency = offer.currency;
                    newUpcData.OfferListPrice = (double?)offer.list_price;
                    newUpcData.OfferPrice = (double?)offer.price;
                    newUpcData.OfferShipping = offer.shipping;
                    newUpcData.OfferCondition = offer.condition;
                    newUpcData.OfferAvailability = offer.availability;
                    newUpcData.OfferLink = offer.link;
                    newUpcData.OfferUpdatedT = DateTimeOffset.FromUnixTimeSeconds(offer.updated_t).DateTime;
                }

                _db.UpcData.Add(newUpcData);
                await _db.SaveChangesAsync();

                return newUpcData;
            }
            catch (HttpRequestException ex)
            {
                switch (ex.StatusCode)
                {
                    case System.Net.HttpStatusCode.BadRequest:
                        Console.WriteLine("INVALID_QUERY: missing required parameters");
                        break;
                    case System.Net.HttpStatusCode.NotFound:
                        Console.WriteLine("NOT_FOUND: no match was found");
                        break;
                    case System.Net.HttpStatusCode.TooManyRequests:
                        Console.WriteLine("EXCEED_LIMIT: exceed request limit");
                        break;
                    case System.Net.HttpStatusCode.InternalServerError:
                        Console.WriteLine("SERVER_ERR: internal server error");
                        break;
                    default:
                        Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                        break;
                }
                return null;
            }
        }

        public async Task<UpcData?> GetByIdAsync(int id) =>
            await _db.UpcData.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<UpcData> CreateAsync(UpcData entity)
        {
            _db.UpcData.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(UpcData entity)
        {
            _db.UpcData.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<UpcData[]> SearchLocalAsync(string filter)
        {
            var q = _db.UpcData.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter))
                q = q.Where(x => x.Upc.Contains(filter) || (x.Title ?? "").ToLower().Contains(filter.ToLower()));
            return await q.OrderByDescending(x => x.RetrievedAt).ToArrayAsync();
        }

        public async Task<UpcData[]> SearchAsync(string? upcFilter, string? titleFilter)
        {
            var q = _db.UpcData.AsQueryable();
            if (!string.IsNullOrWhiteSpace(upcFilter))
                q = q.Where(x => x.Upc == upcFilter);
            if (!string.IsNullOrWhiteSpace(titleFilter))
                q = q.Where(x => (x.Title ?? "").ToLower().Contains(titleFilter.ToLower()));
            return await q.OrderByDescending(x => x.RetrievedAt).ToArrayAsync();
        }

        public async Task<UpcData[]> SearchWithLookupAsync(string? upcFilter, string? titleFilter)
        {
            if (!string.IsNullOrWhiteSpace(upcFilter))
            {
                var items = await SearchAsync(upcFilter, null);
                if (items.Length > 0)
                {
                    return items;
                }

                var item = await GetUpcDataByUpcAsync(upcFilter);
                if (item is not null)
                {
                    return new[] { item };
                }
            }

            return await SearchAsync(null, titleFilter);
        }

        public async Task DeleteAsync(UpcData entity)
        {
            _db.UpcData.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<UpcData[]> GetAllAsync() =>
            await _db.UpcData.OrderByDescending(x => x.RetrievedAt).ToArrayAsync();
    }
}