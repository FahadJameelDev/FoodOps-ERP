namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class ProductionExceptionType
{
    public const string MaterialShortage = "MaterialShortage";
    public const string Equipment = "Equipment";
    public const string Quality = "Quality";
    public const string ProcessDeviation = "ProcessDeviation";
    public const string Safety = "Safety";
    public const string Other = "Other";
    public static readonly string[] All = { MaterialShortage, Equipment, Quality, ProcessDeviation, Safety, Other };
}

public static class ExceptionSeverity
{
    public const string Low = "Low";
    public const string Medium = "Medium";
    public const string High = "High";
    public const string Critical = "Critical";
    public static readonly string[] All = { Low, Medium, High, Critical };
}

public static class ExceptionStatus
{
    public const string Open = "Open";
    public const string Investigating = "Investigating";
    public const string Resolved = "Resolved";
    public const string Closed = "Closed";
    public static readonly string[] All = { Open, Investigating, Resolved, Closed };
}

public class ProductionException : AuditedEntity
{
    public int ExceptionID { get; set; }
    public string ExceptionNo { get; set; } = string.Empty;
    public int? ProductionOrderID { get; set; }
    public int? WorkCenterID { get; set; }
    public string ExceptionType { get; set; } = ProductionExceptionType.Other;
    public string Severity { get; set; } = ExceptionSeverity.Low;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = ExceptionStatus.Open;
    public int? AssignedToUserID { get; set; }
    public string? ResolutionNotes { get; set; }
    public int? ResolvedByUserID { get; set; }
    public DateTime? ResolvedAt { get; set; }
}