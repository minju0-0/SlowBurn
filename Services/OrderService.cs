using SlowBurn.Models;

namespace SlowBurn.Services;

/// <summary>
/// Handles business logic for final checkout order placement, inventory dispatch queuing,
/// and receipt generation.
/// </summary>
public class OrderService : IOrderService
{
    public async Task<OrderReceipt> PlaceOrderAsync(CheckoutForm form, ICartService cart)
    {
        if (cart.Items.Count == 0)
        {
            throw new InvalidOperationException("Cannot place an order with an empty cart.");
        }

        // Simulate asynchronous order submission and roast queuing
        await Task.Delay(1000);

        var shipping = cart.Shipping(form.Delivery);
        var total = cart.Net + shipping;

        var receipt = new OrderReceipt
        {
            Reference = $"SB-{Random.Shared.Next(10000, 99999)}",
            Email = form.Email,
            ItemCount = cart.Count,
            Total = total,
            Delivery = form.Delivery,
            CreatedAt = DateTime.UtcNow
        };

        cart.Clear();
        return receipt;
    }
}
