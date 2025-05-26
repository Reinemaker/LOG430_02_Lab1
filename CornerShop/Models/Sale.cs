using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CornerShop.Models;

public class Sale
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("date")]
    public DateTime Date { get; set; }

    [BsonElement("items")]
    public List<SaleItem> Items { get; set; } = new();

    [BsonElement("total")]
    public decimal Total { get; set; }

    public Sale()
    {
        Date = DateTime.UtcNow;
    }
}

public class SaleItem
{
    [BsonElement("productName")]
    public string ProductName { get; set; } = string.Empty;

    [BsonElement("quantity")]
    public int Quantity { get; set; }

    [BsonElement("price")]
    public decimal Price { get; set; }
} 