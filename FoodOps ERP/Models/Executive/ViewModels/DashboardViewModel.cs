namespace FoodOpsERP.Models.Executive.ViewModels;

public class DashboardViewModel
{
    // 1. Factory Overview
    public int TotalEmployees { get; set; }
    public int TotalProducts { get; set; }
    public decimal TodayProductionQty { get; set; }
    public decimal TotalInventoryValue { get; set; }

    // 2. Production KPIs
    public int ActiveProductionOrders { get; set; }
    public decimal TodayProducedQty { get; set; }
    public decimal TodayRejectedQty { get; set; }
    public decimal ProductionEfficiency { get; set; } // Percentage

    // 3. Inventory KPIs
    public int LowStockItemsCount { get; set; }
    public int OutOfStockItemsCount { get; set; }
    public int ExpiringLotsCount { get; set; }

    // 4. Quality KPIs
    public int PendingQCInspections { get; set; }
    public int QCOnHoldBatches { get; set; }
    public decimal QualityPassRate { get; set; } // Percentage

    // 5. Cost & Variance KPIs
    public decimal TotalBatchCostThisMonth { get; set; }
    public decimal TotalVarianceThisMonth { get; set; }

    // 6. Workforce KPIs
    public int TodayPresentEmployees { get; set; }
    public int TodayAbsentEmployees { get; set; }
    public int PendingLeaveRequests { get; set; }
    public decimal AttendancePercentage { get; set; }

    // 7. Critical Alerts (List)
    public List<AlertItem> CriticalAlerts { get; set; } = new();

    // 8. Action Queue (List)
    public List<ActionItem> ActionQueue { get; set; } = new();

    // 9. Trends (Last 7 days Production)
    public List<ChartDataPoint> ProductionTrend { get; set; } = new();

    // 10. AI Summary (Placeholder)
    public string AiSummary { get; set; } = "AI Summary will be available soon.";
}

public class AlertItem
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = "Warning"; // Critical, Warning, Info
    public string Link { get; set; } = string.Empty;
}

public class ActionItem
{
    public string Task { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
}

public class ChartDataPoint
{
    public string Label { get; set; } = string.Empty;
    public decimal Value { get; set; }
}