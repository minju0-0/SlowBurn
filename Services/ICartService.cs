using SlowBurn.Models;

namespace SlowBurn.Services;

/// <summary>
/// Scoped cart service contract for maintaining cart state, applying promotions,
/// and computing subtotals and shipping.
/// </summary>
public interface ICartService
{
    event Action? Changed;

    string? Promo { get; }
    List<CartItem> Items { get; }
    int Count { get; }
    decimal Subtotal { get; }
    decimal Discount { get; }
    decimal Net { get; }
    decimal FreeShippingAt { get; }
    int MaxPerLine { get; }

    bool Add(Product p, GrindSize? grind, int qty = 1);
    void SetQty(CartItem line, int qty);
    void Remove(CartItem line);
    void Clear();
    bool TryApplyPromo(string? code);
    decimal Shipping(DeliveryMethod method);
}
