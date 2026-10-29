using SlowBurn.Models;

namespace SlowBurn.Common;

/// <summary>
/// Display text mappings for enums and ARIA states, kept decoupled from UI markup.
/// </summary>
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
