using System.Collections.Generic;
using System.Threading.Tasks;
using CornerShop.Models;

namespace CornerShop.Services;

public interface IDatabaseService
{
    Task<List<Product>> SearchProducts(string searchTerm);
    Task<Product> GetProductByName(string name);
    Task<bool> UpdateProductStock(string productName, int quantity);
    Task<string> CreateSale(Sale sale);
    Task<List<Sale>> GetRecentSales(int limit);
    Task<Sale> GetSaleById(string id);
    Task<bool> CancelSale(string saleId);
    Task<List<Product>> GetAllProducts();
} 