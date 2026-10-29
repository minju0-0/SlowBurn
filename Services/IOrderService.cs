using SlowBurn.Models;

namespace SlowBurn.Services;

/// <summary>
/// Service abstraction for processing and recording customer orders.
/// </summary>
public interface IOrderService
{
    Task<OrderReceipt> PlaceOrderAsync(CheckoutForm form, ICartService cart);
}
