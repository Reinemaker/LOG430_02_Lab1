using CornerShop.Services;
using CornerShop.Models;

namespace CornerShop;

public class Program
{
    private static readonly DatabaseService _db = new("mongodb://localhost:27017", "cornerShop");

    public static async Task Main(string[] args)
    {
        try
        {
            // Initialize database with sample data
            await _db.InitializeDatabase();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing database: {ex.Message}");
            return;
        }

        while (true)
        {
            Console.WriteLine("\nCorner Shop Management System");
            Console.WriteLine("1. Search Products");
            Console.WriteLine("2. Register Sale");
            Console.WriteLine("3. Cancel Sale");
            Console.WriteLine("4. Check Stock");
            Console.WriteLine("5. Exit");
            Console.Write("\nSelect an option: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await SearchProducts();
                        break;
                    case "2":
                        await RegisterSale();
                        break;
                    case "3":
                        await CancelSale();
                        break;
                    case "4":
                        await CheckStock();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private static async Task SearchProducts()
    {
        Console.Write("Enter search term: ");
        var searchTerm = Console.ReadLine()?.ToLower() ?? string.Empty;
        var products = await _db.SearchProducts(searchTerm);

        if (!products.Any())
        {
            Console.WriteLine("No products found.");
            return;
        }

        foreach (var product in products)
        {
            Console.WriteLine($"Name: {product.Name}");
            Console.WriteLine($"Category: {product.Category}");
            Console.WriteLine($"Price: ${product.Price:F2}");
            Console.WriteLine($"Stock: {product.StockQuantity}");
            Console.WriteLine();
        }
    }

    private static async Task RegisterSale()
    {
        var sale = new Sale();
        var total = 0m;
        var transactionItems = new List<(string Name, int Quantity, decimal Price, decimal Subtotal)>();

        while (true)
        {
            Console.Write("Enter product name (or 'done' to finish): ");
            var productName = Console.ReadLine();
            if (productName?.ToLower() == "done") break;

            var product = await _db.GetProductByName(productName ?? string.Empty);
            if (product == null)
            {
                Console.WriteLine("Product not found.");
                continue;
            }

            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out var quantity) || quantity <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                continue;
            }

            if (quantity > product.StockQuantity)
            {
                Console.WriteLine("Insufficient stock.");
                continue;
            }

            var subtotal = product.Price * quantity;
            transactionItems.Add((product.Name, quantity, product.Price, subtotal));

            var item = new SaleItem
            {
                ProductName = product.Name,
                Quantity = quantity,
                Price = product.Price
            };

            sale.Items.Add(item);
            total += subtotal;

            await _db.UpdateProductStock(product.Name, -quantity);
        }

        if (!sale.Items.Any())
        {
            Console.WriteLine("No items added to sale.");
            return;
        }

        // Display transaction summary
        Console.WriteLine("\nTransaction Summary:");
        Console.WriteLine("===================");
        Console.WriteLine($"Date: {DateTime.Now:g}");
        Console.WriteLine("\nItems:");
        foreach (var item in transactionItems)
        {
            Console.WriteLine($"- {item.Name}");
            Console.WriteLine($"  Quantity: {item.Quantity}");
            Console.WriteLine($"  Price: ${item.Price:F2}");
            Console.WriteLine($"  Subtotal: ${item.Subtotal:F2}");
            Console.WriteLine();
        }
        Console.WriteLine($"Total: ${total:F2}");
        Console.WriteLine("===================");

        sale.Total = total;
        var saleId = await _db.CreateSale(sale);
        Console.WriteLine($"\nSale registered successfully. Sale ID: {saleId}");
    }

    private static async Task CancelSale()
    {
        Console.WriteLine("\nRecent Sales:");
        var recentSales = await _db.GetRecentSales();

        if (!recentSales.Any())
        {
            Console.WriteLine("No recent sales found.");
            return;
        }

        // Display recent sales with numbers
        for (int i = 0; i < recentSales.Count; i++)
        {
            var sale = recentSales[i];
            Console.WriteLine($"\n{i + 1}. Sale ID: {sale.Id}");
            Console.WriteLine($"   Date: {sale.Date:g}");
            Console.WriteLine($"   Total: ${sale.Total:F2}");
            Console.WriteLine("   Items:");
            foreach (var item in sale.Items)
            {
                Console.WriteLine($"   - {item.ProductName}: {item.Quantity} x ${item.Price:F2}");
            }
        }

        Console.Write("\nEnter the number of the sale to cancel (or 0 to go back): ");
        if (!int.TryParse(Console.ReadLine(), out var choice) || choice < 0 || choice > recentSales.Count)
        {
            Console.WriteLine("Invalid selection.");
            return;
        }

        if (choice == 0)
        {
            return;
        }

        var selectedSale = recentSales[choice - 1];
        Console.WriteLine($"\nAre you sure you want to cancel this sale?");
        Console.WriteLine($"Total: ${selectedSale.Total:F2}");
        Console.Write("Enter 'yes' to confirm: ");

        if (Console.ReadLine()?.ToLower() != "yes")
        {
            Console.WriteLine("Cancellation aborted.");
            return;
        }

        var success = await _db.CancelSale(selectedSale.Id);
        if (success)
        {
            Console.WriteLine("Sale cancelled successfully.");
            Console.WriteLine("Stock levels have been restored.");
        }
        else
        {
            Console.WriteLine("Failed to cancel sale.");
        }
    }

    private static async Task CheckStock()
    {
        var products = await _db.GetAllProducts();
        foreach (var product in products)
        {
            Console.WriteLine($"Name: {product.Name}");
            Console.WriteLine($"Stock: {product.StockQuantity}");
            Console.WriteLine();
        }
    }
}
