using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SaveItForPantry.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<ItemData> ItemData { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationItem> LocationItems { get; set; } = null!;
        public DbSet<ShoppingList> ShoppingLists { get; set; } = null!;
        public DbSet<ShoppingListItem> ShoppingListItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // ...existing code...
        }
    }
}
