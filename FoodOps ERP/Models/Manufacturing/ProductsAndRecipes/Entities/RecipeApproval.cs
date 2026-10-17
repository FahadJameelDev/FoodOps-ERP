using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class ApprovalDecision
{
    public const string Pending = "Pending";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
    public static readonly string[] All = { Pending, Approved, Rejected };
}

public class RecipeApproval : VersionedEntity
{
    public int ApprovalID { get; set; }
    public int RecipeID { get; set; }
    public int StepNo { get; set; } = 1;
    public int RequestedByUserID { get; set; }
    public int ApproverUserID { get; set; }

    [Required, MaxLength(15)]
    public string Decision { get; set; } = ApprovalDecision.Pending;

    public DateTime? DecisionAt { get; set; }

    [MaxLength(500)]
    public string? Comments { get; set; }

    // FIX: Database khud time dalega
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Recipe Recipe { get; set; } = null!;
}