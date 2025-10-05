using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaveItForPantry.Data
{
    public class ShoppingListItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ShoppingListId { get; set; }
        public ShoppingList ShoppingList { get; set; }

        [Required]
        public int ItemDataId { get; set; }

        public ItemData? ItemData { get; set; }

        public int Quantity { get; set; }
        public bool InCart { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}
