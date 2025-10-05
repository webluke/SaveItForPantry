using Microsoft.EntityFrameworkCore;
using SaveItForPantry.Data;

namespace SaveItForPantry.Services
{
    public class ShoppingListService
    {
        private readonly ApplicationDbContext _db;

        public ShoppingListService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<ShoppingList>> GetShoppingListsAsync()
        {
            return await _db.ShoppingLists
                .Include(sl => sl.Items)
                .ThenInclude(sli => sli.ItemData)
                .ToListAsync();
        }

        public async Task<ShoppingList?> GetShoppingListByIdAsync(int id)
        {
            return await _db.ShoppingLists
                .Include(sl => sl.Items)
                .ThenInclude(sli => sli.ItemData)
                .FirstOrDefaultAsync(sl => sl.Id == id);
        }

        public async Task<ShoppingList> CreateShoppingListAsync(ShoppingList shoppingList)
        {
            _db.ShoppingLists.Add(shoppingList);
            await _db.SaveChangesAsync();
            return shoppingList;
        }

        public async Task AddItemToShoppingListAsync(int shoppingListId, int upcDataId, int quantityToBuy)
        {
            var existingItem = await _db.ShoppingListItems
                .FirstOrDefaultAsync(sli => sli.ShoppingListId == shoppingListId && sli.ItemDataId == upcDataId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantityToBuy;
            }
            else
            {
                var item = new ShoppingListItem
                {
                    ShoppingListId = shoppingListId,
                    ItemDataId = upcDataId,
                    Quantity = quantityToBuy
                };
                _db.ShoppingListItems.Add(item);
            }
            await _db.SaveChangesAsync();
        }

        public async Task RemoveItemFromShoppingListAsync(int itemId)
        {
            var item = await _db.ShoppingListItems.FindAsync(itemId);
            if (item != null)
            {
                _db.ShoppingListItems.Remove(item);
                await _db.SaveChangesAsync();
            }
        }

        public async Task UpdateShoppingListAsync(ShoppingList shoppingList)
        {
            _db.ShoppingLists.Update(shoppingList);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteShoppingListAsync(int id)
        {
            var items = await _db.ShoppingListItems.Where(sli => sli.ShoppingListId == id).ToListAsync();
            _db.ShoppingListItems.RemoveRange(items);

            var list = await _db.ShoppingLists.FindAsync(id);
            if (list != null)
            {
                _db.ShoppingLists.Remove(list);
                await _db.SaveChangesAsync();
            }
        }

        public async Task UpdateShoppingListItemAsync(ShoppingListItem item)
        {
            var existingItem = await _db.ShoppingListItems.FindAsync(item.Id);
            if (existingItem != null)
            {
                _db.Entry(existingItem).CurrentValues.SetValues(item);
                await _db.SaveChangesAsync();
            }
        }
    }
}
