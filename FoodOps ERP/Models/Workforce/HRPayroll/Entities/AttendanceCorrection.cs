using FoodOpsERP.Models.Entities;
using FoodOpsERP.Models.Entities.Manufacturing;
using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Workforce.HRPayroll.Entities;

public class AttendanceCorrection : VersionedEntity
{
    public int CorrectionID { get; set; }
    public long AttendanceID { get; set; }
    public int RequestedByUserID { get; set; }

    public DateTime? OldCheckIn { get; set; }
    public DateTime? OldCheckOut { get; set; }
    public DateTime? NewCheckIn { get; set; }
    public DateTime? NewCheckOut { get; set; }

    [Required, MaxLength(300)]
    public string Reason { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Pending / Approved / Rejected

    public int? ApprovedByUserID { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public AttendanceLog AttendanceLog { get; set; } = null!;
    public User RequestedByUser { get; set; } = null!;
    public User? ApprovedByUser { get; set; }
}