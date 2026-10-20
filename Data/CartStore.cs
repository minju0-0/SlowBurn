using System.Globalization;
using SlowBurn.Models;

namespace SlowBurn.Data;

/// <summary>
/// In-memory cart shared by every page. It is static so state survives page navigation without
/// touching Program.cs. Fine for a single-user demo; a real shop would use a per-user service.
/// </summary>
public static class CartStore
{
    public const decimal FreeShippingAt = 60m;
    public const int MaxPerLine = 9;
    private const decimal PromoRate = 0.10m;

    private static readonly List<CartItem> items = new();
    private static readonly object gate = new();

    public static event Action? Changed;

    public static string? Promo { get; private set; }
    public static List<CartItem> Items { get { lock (gate) return items.ToList(); } }
    public static int Count => Items.Sum(i => i.Quantity);
    public static decimal Subtotal => Items.Sum(i => i.LineTotal);
    public static decimal Discount => Promo is null ? 0 : Math.Round(Subtotal * PromoRate, 2);
    public static decimal Net => Subtotal - Discount;

    /// <summary>Adds items, refusing to go over stock or the per-product cap. Returns false if refused.</summary>
    public static bool Add(Product p, GrindSize? grind, int qty = 1)
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

    public static void SetQty(CartItem line, int qty)
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

    public static void Remove(CartItem line)
    {
        lock (gate) items.Remove(line);
        Changed?.Invoke();
    }

    public static void Clear()
    {
        lock (gate) { items.Clear(); Promo = null; }
        Changed?.Invoke();
    }

    public static bool TryApplyPromo(string? code)
    {
        if (!string.Equals(code?.Trim(), "FIRSTPOUR", StringComparison.OrdinalIgnoreCase)) return false;
        Promo = "FIRSTPOUR";
        Changed?.Invoke();
        return true;
    }

    public static decimal Shipping(DeliveryMethod method) => method switch
    {
        DeliveryMethod.Pickup => 0m,
        DeliveryMethod.Express => 14m,
        _ => Net >= FreeShippingAt ? 0m : 6m
    };
}

public static class Money
{
    public static string Fmt(decimal v) => "$" + v.ToString("N2", CultureInfo.InvariantCulture);
}