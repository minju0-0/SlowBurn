namespace SlowBurn.Models;

public class BrewMethod
{
    public string Name { get; set; } = "";
    public string Tagline { get; set; } = "";
    public double Ratio { get; set; }          // grams of water per gram of coffee
    public int TempC { get; set; }
    public int Seconds { get; set; }
    public int MlPerServing { get; set; }
    public int MaxServings { get; set; }
    public string Unit { get; set; } = "cup";
    public GrindSize Grind { get; set; }
    public List<string> Steps { get; set; } = new();
}