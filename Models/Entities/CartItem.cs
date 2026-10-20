namespace SlowBurn.Models;

public class CartItem
{
    public Product Product { get; set; } = default!;
    public int Quantity { get; set; }
    public GrindSize? Grind { get; set; }   // only for beans

    public decimal LineTotal => Product.Price * Quantity;
}