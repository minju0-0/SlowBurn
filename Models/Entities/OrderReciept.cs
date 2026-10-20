namespace SlowBurn.Models;

public class OrderReceipt
{
    public string Reference { get; set; } = "";
    public string Email { get; set; } = "";
    public int ItemCount { get; set; }
    public decimal Total { get; set; }
    public DeliveryMethod Delivery { get; set; }
}