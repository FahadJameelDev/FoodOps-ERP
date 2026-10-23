using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // ✅ Yeh add karein

namespace FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

public class EmployeeViewModel
{
    public int EmployeeID { get; set; }

    [Required(ErrorMessage = "Plant is required")]
    public int PlantID { get; set; }
    public string? PlantName { get; set; }

    [Required(ErrorMessage = "Department is required")]
    public int DepartmentID { get; set; }
    public string? DepartmentName { get; set; }

    public int? ShiftID { get; set; }
    public string? ShiftName { get; set; }

    public int? UserID { get; set; }
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Employee Code is required")]
    [StringLength(30)]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "First Name is required")]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [StringLength(15)]
    public string? CNIC { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? Designation { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime JoiningDate { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    public DateTime? LeavingDate { get; set; }

    [Required]
    public string CurrentEmploymentStatus { get; set; } = "Active";

    [StringLength(30)]
    public string? BankAccountNo { get; set; }

    [StringLength(100)]
    public string? BankName { get; set; }

    public bool IsActive { get; set; } = true;

    public List<EmployeeDocumentViewModel> Documents { get; set; } = new();

    // ✅ Dropdowns ke liye nayi properties
    public IEnumerable<SelectListItem>? PlantList { get; set; }
    public IEnumerable<SelectListItem>? DepartmentList { get; set; }
    public IEnumerable<SelectListItem>? ShiftList { get; set; }
}

public class EmployeeDocumentViewModel
{
    public int DocumentID { get; set; }
    public int EmployeeID { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
    public DateTime UploadedAt { get; set; }
}