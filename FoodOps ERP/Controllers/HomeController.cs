using Microsoft.AspNetCore.Mvc;
using FoodOpsERP.Models.ViewModels;

namespace FoodOpsERP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = BuildDashboardViewModel();
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var requestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            _logger.LogError("Unhandled error occurred. RequestId: {RequestId}", requestId);
            ViewData["RequestId"] = requestId;
            return View();
        }

        private static DashboardViewModel BuildDashboardViewModel()
        {
            return new DashboardViewModel
            {
                Production = new ProductionKpi
                {
                    AchievementPercent = 78,
                    Actual = 7800,
                    Target = 10000
                },
                Inventory = new InventoryKpi { LowStockCount = 6 },
                Quality = new QualityKpi { QcHoldCount = 3 },
                Attendance = new AttendanceKpi
                {
                    PresentPercent = 92,
                    Present = 248,
                    Total = 268
                },
                AttentionRequired = new List<AlertItem>
                {
                    new()
                    {
                        Severity = "Critical",
                        Title = "Raw material shortage",
                        Description = "Tomato Paste (Lot 2025-001) — 120 kg short",
                        TimeAgo = "2h ago",
                        ActionText = "Open Shortage Analysis",
                        ActionUrl = "#"
                    },
                    new()
                    {
                        Severity = "Warning",
                        Title = "Batch B-105 awaiting QC decision",
                        Description = "Tomato Sauce — Inspection results pending",
                        TimeAgo = "4h ago",
                        ActionText = "Review Batch",
                        ActionUrl = "#"
                    },
                    new()
                    {
                        Severity = "Info",
                        Title = "Missing check-outs & overtime",
                        Description = "12 employees + 3 pending overtime approvals",
                        TimeAgo = "5h ago",
                        ActionText = "Review Attendance",
                        ActionUrl = "#"
                    }
                },
                ActiveBatches = new List<BatchItem>
                {
                    new() { BatchNumber = "B-105", ProductName = "Tomato Sauce", ProductIcon = "bi-cup-hot", Stage = "Mixing", Progress = 68, Status = "In Progress", StatusBadgeClass = "bg-success" },
                    new() { BatchNumber = "B-106", ProductName = "Vegetable Soup", ProductIcon = "bi-cup-straw", Stage = "QC Inspection", Progress = 92, Status = "QC Hold", StatusBadgeClass = "bg-danger" },
                    new() { BatchNumber = "B-107", ProductName = "Chili Paste", ProductIcon = "bi-fire", Stage = "Packaging", Progress = 35, Status = "In Progress", StatusBadgeClass = "bg-success" },
                    new() { BatchNumber = "B-108", ProductName = "Pickles", ProductIcon = "bi-egg", Stage = "Raw Material Issue", Progress = 12, Status = "Planned", StatusBadgeClass = "bg-secondary" }
                },
                ProductionChart = new ChartData
                {
                    Labels = new List<string> { "Tomato Sauce", "Vegetable Soup", "Chili Paste", "Pickles" },
                    Planned = new List<decimal> { 2500, 3000, 2000, 2500 },
                    Actual = new List<decimal> { 1950, 2420, 1380, 2050 }
                }
            };
        }
    }
}