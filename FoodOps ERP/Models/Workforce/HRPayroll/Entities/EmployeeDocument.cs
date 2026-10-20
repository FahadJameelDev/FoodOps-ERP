using FoodOpsERP.Models.Entities;
using FoodOpsERP.Models.Entities.Manufacturing;
using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Workforce.HRPayroll.Entities;

public class EmployeeDocument : VersionedEntity
{
    public int DocumentID { get; set; }
    public int EmployeeID { get; set; }

    [Required, MaxLength(30)]
    public string DocumentType { get; set; } = string.Empty; // CNIC / Contract / Photo / Other

    [Required, MaxLength(100)]
    public string DocumentName { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string FilePath { get; set; } = string.Empty;

    public DateTime? ExpiryDate { get; set; }
    public int UploadedByUserID { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Employee Employee { get; set; } = null!;
    public User UploadedByUser { get; set; } = null!;
}