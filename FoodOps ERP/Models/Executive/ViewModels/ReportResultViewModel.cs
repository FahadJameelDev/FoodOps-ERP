namespace FoodOpsERP.Models.Executive.ViewModels;

public class ReportResultViewModel
{
    public string ReportTitle { get; set; } = string.Empty;
    public string ReportType { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Table ke liye column headers
    public List<string> Headers { get; set; } = new();

    // Table ki rows (List of string arrays)
    public List<string[]> Rows { get; set; } = new();
}