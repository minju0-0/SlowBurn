using Supabase.Postgrest;
using Supabase.Postgrest.Interfaces;
using Supabase.Postgrest.Linq;
using static Supabase.Postgrest.Constants;
using SlowBurn.Models;
using SlowBurn.Models.Records;

namespace SlowBurn.Services;

/// <summary>
/// Encapsulates product catalog retrieval, filtering, and CRUD operations backed by Supabase.
/// </summary>
public class ProductService : IProductService
{
    private readonly Supabase.Client _client;

    public ProductService(Supabase.Client client)
    {
        _client = client;
    }

    public async Task<int> CountAsync()
    {
        return await _client.From<ProductRecord>()
            .Where(p => p.DeletedAt == null)
            .Count(CountType.Exact);
    }

    public async Task<List<Product>> GetFilteredAsync(string? search, ProductCategory? category, RoastLevel? roast, string? sort)
    {
        var query = _client.From<ProductRecord>()
            .Where(p => p.DeletedAt == null);

        if (category is not null)
        {
            var catStr = category.Value.ToString();
            query = query.Where(p => p.Category == catStr);
        }

        if (roast is not null && roast.Value != RoastLevel.None)
        {
            var roastStr = roast.Value.ToString();
            query = query.Where(p => p.Roast == roastStr);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var clean = new string(search.Where(c => c != ',' && c != '(' && c != ')' && c != '*' && c != '%').ToArray()).Trim();
            if (!string.IsNullOrEmpty(clean))
            {
                var pattern = $"*{clean}*";
                var filters = new List<IPostgrestQueryFilter>
                {
                    new QueryFilter("name", Operator.ILike, pattern),
                    new QueryFilter("origin", Operator.ILike, pattern),
                    new QueryFilter("notes", Operator.ILike, pattern),
                    new QueryFilter("sku", Operator.ILike, pattern)
                };
                query = query.Or(filters);
            }
        }

        query = sort switch
        {
            "price-asc" => query.Order("price", Ordering.Ascending),
            "price-desc" => query.Order("price", Ordering.Descending),
            "name" => query.Order("name", Ordering.Ascending),
            _ => query.Order("id", Ordering.Ascending)
        };

        query = query.Range(0, 49);

        var response = await query.Get();
        return response.Models.Select(m => m.ToProduct()).ToList();
    }

    public async Task<(Product Hero, Product Starter, IReadOnlyList<Product> Lots)> GetHomeAsync()
    {
        var catFilters = new List<IPostgrestQueryFilter>
        {
            new QueryFilter("category", Operator.Equals, "Beans"),
            new QueryFilter("category", Operator.Equals, "Bundles")
        };

        var response = await _client.From<ProductRecord>()
            .Where(p => p.DeletedAt == null)
            .Or(catFilters)
            .Order("id", Ordering.Ascending)
            .Get();

        var items = response.Models.Select(m => m.ToProduct()).ToList();
        var hero = items.FirstOrDefault(p => p.Category == ProductCategory.Beans) ?? new Product();
        var starter = items.FirstOrDefault(p => p.Category == ProductCategory.Bundles) ?? new Product();
        var lots = items.Where(p => p.Category == ProductCategory.Beans).ToList();

        return (hero, starter, lots);
    }

    public async Task<List<BrewMethod>> GetBrewMethodsAsync()
    {
        var response = await _client.From<BrewMethodRecord>()
            .Order("sort_order", Ordering.Ascending)
            .Get();

        return response.Models.Select(m => m.ToBrewMethod()).ToList();
    }

    public async Task<Product> CreateAsync(Product product)
    {
        var record = ProductRecord.From(product);
        var response = await _client.From<ProductRecord>().Insert(record);
        var inserted = response.Models.FirstOrDefault();
        return inserted?.ToProduct() ?? product;
    }

    public async Task UpdateAsync(int id, decimal price, int stock)
    {
        await _client.From<ProductRecord>()
            .Where(x => x.Id == id)
            .Set(x => x.Price, price)
            .Set(x => x.Stock, stock)
            .Update();
    }

    public async Task SoftDeleteAsync(int id)
    {
        await _client.From<ProductRecord>()
            .Where(x => x.Id == id)
            .Set(x => (object)x.DeletedAt!, DateTime.UtcNow)
            .Update();
    }

    // Synchronous adapters for Razor components prior to Phase 3
    public IReadOnlyList<Product> GetAll() =>
        GetFilteredAsync(null, null, null, null).GetAwaiter().GetResult();

    public IEnumerable<Product> GetFiltered(string? search, ProductCategory? category, RoastLevel? roast, string? sort) =>
        GetFilteredAsync(search, category, roast, sort).GetAwaiter().GetResult();

    public Product? GetById(int id) =>
        GetFilteredAsync(null, null, null, null).GetAwaiter().GetResult().FirstOrDefault(p => p.Id == id);

    public Product GetHero() =>
        GetHomeAsync().GetAwaiter().GetResult().Hero;

    public Product GetStarterBundle() =>
        GetHomeAsync().GetAwaiter().GetResult().Starter;

    public IReadOnlyList<Product> GetBeanLots() =>
        GetHomeAsync().GetAwaiter().GetResult().Lots;

    public IReadOnlyList<BrewMethod> GetBrewMethods() =>
        GetBrewMethodsAsync().GetAwaiter().GetResult();
}
