using Microsoft.AspNetCore.Mvc;
using FoodOpsERP.Models.ViewModels;
using FoodOpsERP.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FoodOpsERP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = await BuildDashboardViewModelAsync();
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            _logger.LogError("Unhandled error occurred. RequestId: {RequestId}", requestId);
            ViewData["RequestId"] = requestId;
            return View();
        }

        private async Task<DashboardViewModel> BuildDashboardViewModelAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            // 1. Production KPIs
            var todayOrders = await _db.ProductionOrders
                .Where(o => o.CreatedAt >= today && o.CreatedAt < tomorrow)
                .ToListAsync();

            decimal actualProd = todayOrders.Sum(o => o.ProducedQty);
            decimal targetProd = todayOrders.Sum(o => o.TargetQty);
            decimal achievement = targetProd > 0 ? (actualProd / targetProd) * 100 : 0;

            // 2. Inventory KPIs
            var lowStock = await _db.InventoryBatches.CountAsync(b => b.CurrentQuantity > 0 && b.CurrentQuantity < 10);

            // 3. Quality KPIs
            var qcHold = await _db.InventoryBatches.CountAsync(b => b.QCStatus == "Hold" || b.QCStatus == "Quarantine");

            // 4. Attendance KPIs
            var totalEmp = await _db.Employees.CountAsync(e => e.IsActive);
            var presentToday = await _db.AttendanceLogs
                .CountAsync(a => a.LogDate >= today && a.LogDate < tomorrow && (a.Status == "Present" || a.Status == "Late"));
            decimal attendancePct = totalEmp > 0 ? ((decimal)presentToday / totalEmp) * 100 : 0;

            // 5. Active Batches (✅ FIX: Id add kiya)
            var activeBatches = await _db.ProductionOrders
                .Where(o => o.Status == "InProgress" || o.Status == "Released" || o.Status == "Planned")
                .OrderByDescending(o => o.CreatedAt)
                .Take(5)
                .Select(o => new BatchItem
                {
                    Id = o.ProductionOrderID, // ✅ ID Add Kiya
                    BatchNumber = o.OrderNumber,
                    ProductName = _db.Products.Where(p => p.ProductID == o.ProductID).Select(p => p.ProductName).FirstOrDefault() ?? "Unknown",
                    ProductIcon = "bi-box",
                    Stage = o.Status == "Planned" ? "Planning" : "Production",
                    Progress = o.TargetQty > 0 ? (int)((o.ProducedQty / o.TargetQty) * 100) : 0,
                    Status = o.Status,
                    StatusBadgeClass = o.Status == "InProgress" ? "bg-success" : "bg-secondary"
                })
                .ToListAsync();

            // 6. Production Chart
            var chartData = await _db.ProductionOrders
                .GroupBy(o => o.ProductID)
                .Select(g => new
                {
                    ProductName = _db.Products.Where(p => p.ProductID == g.Key).Select(p => p.ProductName).FirstOrDefault() ?? "Item",
                    Planned = g.Sum(o => o.TargetQty),
                    Actual = g.Sum(o => o.ProducedQty)
                })
                .Take(5)
                .ToListAsync();

            var chart = new ChartData
            {
                Labels = chartData.Select(c => c.ProductName).ToList(),
                Planned = chartData.Select(c => c.Planned).ToList(),
                Actual = chartData.Select(c => c.Actual).ToList()
            };

            // 7. Alerts
            var alerts = new List<AlertItem>();
            if (lowStock > 0)
                alerts.Add(new AlertItem { Severity = "Warning", Title = "Low Stock", Description = $"{lowStock} items below reorder level", TimeAgo = "Just now", ActionText = "View Stock", ActionUrl = "/Stock/Balance" });
            if (qcHold > 0)
                alerts.Add(new AlertItem { Severity = "Critical", Title = "QC Hold", Description = $"{qcHold} batches on hold", TimeAgo = "Just now", ActionText = "Review", ActionUrl = "/QualityDecisions" });

            return new DashboardViewModel
            {
                Production = new ProductionKpi { AchievementPercent = achievement, Actual = actualProd, Target = targetProd },
                Inventory = new InventoryKpi { LowStockCount = lowStock },
                Quality = new QualityKpi { QcHoldCount = qcHold },
                Attendance = new AttendanceKpi { PresentPercent = attendancePct, Present = presentToday, Total = totalEmp },
                AttentionRequired = alerts,
                ActiveBatches = activeBatches,
                ProductionChart = chart
            };
        }
    }
}