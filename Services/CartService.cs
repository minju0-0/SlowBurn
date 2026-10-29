using SlowBurn.Models;

namespace SlowBurn.Services;

/// <summary>
/// Scoped shopping cart service for managing user cart items, promotional discounts,
/// and delivery cost calculations.
/// </summary>
public class CartService : ICartService
{
    public const decimal FreeShippingThreshold = 60m;
    public const int MaxPerLineLimit = 9;
    private const decimal PromoRate = 0.10m;

    private readonly List<CartItem> items = new();
    private readonly object gate = new();

    public event Action? Changed;

    public string? Promo { get; private set; }
    public List<CartItem> Items { get { lock (gate) return items.ToList(); } }
    public int Count => Items.Sum(i => i.Quantity);
    public decimal Subtotal => Items.Sum(i => i.LineTotal);
    public decimal Discount => Promo is null ? 0 : Math.Round(Subtotal * PromoRate, 2);
    public decimal Net => Subtotal - Discount;
    public decimal FreeShippingAt => FreeShippingThreshold;
    public int MaxPerLine => MaxPerLineLimit;

    /// <summary>Adds items, refusing to go over stock or the per-product cap. Returns false if refused.</summary>
    public bool Add(Product p, GrindSize? grind, int qty = 1)
    {
        lock (gate)
        {
            var limit = Math.Min(p.Stock, MaxPerLine);
            var inCart = items.Where(i => i.Product.Id == p.Id).Sum(i => i.Quantity);
            if (qty < 1 || inCart + qty > limit) return false;

            var line = items.FirstOrDefault(i => i.Product.Id == p.Id && i.Grind == grind);
            if (line is null) items.Add(new CartItem { Product = p, Grind = grind, Quantity = qty });
            else line.Quantity += qty;
        }
        Changed?.Invoke();
        return true;
    }

    public void SetQty(CartItem line, int qty)
    {
        lock (gate)
        {
            if (qty <= 0) items.Remove(line);
            else
            {
                var others = items.Where(i => i.Product.Id == line.Product.Id && i != line).Sum(i => i.Quantity);
                line.Quantity = Math.Min(qty, Math.Min(line.Product.Stock, MaxPerLine) - others);
            }
        }
        Changed?.Invoke();
    }

    public void Remove(CartItem line)
    {
        lock (gate) items.Remove(line);
        Changed?.Invoke();
    }

    public void Clear()
    {
        lock (gate) { items.Clear(); Promo = null; }
        Changed?.Invoke();
    }

    public bool TryApplyPromo(string? code)
    {
        if (!string.Equals(code?.Trim(), "FIRSTPOUR", StringComparison.OrdinalIgnoreCase)) return false;
        Promo = "FIRSTPOUR";
        Changed?.Invoke();
        return true;
    }

    public decimal Shipping(DeliveryMethod method) => method switch
    {
        DeliveryMethod.Pickup => 0m,
        DeliveryMethod.Express => 14m,
        _ => Net >= FreeShippingThreshold ? 0m : 6m
    };
}
