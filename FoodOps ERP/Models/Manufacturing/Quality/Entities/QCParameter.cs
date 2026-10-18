using FoodOpsERP.Models.Entities; // Group 2 aur Master tables ke liye

namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class ParameterTypes
{
    public const string Numeric = "Numeric";
    public const string PassFail = "PassFail";
    public const string Text = "Text";
    public static readonly string[] All = { Numeric, PassFail, Text };
}

public class QCParameter : AuditedEntity
{
    public int ParameterID { get; set; }

    public string ParameterCode { get; set; } = string.Empty;

    public string ParameterName { get; set; } = string.Empty;

    public string ParameterType { get; set; } = ParameterTypes.Numeric;

    public int? DefaultUOMID { get; set; }

    public bool IsActive { get; set; } = true;
}