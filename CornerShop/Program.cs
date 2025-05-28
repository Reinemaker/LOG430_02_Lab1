using CornerShop.Services;
using CornerShop.Models;

namespace CornerShop;

public class Program
{
    private static readonly IDatabaseService _db = new DatabaseService("mongodb://localhost:27017", "cornerShop");
    private static readonly IProductService _productService = new ProductService(_db);
    private static readonly ISaleService _saleService = new SaleService(_db, _productService);

    public static async Task Main(string[] args)
    {
        try
        {
            await _db.InitializeDatabase();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing database: {ex.Message}");
            return;
        }

        while (true)
        {
            DisplayMenu();
            var choice = Console.ReadLine();
            Console.WriteLine();

            try
            {
                await ProcessUserChoice(choice);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private static void DisplayMenu()
    {
        Console.WriteLine("\nCorner Shop Management System");
        Console.WriteLine("1. Search Products");
        Console.WriteLine("2. Register Sale");
        Console.WriteLine("3. Cancel Sale");
        Console.WriteLine("4. Check Stock");
        Console.WriteLine("5. Exit");
        Console.Write("\nSelect an option: ");
    }

    private static async Task ProcessUserChoice(string? choice)
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
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }

    private static async Task SearchProducts()
    {
        Console.Write("Enter search term: ");
        var searchTerm = Console.ReadLine()?.ToLower() ?? string.Empty;
        
        var products = await _productService.SearchProducts(searchTerm);
        DisplayProducts(products);
    }

    private static void DisplayProducts(List<Product> products)
    {
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
        var transactionItems = new List<(string Name, int Quantity, decimal Price, decimal Subtotal)>();

        while (true)
        {
            Console.Write("Enter product name (or 'done' to finish): ");
            var productName = Console.ReadLine();
            if (productName?.ToLower() == "done") break;

            if (!await _productService.ValidateProductExists(productName ?? string.Empty))
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

            if (!await _productService.ValidateStockAvailability(productName ?? string.Empty, quantity))
            {
                Console.WriteLine("Insufficient stock.");
                continue;
            }

            var product = await _productService.GetProductByName(productName ?? string.Empty);
            if (product == null) continue;

            var subtotal = product.Price * quantity;
            transactionItems.Add((product.Name, quantity, product.Price, subtotal));

            var item = new SaleItem
            {
                ProductName = product.Name,
                Quantity = quantity,
                Price = product.Price
            };

            sale.Items.Add(item);
        }

        if (!sale.Items.Any())
        {
            Console.WriteLine("No items added to sale.");
            return;
        }

        DisplayTransactionSummary(transactionItems);
        var saleId = await _saleService.CreateSale(sale);
        Console.WriteLine($"\nSale registered successfully. Sale ID: {saleId}");
    }

    private static void DisplayTransactionSummary(List<(string Name, int Quantity, decimal Price, decimal Subtotal)> items)
    {
        Console.WriteLine("\nTransaction Summary:");
        Console.WriteLine("===================");
        Console.WriteLine($"Date: {DateTime.Now:g}");
        Console.WriteLine("\nItems:");
        foreach (var item in items)
        {
            Console.WriteLine($"- {item.Name}");
            Console.WriteLine($"  Quantity: {item.Quantity}");
            Console.WriteLine($"  Price: ${item.Price:F2}");
            Console.WriteLine($"  Subtotal: ${item.Subtotal:F2}");
            Console.WriteLine();
        }
        Console.WriteLine($"Total: ${items.Sum(i => i.Subtotal):F2}");
        Console.WriteLine("===================");
    }

    private static async Task CancelSale()
    {
        Console.WriteLine("\nRecent Sales:");
        var recentSales = await _saleService.GetRecentSales();

        if (!recentSales.Any())
        {
            Console.WriteLine("No recent sales found.");
            return;
        }

        DisplayRecentSales(recentSales);
        var choice = GetSaleChoice(recentSales.Count);

        if (choice == 0) return;

        var selectedSale = recentSales[choice - 1];
        if (!ConfirmSaleCancellation(selectedSale)) return;

        var success = await _saleService.CancelSale(selectedSale.Id);
        DisplayCancellationResult(success);
    }

    private static void DisplayRecentSales(List<Sale> sales)
    {
        for (int i = 0; i < sales.Count; i++)
        {
            var sale = sales[i];
            Console.WriteLine($"\n{i + 1}. Sale ID: {sale.Id}");
            Console.WriteLine($"   Date: {sale.Date:g}");
            Console.WriteLine($"   Total: ${sale.Total:F2}");
            Console.WriteLine("   Items:");
            foreach (var item in sale.Items)
            {
                Console.WriteLine($"   - {item.ProductName}: {item.Quantity} x ${item.Price:F2}");
            }
        }
    }

    private static int GetSaleChoice(int maxSales)
    {
        Console.Write("\nEnter the number of the sale to cancel (or 0 to go back): ");
        if (!int.TryParse(Console.ReadLine(), out var choice) || choice < 0 || choice > maxSales)
        {
            Console.WriteLine("Invalid selection.");
            return 0;
        }
        return choice;
    }

    private static bool ConfirmSaleCancellation(Sale sale)
    {
        Console.WriteLine($"\nAre you sure you want to cancel this sale?");
        Console.WriteLine($"Total: ${sale.Total:F2}");
        Console.Write("Enter 'yes' to confirm: ");
        return Console.ReadLine()?.ToLower() == "yes";
    }

    private static void DisplayCancellationResult(bool success)
    {
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
        var products = await _productService.GetAllProducts();
        DisplayProducts(products);
    }
}
