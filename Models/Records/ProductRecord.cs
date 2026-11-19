using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using SlowBurn.Models;

namespace SlowBurn.Models.Records;

[Table("products")]
public class ProductRecord : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("sku")]
    public string Sku { get; set; } = "";

    [Column("name")]
    public string Name { get; set; } = "";

    [Column("origin")]
    public string Origin { get; set; } = "";

    [Column("notes")]
    public string Notes { get; set; } = "";

    [Column("description")]
    public string Description { get; set; } = "";

    [Column("price")]
    public decimal Price { get; set; }

    [Column("stock")]
    public int Stock { get; set; }

    [Column("is_new")]
    public bool IsNew { get; set; }

    [Column("category")]
    public string Category { get; set; } = "Beans";

    [Column("roast")]
    public string Roast { get; set; } = "None";

    [Column("art")]
    public string Art { get; set; } = "bag";

    [Column("tone")]
    public string Tone { get; set; } = "#7b2d12";

    [Column("image_url")]
    public string? ImageUrl { get; set; }

    [Column("details")]
    public List<string> Details { get; set; } = new();

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    public Product ToProduct() => new()
    {
        Id = Id,
        Sku = Sku,
        Name = Name,
        Origin = Origin,
        Notes = Notes,
        Description = Description,
        Price = Price,
        Stock = Stock,
        IsNew = IsNew,
        Category = Enum.TryParse<ProductCategory>(Category, true, out var c) ? c : ProductCategory.Beans,
        Roast = Enum.TryParse<RoastLevel>(Roast, true, out var r) ? r : RoastLevel.None,
        Art = Art,
        Tone = Tone,
        ImageUrl = ImageUrl,
        Details = Details != null ? new List<string>(Details) : new()
    };

    public static ProductRecord From(Product p) => new()
    {
        Id = p.Id,
        Sku = p.Sku,
        Name = p.Name,
        Origin = p.Origin,
        Notes = p.Notes,
        Description = p.Description,
        Price = p.Price,
        Stock = p.Stock,
        IsNew = p.IsNew,
        Category = p.Category.ToString(),
        Roast = p.Roast.ToString(),
        Art = p.Art,
        Tone = p.Tone,
        ImageUrl = p.ImageUrl,
        Details = p.Details != null ? new List<string>(p.Details) : new()
    };
}
