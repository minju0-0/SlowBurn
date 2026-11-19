using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SlowBurn.Models.Records;

[Table("order_items")]
public class OrderItemRecord : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("order_reference")]
    public string OrderReference { get; set; } = "";

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("grind")]
    public string? Grind { get; set; }

    [Column("unit_price")]
    public decimal UnitPrice { get; set; }
}
