namespace FoodOpsERP.Infrastructure;

public static class Fmt
{
    public static string Date(object? v) => v switch
    {
        DateTime d => d.ToString("dd MMM yyyy"),
        DateOnly o => o.ToString("dd MMM yyyy"),
        _ => "—"
    };
    public static string Money(object? v) => v is null ? "—" : Convert.ToDecimal(v).ToString("N2");
    public static string Qty(object? v) => v is null ? "0" : Convert.ToDecimal(v).ToString("#,##0.##");

    public static int? DaysLeft(object? v) => v switch
    {
        DateTime d => (d.Date - DateTime.Today).Days,
        DateOnly o => o.DayNumber - DateOnly.FromDateTime(DateTime.Today).DayNumber,
        _ => null
    };
    public static string ExpiryClass(int? d) => d switch { null => "pill--muted", <= 7 => "pill--bad", <= 30 => "pill--warn", _ => "pill--ok" };
    public static string ExpiryText(int? d) => d switch { null => "No expiry", < 0 => $"Expired {-d}d ago", 0 => "Expires today", _ => $"{d}d left" };
}