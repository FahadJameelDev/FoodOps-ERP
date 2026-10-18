using FoodOpsERP.Models.Entities; // Group 2 aur Master tables ke liye

namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class QualityDecisionTypes
{
    public const string Approve = "Approve";
    public const string Reject = "Reject";
    public const string Hold = "Hold";
    public const string Rework = "Rework";
    public const string ConditionalRelease = "ConditionalRelease";
    public static readonly string[] All = { Approve, Reject, Hold, Rework, ConditionalRelease };
}

public class QualityDecision
{
    public int DecisionID { get; set; }

    public long BatchID { get; set; }

    public int? InspectionID { get; set; }

    public int? NCRID { get; set; }

    public string Decision { get; set; } = QualityDecisionTypes.Approve;

    public string? Reason { get; set; }

    public int DecidedByUserID { get; set; }

    public DateTime DecidedAt { get; set; }

    public int? ApprovedByUserID { get; set; }

    public DateTime? ApprovedAt { get; set; }
}