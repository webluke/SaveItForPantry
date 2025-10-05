using System.ComponentModel.DataAnnotations;

namespace SaveItForPantry.Data
{
    public class ItemData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Upc { get; set; }

        public string? Ean { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? Dimension { get; set; }
        public string? Weight { get; set; }
        public string? Category { get; set; }
        public string? Currency { get; set; }
        public double? LowestRecordedPrice { get; set; }
        public double? HighestRecordedPrice { get; set; }

        public string? ImageUrl { get; set; }
        public string? OfferMerchant { get; set; }
        public string? OfferDomain { get; set; }
        public string? OfferTitle { get; set; }
        public string? OfferCurrency { get; set; }
        public double? OfferListPrice { get; set; }
        public double? OfferPrice { get; set; }
        public string? OfferShipping { get; set; }
        public string? OfferCondition { get; set; }
        public string? OfferAvailability { get; set; }
        public string? OfferLink { get; set; }
        public DateTime? OfferUpdatedT { get; set; }

        public DateTime RetrievedAt { get; set; } = DateTime.UtcNow;
    }
}
