using System;

namespace SaveItForPantry.Data
{
    public class UpcItem
    {
        public int Id { get; set; }
        public string Upc { get; set; } = "";
        public string? Title { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Description { get; set; }
        // Serialized images array as JSON
        public string? ImagesJson { get; set; }
        // Store the raw API JSON for debugging / rehydration
        public string? RawJson { get; set; }
        public DateTime RetrievedAt { get; set; } = DateTime.UtcNow;
    }
}