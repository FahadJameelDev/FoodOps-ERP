using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class RecipeChangeType
{
    public const string Ingredient = "Ingredient";
    public const string Quantity = "Quantity";
    public const string Process = "Process";
    public const string Supplier = "Supplier";
    public const string Regulatory = "Regulatory";
    public const string Other = "Other";
    public static readonly string[] All = { Ingredient, Quantity, Process, Supplier, Regulatory, Other };
}

public static class ChangeRequestStatus
{
    public const string Requested = "Requested";
    public const string UnderReview = "UnderReview";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
    public const string Implemented = "Implemented";
    public const string Cancelled = "Cancelled";
    public static readonly string[] All = { Requested, UnderReview, Approved, Rejected, Implemented, Cancelled };
}

public class RecipeChangeRequest : AuditedEntity
{
    public int ChangeID { get; set; }

    [Required, MaxLength(30)]
    public string ChangeNo { get; set; } = string.Empty;

    public int RecipeID { get; set; }
    public int? NewRecipeID { get; set; }

    [Required, MaxLength(30)]
    public string ChangeType { get; set; } = RecipeChangeType.Other;

    [Required, MaxLength(500)]
    public string Reason { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? ImpactAssessment { get; set; }

    [Required, MaxLength(20)]
    public string Status { get; set; } = ChangeRequestStatus.Requested;

    public int? ApprovedByUserID { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? ImplementedAt { get; set; }

    // Navigation
    public Recipe Recipe { get; set; } = null!;
    public Recipe? NewRecipe { get; set; }
}