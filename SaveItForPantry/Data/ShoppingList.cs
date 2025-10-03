using System.ComponentModel.DataAnnotations;

namespace SaveItForPantry.Data
{
    public class ShoppingList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public List<ShoppingListItem> Items { get; set; } = new();
    }
}
