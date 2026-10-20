namespace SlowBurn.Models;

public class Product
{
    public int Id { get; set; }
    public string Sku { get; set; } = "";
    public string Name { get; set; } = "";
    public string Origin { get; set; } = "";
    public string Notes { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsNew { get; set; }
    public ProductCategory Category { get; set; }
    public RoastLevel Roast { get; set; }
    public string Art { get; set; } = "bag";          // which inline SVG ProductArt draws
    public string Tone { get; set; } = "#7b2d12";     // main colour of that SVG
    public string? ImageUrl { get; set; }
    public List<string> Details { get; set; } = new();

    public bool HasGrind => Category == ProductCategory.Beans;
}