using SlowBurn.Data;
using SlowBurn.Models;

namespace SlowBurn.Services;

/// <summary>
/// Encapsulates product catalog retrieval, filtering, and sorting business logic.
/// </summary>
public class ProductService : IProductService
{
    public IReadOnlyList<Product> GetAll() => SampleData.Products;

    public IEnumerable<Product> GetFiltered(string? search, ProductCategory? category, RoastLevel? roast, string? sort)
    {
        var q = SampleData.Products.Where(p =>
            (category is null || p.Category == category) &&
            (roast is null || p.Roast == roast) &&
            (string.IsNullOrWhiteSpace(search)
                || p.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                || p.Origin.Contains(search, StringComparison.OrdinalIgnoreCase)
                || p.Notes.Contains(search, StringComparison.OrdinalIgnoreCase)
                || p.Sku.Contains(search, StringComparison.OrdinalIgnoreCase)));

        return sort switch
        {
            "price-asc" => q.OrderBy(p => p.Price),
            "price-desc" => q.OrderByDescending(p => p.Price),
            "name" => q.OrderBy(p => p.Name),
            _ => q
        };
    }

    public Product? GetById(int id) => SampleData.Products.FirstOrDefault(p => p.Id == id);

    public Product GetHero() => SampleData.Products[0];

    public Product GetStarterBundle() => SampleData.Products.First(p => p.Category == ProductCategory.Bundles);

    public IReadOnlyList<Product> GetBeanLots() => SampleData.Products.Where(p => p.Category == ProductCategory.Beans).ToList();

    public IReadOnlyList<BrewMethod> GetBrewMethods() => SampleData.BrewMethods;
}
