using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using SlowBurn.Models;

namespace SlowBurn.Models.Records;

[Table("brew_methods")]
public class BrewMethodRecord : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = "";

    [Column("tagline")]
    public string Tagline { get; set; } = "";

    [Column("ratio")]
    public double Ratio { get; set; }

    [Column("temp_c")]
    public int TempC { get; set; }

    [Column("seconds")]
    public int Seconds { get; set; }

    [Column("ml_per_serving")]
    public int MlPerServing { get; set; }

    [Column("max_servings")]
    public int MaxServings { get; set; }

    [Column("unit")]
    public string Unit { get; set; } = "cup";

    [Column("grind")]
    public string Grind { get; set; } = "Filter";

    [Column("steps")]
    public List<string> Steps { get; set; } = new();

    [Column("sort_order")]
    public int SortOrder { get; set; }

    public BrewMethod ToBrewMethod() => new()
    {
        Name = Name,
        Tagline = Tagline,
        Ratio = Ratio,
        TempC = TempC,
        Seconds = Seconds,
        MlPerServing = MlPerServing,
        MaxServings = MaxServings,
        Unit = Unit,
        Grind = Enum.TryParse<GrindSize>(Grind, true, out var g) ? g : GrindSize.Filter,
        Steps = Steps != null ? new List<string>(Steps) : new()
    };
}
