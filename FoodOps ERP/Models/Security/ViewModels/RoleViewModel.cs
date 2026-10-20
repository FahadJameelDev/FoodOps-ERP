using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Security.ViewModels;

public class RoleViewModel
{
    public int RoleID { get; set; }

    [Required]
    [StringLength(30)]
    [Display(Name = "Role Code")]
    public string RoleCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Display(Name = "Role Name")]
    public string RoleName { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Description { get; set; }

    public bool IsSystem { get; set; }
    public bool IsActive { get; set; } = true;
}