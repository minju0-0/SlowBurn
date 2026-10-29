using SlowBurn.Models;

namespace SlowBurn.Services;

/// <summary>
/// Service abstraction for retrieving, searching, and filtering products and brew guides.
/// </summary>
public interface IProductService
{
    IReadOnlyList<Product> GetAll();
    IEnumerable<Product> GetFiltered(string? search, ProductCategory? category, RoastLevel? roast, string? sort);
    Product? GetById(int id);
    Product GetHero();
    Product GetStarterBundle();
    IReadOnlyList<Product> GetBeanLots();
    IReadOnlyList<BrewMethod> GetBrewMethods();
}
