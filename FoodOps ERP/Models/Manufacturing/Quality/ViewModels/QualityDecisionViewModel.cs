using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Manufacturing.Quality.ViewModels;

public class QualityDecisionViewModel
{
    public int DecisionID { get; set; }

    [Required(ErrorMessage = "Batch is required")]
    public long BatchID { get; set; }
    public string? BatchNumber { get; set; }

    public int? InspectionID { get; set; }
    public int? NCRID { get; set; }

    [Required]
    public string Decision { get; set; } = "Approve";

    [StringLength(500)]
    public string? Reason { get; set; }

    public int DecidedByUserID { get; set; }
    public string? DecidedByName { get; set; }
    public DateTime DecidedAt { get; set; }

    public int? ApprovedByUserID { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedAt { get; set; }
}