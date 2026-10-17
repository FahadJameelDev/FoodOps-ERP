using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class RecipeStatus
{
    public const string Draft = "Draft";
    public const string PendingApproval = "PendingApproval";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
    public const string Obsolete = "Obsolete";
    public static readonly string[] All = { Draft, PendingApproval, Approved, Rejected, Obsolete };
}

public class Recipe : AuditedEntity
{
    public int RecipeID { get; set; }
    public int ProductID { get; set; }
    public int VersionNo { get; set; } = 1;

    [Required, MaxLength(30)]
    public string RecipeCode { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Status { get; set; } = RecipeStatus.Draft;

    [Column(TypeName = "decimal(18,4)")]
    public decimal StandardBatchSize { get; set; }

    public int BatchUOMID { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal YieldPercentage { get; set; }

    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public bool IsCurrent { get; set; }

    public int? ApprovedByUserID { get; set; }
    public DateTime? ApprovedAt { get; set; }

    [MaxLength(500)]
    public string? ChangeSummary { get; set; }

    // Navigation Properties
    public ICollection<RecipeItem> Items { get; set; } = new List<RecipeItem>();
    public ICollection<RecipeApproval> Approvals { get; set; } = new List<RecipeApproval>();
    public ICollection<RecipeChangeRequest> ChangeRequests { get; set; } = new List<RecipeChangeRequest>();
}