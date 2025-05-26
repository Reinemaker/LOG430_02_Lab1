using MongoDB.Driver;
using CornerShop.Models;
using MongoDB.Bson;

namespace CornerShop.Services;

public class DatabaseService : IDatabaseService
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Product> _products;
    private readonly IMongoCollection<Sale> _sales;

    public DatabaseService(string connectionString = "mongodb://localhost:27017", string databaseName = "cornerShop")
    {
        try
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
            _products = _database.GetCollection<Product>("products");
            _sales = _database.GetCollection<Sale>("sales");
            Console.WriteLine("Successfully connected to MongoDB!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to MongoDB: {ex.Message}");
            throw;
        }
    }

    public DatabaseService(IMongoDatabase database)
    {
        _database = database;
        _products = _database.GetCollection<Product>("products");
        _sales = _database.GetCollection<Sale>("sales");
    }

    public async Task InitializeDatabase()
    {
        // Check if we already have products
        var existingProducts = await _products.Find(_ => true).ToListAsync();
        if (existingProducts.Any())
        {
            Console.WriteLine("Database already contains products.");
            return;
        }

        var sampleProducts = new List<Product>
        {
            new Product
            {
                Name = "Laptop",
                Category = "Electronics",
                Price = 999.99m,
                StockQuantity = 10
            },
            new Product
            {
                Name = "Smartphone",
                Category = "Electronics",
                Price = 499.99m,
                StockQuantity = 20
            },
            new Product
            {
                Name = "Headphones",
                Category = "Electronics",
                Price = 79.99m,
                StockQuantity = 50
            },
            new Product
            {
                Name = "Coffee Maker",
                Category = "Appliances",
                Price = 49.99m,
                StockQuantity = 15
            },
            new Product
            {
                Name = "Desk Chair",
                Category = "Furniture",
                Price = 129.99m,
                StockQuantity = 8
            },
            new Product
            {
                Name = "Apple",
                Category = "Fruits",
                Price = 1.99m,
                StockQuantity = 100
            },
            new Product
            {
                Name = "Banana",
                Category = "Fruits",
                Price = 0.99m,
                StockQuantity = 100
            },
            new Product
            {
                Name = "Orange",
                Category = "Fruits",
                Price = 0.99m,
                StockQuantity = 100
            },
            new Product
            {
                Name = "Pineapple",
                Category = "Fruits",
                Price = 1.99m,
                StockQuantity = 100
            }
        };

        await _products.InsertManyAsync(sampleProducts);
        Console.WriteLine("Sample products added to database.");
    }

    public async Task<List<Product>> SearchProducts(string searchTerm)
    {
        var filter = Builders<Product>.Filter.Regex(p => p.Name,
            new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"));
        var cursor = await _products.FindAsync(filter);
        return await cursor.ToListAsync();
    }

    public async Task<Product?> GetProductByName(string name)
    {
        var filter = Builders<Product>.Filter.Regex(p => p.Name,
            new MongoDB.Bson.BsonRegularExpression($"^{name}$", "i"));
        var cursor = await _products.FindAsync(filter);
        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateProductStock(string productName, int quantity)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Name, productName);
        var update = Builders<Product>.Update.Inc(p => p.StockQuantity, quantity);
        var result = await _products.UpdateOneAsync(filter, update);
        return result.ModifiedCount > 0;
    }

    public async Task<string> CreateSale(Sale sale)
    {
        if (string.IsNullOrEmpty(sale.Id))
        {
            sale.Id = ObjectId.GenerateNewId().ToString();
        }
        await _sales.InsertOneAsync(sale);
        return sale.Id;
    }

    public async Task<List<Sale>> GetRecentSales(int limit = 10)
    {
        return await _sales.Find(_ => true)
            .Sort(Builders<Sale>.Sort.Descending(s => s.Date))
            .Limit(limit)
            .ToListAsync();
    }

    public async Task<Sale?> GetSaleById(string id)
    {
        var filter = Builders<Sale>.Filter.Eq(s => s.Id, id);
        var cursor = await _sales.FindAsync(filter);
        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<bool> CancelSale(string saleId)
    {
        var sale = await GetSaleById(saleId);
        if (sale == null) return false;

        foreach (var item in sale.Items)
        {
            await UpdateProductStock(item.ProductName, item.Quantity);
        }

        var filter = Builders<Sale>.Filter.Eq(s => s.Id, saleId);
        var result = await _sales.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }

    public async Task<List<Product>> GetAllProducts()
    {
        return await _products.Find(_ => true).ToListAsync();
    }

    public async Task PrintAllProductNames()
    {
        var products = await GetAllProducts();
        foreach (var product in products)
        {
            Console.WriteLine($"Product: {product.Name}");
            Console.WriteLine();
        }
    }
}
