using Supabase.Postgrest;
using SlowBurn.Models;
using SlowBurn.Models.Records;

namespace SlowBurn.Services;

/// <summary>
/// Handles business logic for final checkout order placement and recording to Supabase.
/// </summary>
public class OrderService : IOrderService
{
    private readonly Supabase.Client _client;

    public OrderService(Supabase.Client client)
    {
        _client = client;
    }

    public async Task<OrderReceipt> PlaceOrderAsync(CheckoutForm form, ICartService cart)
    {
        if (cart.Items.Count == 0)
        {
            throw new InvalidOperationException("Cannot place an order with an empty cart.");
        }

        var shipping = cart.Shipping(form.Delivery);
        var total = cart.Net + shipping;
        var reference = $"SB-{Random.Shared.Next(100000, 999999)}";

        var orderRecord = new OrderRecord
        {
            Reference = reference,
            FullName = form.FullName.Trim(),
            Email = form.Email.Trim(),
            Phone = form.Phone.Trim(),
            Delivery = form.Delivery.ToString(),
            Address = string.IsNullOrWhiteSpace(form.Address) ? null : form.Address.Trim(),
            City = string.IsNullOrWhiteSpace(form.City) ? null : form.City.Trim(),
            PostalCode = string.IsNullOrWhiteSpace(form.PostalCode) ? null : form.PostalCode.Trim(),
            Note = form.Note?.Trim() ?? "",
            ItemCount = cart.Count,
            Total = total
        };

        var options = new QueryOptions { Returning = QueryOptions.ReturnType.Minimal };

        // Insert order record
        await _client.From<OrderRecord>().Insert(orderRecord, options);

        // Insert order line items in one batch
        var itemRecords = cart.Items.Select(line => new OrderItemRecord
        {
            OrderReference = reference,
            ProductId = line.Product.Id,
            Quantity = line.Quantity,
            Grind = line.Grind?.ToString(),
            UnitPrice = line.Product.Price
        }).ToList();

        if (itemRecords.Count > 0)
        {
            await _client.From<OrderItemRecord>().Insert(itemRecords, options);
        }

        var receipt = new OrderReceipt
        {
            Reference = reference,
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
