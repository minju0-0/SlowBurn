using System.Globalization;

namespace SlowBurn.Common;

/// <summary>
/// Currency and price formatting utilities.
/// </summary>
public static class Money
{
    public static string Fmt(decimal v) => "$" + v.ToString("N2", CultureInfo.InvariantCulture);
}
