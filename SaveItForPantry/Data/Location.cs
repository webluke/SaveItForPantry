using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaveItForPantry.Data
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<LocationItem> LocationItems { get; set; } = new();
    }
}
