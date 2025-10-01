using System;
using System.ComponentModel.DataAnnotations;

namespace SaveItForPantry.Data
{
    public class LocationItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LocationId { get; set; }
        public Location Location { get; set; }

        [Required]
        public int UpcDataId { get; set; }
        public UpcData UpcData { get; set; }

        public int Quantity { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public DateTime? ExpirationDate { get; set; }
        public string ShortId { get; set; }
    }
}
