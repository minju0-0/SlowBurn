using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SlowBurn.Models.Records;

[Table("contact_messages")]
public class ContactMessageRecord : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = "";

    [Column("email")]
    public string Email { get; set; } = "";

    [Column("message")]
    public string Message { get; set; } = "";
}
