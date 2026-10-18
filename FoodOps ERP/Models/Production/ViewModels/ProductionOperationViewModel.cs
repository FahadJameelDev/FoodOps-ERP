using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Manufacturing.Production.ViewModels;

public class ProductionOperationViewModel
{
    public int OperationID { get; set; }
    public int ProductionOrderID { get; set; }
    public int? RoutingID { get; set; }

    [Required]
    public int WorkCenterID { get; set; }
    public string? WorkCenterName { get; set; }

    public int SequenceOrder { get; set; }

    [Required]
    public string OperationName { get; set; } = string.Empty;

    public decimal StandardMinutes { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? ActualMinutes { get; set; }
    public int? OperatorUserID { get; set; }
    public string? OperatorName { get; set; }
}