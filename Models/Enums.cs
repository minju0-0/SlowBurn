namespace SlowBurn.Models;

public enum ProductCategory { Beans, Gear, Bundles }
public enum RoastLevel { None = 0, Light = 1, Medium = 2, Dark = 3 }
public enum GrindSize { WholeBean, Espresso, Filter, Coarse }
public enum DeliveryMethod { Standard, Express, Pickup }
public enum BrewStrength { Mild, Standard, Bold }

/// <summary>Display text for enums, kept out of the markup.</summary>
public static class Labels
{
    public static string On(bool b) => b ? "true" : "false"; // for aria-pressed / aria-expanded

    public static string Grind(GrindSize g) => g switch
    {
        GrindSize.WholeBean => "Whole bean",
        GrindSize.Espresso => "Espresso (fine)",
        GrindSize.Filter => "Filter (medium)",
        _ => "Coarse (press)"
    };

    public static string Delivery(DeliveryMethod d) => d switch
    {
        DeliveryMethod.Standard => "Standard",
        DeliveryMethod.Express => "Express",
        _ => "Pickup"
    };

    public static string DeliveryNote(DeliveryMethod d) => d switch
    {
        DeliveryMethod.Standard => "3 to 5 days. Free over $60.",
        DeliveryMethod.Express => "Next business day, tracked.",
        _ => "Collect at the roastery counter, Tue to Sat."
    };

    public static string Eta(DeliveryMethod d) => d switch
    {
        DeliveryMethod.Standard => "3 to 5 days",
        DeliveryMethod.Express => "next business day",
        _ => "ready for collection on Tuesday"
    };
}