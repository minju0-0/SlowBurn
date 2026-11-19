using SlowBurn.Models;

namespace SlowBurn.Services;

/// <summary>
/// Service abstraction for retrieving, searching, and filtering products and brew guides from Supabase.
/// </summary>
public interface IProductService
{
    // Phase 2 Async API
    Task<int> CountAsync();
    Task<List<Product>> GetFilteredAsync(string? search, ProductCategory? category, RoastLevel? roast, string? sort);
    Task<(Product Hero, Product Starter, IReadOnlyList<Product> Lots)> GetHomeAsync();
    Task<List<BrewMethod>> GetBrewMethodsAsync();
    Task<Product> CreateAsync(Product product);
    Task UpdateAsync(int id, decimal price, int stock);
    Task SoftDeleteAsync(int id);

    // Synchronous adapters for Razor components prior to Phase 3
    IReadOnlyList<Product> GetAll();
    IEnumerable<Product> GetFiltered(string? search, ProductCategory? category, RoastLevel? roast, string? sort);
    Product? GetById(int id);
    Product GetHero();
    Product GetStarterBundle();
    IReadOnlyList<Product> GetBeanLots();
    IReadOnlyList<BrewMethod> GetBrewMethods();
}
