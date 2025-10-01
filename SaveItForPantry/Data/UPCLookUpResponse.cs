using System.Text.Json.Serialization;

namespace SaveItForPantry.Data;

public class UPCLookUpResponse
{
    public string code { get; set; }
    public int total { get; set; }
    public int offset { get; set; }
    public Item[] items { get; set; }
}

public class Item
{
    public string ean { get; set; }
    public string title { get; set; }
    public string upc { get; set; }
    public string gtin { get; set; }
    public string asin { get; set; }
    public string description { get; set; }
    public string brand { get; set; }
    public string model { get; set; }
    public string dimension { get; set; }
    public string weight { get; set; }
    public string category { get; set; }
    public string currency { get; set; }
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double? lowest_recorded_price { get; set; }
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double? highest_recorded_price { get; set; }
    public string[] images { get; set; }
    public Offer[] offers { get; set; }
}

public class Offer
{
    public string merchant { get; set; }
    public string domain { get; set; }
    public string title { get; set; }
    public string currency { get; set; }
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double? list_price { get; set; }
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double? price { get; set; }
    public string shipping { get; set; }
    public string condition { get; set; }
    public string availability { get; set; }
    public string link { get; set; }
    public long updated_t { get; set; }
}

