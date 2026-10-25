using System.Text.Json;

namespace FoodOpsERP.Models.ViewModels
{
    public class DashboardViewModel
    {
        public ProductionKpi Production { get; set; } = new();
        public InventoryKpi Inventory { get; set; } = new();
        public QualityKpi Quality { get; set; } = new();
        public AttendanceKpi Attendance { get; set; } = new();

        public List<AlertItem> AttentionRequired { get; set; } = new();
        public List<BatchItem> ActiveBatches { get; set; } = new();
        public ChartData ProductionChart { get; set; } = new();
    }

    public class ProductionKpi
    {
        public decimal AchievementPercent { get; set; }
        public decimal Actual { get; set; }
        public decimal Target { get; set; }
    }

    public class InventoryKpi
    {
        public int LowStockCount { get; set; }
    }

    public class QualityKpi
    {
        public int QcHoldCount { get; set; }
    }

    public class AttendanceKpi
    {
        public decimal PresentPercent { get; set; }
        public int Present { get; set; }
        public int Total { get; set; }
    }

    public class AlertItem
    {
        public string Severity { get; set; } = "Info";
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TimeAgo { get; set; } = string.Empty;
        public string ActionText { get; set; } = string.Empty;
        public string ActionUrl { get; set; } = "#";
    }

    public class BatchItem
    {
        // ✅ YEH LINE ADD KAREIN (Yeh missing thi)
        public int Id { get; set; }

        public string BatchNumber { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductIcon { get; set; } = "bi-box";
        public string Stage { get; set; } = string.Empty;
        public int Progress { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusBadgeClass { get; set; } = "bg-secondary";
    }

    public class ChartData
    {
        public List<string> Labels { get; set; } = new();
        public List<decimal> Planned { get; set; } = new();
        public List<decimal> Actual { get; set; } = new();

        // ✅ JSON pre-serialized — @Json.Serialize error se bachne ke liye
        public string LabelsJson => JsonSerializer.Serialize(Labels);
        public string PlannedJson => JsonSerializer.Serialize(Planned);
        public string ActualJson => JsonSerializer.Serialize(Actual);
    }
}