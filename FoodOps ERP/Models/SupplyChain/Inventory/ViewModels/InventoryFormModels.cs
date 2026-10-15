using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.ViewModels.Inventory;

// ── read rows (lookup service) ──
public class LookupRow { public int Id { get; set; } public string Text { get; set; } = ""; }

public class BatchRow
{
    public long BatchID { get; set; }
    public string BatchNumber { get; set; } = "";
    public string ProductName { get; set; } = "";
    public decimal CurrentQuantity { get; set; }
    public string QCStatus { get; set; } = "";
}

public class StockMovementRow
{
    public long TransactionID { get; set; }
    public string BatchNumber { get; set; } = "";
    public string ProductName { get; set; } = "";
    public string TransactionType { get; set; } = "";
    public decimal Quantity { get; set; }
    public string? Remarks { get; set; }
    public DateTime Timestamp { get; set; }
}

// ── forms ──
public class StockAdjustViewModel : IValidatableObject
{
    public static readonly string[] ReasonOptions = { "Count correction", "Damaged", "Expired write-off", "Found stock", "Other" };

    [Required(ErrorMessage = "Select a batch")] public long? BatchId { get; set; }
    [Required(ErrorMessage = "Enter the quantity change")] public decimal? Qty { get; set; }
    [Required(ErrorMessage = "Select a reason"), StringLength(100)] public string? Reason { get; set; }
    [StringLength(300)] public string? Remarks { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext _)
    {
        if (Qty == 0) yield return new ValidationResult("Quantity change cannot be zero.", new[] { nameof(Qty) });
    }
}

// Material Issue / Return (Kind = "Issue" | "Return")
public class StockMovementViewModel
{
    public string Kind { get; set; } = "Issue";
    [Required(ErrorMessage = "Select a batch")] public long? BatchId { get; set; }
    [Required(ErrorMessage = "Enter the quantity"), Range(0.0001, 1000000000, ErrorMessage = "Quantity must be greater than zero")]
    public decimal? Qty { get; set; }
    [StringLength(300)] public string? Remarks { get; set; }
}

public class WastageViewModel
{
    public static readonly string[] ReasonOptions = { "Expired", "Damaged", "Spillage", "Contamination", "Production loss", "Other" };

    [Required(ErrorMessage = "Select a batch")] public long? BatchId { get; set; }
    [Required(ErrorMessage = "Enter the quantity"), Range(0.0001, 1000000000, ErrorMessage = "Quantity must be greater than zero")]
    public decimal? Qty { get; set; }
    [Required(ErrorMessage = "Select a reason"), StringLength(100)] public string? Reason { get; set; }
    [StringLength(300)] public string? Remarks { get; set; }
}

public class QCDecisionViewModel : IValidatableObject
{
    public long Id { get; set; }                       // BatchID
    [Required(ErrorMessage = "Choose a decision")] public string? Decision { get; set; }   // Pass | Fail | Hold
    [StringLength(500)] public string? Remarks { get; set; }
    public string? BatchNumber { get; set; }
    public string? ProductName { get; set; }
    public decimal Quantity { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext _)
    {
        if (Decision is "Fail" or "Hold" && string.IsNullOrWhiteSpace(Remarks))
            yield return new ValidationResult("Remarks are required when failing or holding a batch.", new[] { nameof(Remarks) });
    }
}