using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodOpsERP.Models.Executive.ViewModels;

public class ReportFilterViewModel
{
    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-30);

    [Required]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; } = DateTime.Today;

    public int? PlantID { get; set; }
    public string ReportType { get; set; } = "Production";

    // ✅ Dropdown ke liye nayi property
    public IEnumerable<SelectListItem>? PlantList { get; set; }
}