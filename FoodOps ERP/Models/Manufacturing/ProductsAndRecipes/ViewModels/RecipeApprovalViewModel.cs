using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;

public class RecipeApprovalViewModel
{
    public int ApprovalID { get; set; }

    [Required]
    public int RecipeID { get; set; }

    // Display only (taake UI par recipe ka naam dikh sake)
    public string? RecipeCode { get; set; }
    public string? RecipeName { get; set; }

    public int StepNo { get; set; } = 1;

    public int RequestedByUserID { get; set; }

    // Display only
    public string? RequestedByName { get; set; }

    [Required]
    public int ApproverUserID { get; set; }

    // Display only
    public string? ApproverName { get; set; }

    [Required]
    [Display(Name = "Decision")]
    public string Decision { get; set; } = "Pending";

    [Display(Name = "Decision Date")]
    public DateTime? DecisionAt { get; set; }

    [StringLength(500)]
    [Display(Name = "Comments")]
    public string? Comments { get; set; }

    [Display(Name = "Requested At")]
    public DateTime CreatedAt { get; set; }
}