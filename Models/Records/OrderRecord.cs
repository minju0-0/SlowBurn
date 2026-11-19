using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SlowBurn.Models.Records;

[Table("orders")]
public class OrderRecord : BaseModel
{
    [PrimaryKey("reference", true)]
    public string Reference { get; set; } = "";

    [Column("full_name")]
    public string FullName { get; set; } = "";

    [Column("email")]
    public string Email { get; set; } = "";

    [Column("phone")]
    public string Phone { get; set; } = "";

    [Column("delivery")]
    public string Delivery { get; set; } = "Standard";

    [Column("address")]
    public string? Address { get; set; }

    [Column("city")]
    public string? City { get; set; }

    [Column("postal_code")]
    public string? PostalCode { get; set; }

    [Column("note")]
    public string Note { get; set; } = "";

    [Column("item_count")]
    public int ItemCount { get; set; }

    [Column("total")]
    public decimal Total { get; set; }
}
