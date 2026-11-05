using FoodOpsERP.Data;
using FoodOpsERP.Models.AI.Entities;
using Google.GenAI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOpsERP.Services.AI
{
    public class AIAssistantService : IAIAssistantService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public AIAssistantService(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<string> AskAssistantAsync(string domainContext, string question, int userId)
        {
            string? apiKey = _configuration["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_OPENAI_API_KEY_HERE")
                return "Gemini API Key is not configured in appsettings.json.";

            try
            {
                // ✅ 1. REAL DATA NIKALEIN (Aapke Database se)
                StringBuilder realData = new StringBuilder();
                var today = DateTime.UtcNow.Date;
                var yesterday = today.AddDays(-1);

                realData.AppendLine("REAL DATABASE DATA (A to Z):");

                // ── 1. INVENTORY ──
                if (domainContext == "Inventory")
                {
                    var totalProducts = await _db.Products.CountAsync();
                    var totalStock = await _db.InventoryBatches.SumAsync(b => b.CurrentQuantity);
                    var todayReceived = await _db.InventoryTransactions.Where(t => t.Timestamp.Date == today && t.TransactionType == "Receipt").SumAsync(t => t.Quantity);
                    var yesterdayReceived = await _db.InventoryTransactions.Where(t => t.Timestamp.Date == yesterday && t.TransactionType == "Receipt").SumAsync(t => t.Quantity);

                    // ✅ Expiring Batches
                    var expiringBatches = await _db.InventoryBatches
                        .Include(b => b.Product)
                        .Where(b => b.ExpiryDate.HasValue && b.ExpiryDate.Value.Date <= DateTime.UtcNow.AddDays(30) && b.CurrentQuantity > 0)
                        .Select(b => new { b.BatchNumber, Product = b.Product.ProductName, Expiry = b.ExpiryDate.Value.ToString("dd-MMM-yyyy") })
                        .Take(5).ToListAsync();

                    realData.AppendLine($"- Total Products: {totalProducts}");
                    realData.AppendLine($"- Total Stock Quantity: {totalStock} units");
                    realData.AppendLine($"- Stock Received Today: {todayReceived} units");
                    realData.AppendLine($"- Stock Received Yesterday: {yesterdayReceived} units");

                    if (expiringBatches.Any())
                    {
                        realData.AppendLine("- Batches Expiring Soon (Next 30 Days):");
                        foreach (var item in expiringBatches)
                            realData.AppendLine($"  * {item.Product} (Batch: {item.BatchNumber}) - Expiry: {item.Expiry}");
                    }

                    var lowStockDetails = await _db.InventoryBatches
                        .Include(b => b.Product)
                        .Where(b => b.CurrentQuantity < 100)
                        .Select(b => new { b.BatchNumber, Product = b.Product.ProductName, b.CurrentQuantity })
                        .Take(5).ToListAsync();

                    if (lowStockDetails.Any())
                    {
                        realData.AppendLine("- Low Stock Items:");
                        foreach (var item in lowStockDetails)
                            realData.AppendLine($"  * {item.Product} (Batch: {item.BatchNumber}) - Only {item.CurrentQuantity} left.");
                    }
                }

                // ── 2. PRODUCTION ──
                else if (domainContext == "Production")
                {
                    var activeBatchesCount = await _db.ProductionOrders.CountAsync(p => p.Status == "InProgress");
                    var todayProduced = await _db.BatchOutputLogs.Where(l => l.RecordedAt.Date == today).SumAsync(l => l.ProducedQty);
                    var yesterdayProduced = await _db.BatchOutputLogs.Where(l => l.RecordedAt.Date == yesterday).SumAsync(l => l.ProducedQty);
                    var delayedPOs = await _db.PurchaseOrders.Where(p => p.ExpectedDate < today && p.Status == "Pending").Select(p => p.POCode).Take(3).ToListAsync();

                    realData.AppendLine($"- Active Production Batches: {activeBatchesCount}");
                    realData.AppendLine($"- Produced Today: {todayProduced} units");
                    realData.AppendLine($"- Produced Yesterday: {yesterdayProduced} units");

                    if (delayedPOs.Any())
                        realData.AppendLine($"- Delayed Purchase Orders: {string.Join(", ", delayedPOs)}");

                    // ✅ Active Batches Detail
                    // ✅ FIX: 'Include' aur 'ProductName' hata diya taake error khatam ho
                    var batchDetails = await _db.ProductionOrders
                        .Where(p => p.Status == "InProgress")
                        .Select(p => new { p.OrderNumber, p.TargetQty, p.ProducedQty })
                        .Take(5).ToListAsync();
                    if (batchDetails.Any())
                    {
                        realData.AppendLine("- Active Batches Detail:");
                        foreach (var b in batchDetails)
                            realData.AppendLine($"  * {b.OrderNumber} | Target: {b.TargetQty} | Produced: {b.ProducedQty}");
                    }
                }

                // ── 3. QUALITY ──
                else if (domainContext == "Quality")
                {
                    var qcHold = await _db.InventoryBatches.CountAsync(b => b.QCStatus == "Hold" || b.QCStatus == "Quarantine");
                    var todayInspections = await _db.QCInspections.CountAsync(q => q.InspectionDate.Date == today);
                    var yesterdayInspections = await _db.QCInspections.CountAsync(q => q.InspectionDate.Date == yesterday);
                    var pendingNCR = await _db.NonConformanceReports.CountAsync(n => n.Status == "Open");

                    realData.AppendLine($"- Batches on QC Hold: {qcHold}");
                    realData.AppendLine($"- Inspections Today: {todayInspections}");
                    realData.AppendLine($"- Inspections Yesterday: {yesterdayInspections}");
                    realData.AppendLine($"- Pending Non-Conformance Reports (NCR): {pendingNCR}");

                    // ✅ Detail of QC Hold Batches
                    var holdBatches = await _db.InventoryBatches
                        .Include(b => b.Product)
                        .Where(b => b.QCStatus == "Hold" || b.QCStatus == "Quarantine")
                        .Select(b => new { b.BatchNumber, Product = b.Product.ProductName, b.QCStatus })
                        .Take(5).ToListAsync();

                    if (holdBatches.Any())
                    {
                        realData.AppendLine("- Details of Batches on Hold:");
                        foreach (var h in holdBatches)
                            realData.AppendLine($"  * {h.BatchNumber} | Product: {h.Product} | Status: {h.QCStatus}");
                    }
                }

                // ── 4. COST ──
                else if (domainContext == "Cost")
                {
                    var totalBatchCost = await _db.BatchCostSummaries.SumAsync(c => c.TotalBatchCost);
                    var totalVariance = await _db.CostVariances.SumAsync(v => v.TotalVarianceAmount);
                    var highVarianceCount = await _db.CostVariances.CountAsync(v => v.Severity == "High" || v.Severity == "Critical");

                    realData.AppendLine($"- Total Batch Cost: {totalBatchCost:C}");
                    realData.AppendLine($"- Total Cost Variance: {totalVariance:C}");
                    realData.AppendLine($"- High/Critical Variances: {highVarianceCount}");

                    // ✅ Detail of High Variances
                    var variances = await _db.CostVariances
                        .Where(v => v.Severity == "High" || v.Severity == "Critical")
                        .Select(v => new { v.ProductionOrderID, v.Severity, v.TotalVarianceAmount })
                        .Take(5).ToListAsync();

                    if (variances.Any())
                    {
                        realData.AppendLine("- Details of High Variances:");
                        foreach (var v in variances)
                            realData.AppendLine($"  * PO ID: {v.ProductionOrderID} | Severity: {v.Severity} | Amount: {v.TotalVarianceAmount:C}");
                    }
                }

                // ── 5. PAYROLL ──
                else if (domainContext == "Payroll")
                {
                    var activeEmployees = await _db.Employees.CountAsync(e => e.EmploymentStatus == "Active");
                    var todayAttendance = await _db.AttendanceLogs.CountAsync(a => a.LogDate.Date == today);
                    var yesterdayAttendance = await _db.AttendanceLogs.CountAsync(a => a.LogDate.Date == yesterday);
                    var pendingOT = await _db.OvertimeRecords.CountAsync(o => o.Status == "Pending");

                    realData.AppendLine($"- Total Active Employees: {activeEmployees}");
                    realData.AppendLine($"- Attendance Logs Today: {todayAttendance}");
                    realData.AppendLine($"- Attendance Logs Yesterday: {yesterdayAttendance}");
                    realData.AppendLine($"- Pending Overtime Requests: {pendingOT}");

                    // ✅ Detail of Pending Overtime
                    var otRequests = await _db.OvertimeRecords
                        .Include(o => o.Employee)
                        .Where(o => o.Status == "Pending")
                        .Select(o => new { Employee = o.Employee.FirstName + " " + o.Employee.LastName, o.OTHours, o.OTDate })
                        .Take(5).ToListAsync();

                    if (otRequests.Any())
                    {
                        realData.AppendLine("- Details of Pending Overtime Requests:");
                        foreach (var ot in otRequests)
                            realData.AppendLine($"  * {ot.Employee} | Date: {ot.OTDate:dd-MMM-yyyy} | Hours: {ot.OTHours}");
                    }
                }

                // ── 6. MANAGEMENT ──
                else if (domainContext == "Management")
                {
                    var totalPlants = await _db.Plants.CountAsync();
                    var totalUsers = await _db.Users.CountAsync();
                    var totalProdOrders = await _db.ProductionOrders.CountAsync();
                    var totalPOs = await _db.PurchaseOrders.CountAsync();

                    realData.AppendLine($"- Total Plants: {totalPlants}");
                    realData.AppendLine($"- Total System Users: {totalUsers}");
                    realData.AppendLine($"- Total Production Orders: {totalProdOrders}");
                    realData.AppendLine($"- Total Purchase Orders: {totalPOs}");
                }

                // ✅ STRICT AI PROMPT (No robotic replies)
                string systemPrompt = $@"You are a professional AI assistant for FoodOpsERP. 
Your job is to answer the user's question using ONLY the provided REAL DATABASE DATA. 
- Be direct and professional. 
- Do NOT write phrases like 'Based on the provided database data' or 'According to the data'. 
- If the user asks for details, provide the list from the data. 
- If the data is not available, say 'I don't have that specific data right now.'

{realData}

User Question: {question}";

                var client = new Client(apiKey: apiKey);
                int maxRetries = 3;

                for (int i = 0; i < maxRetries; i++)
                {
                    try
                    {
                        var response = await client.Models.GenerateContentAsync(
                            model: "gemini-3.6-flash",
                            contents: systemPrompt
                        );

                        string answer = response?.Candidates?[0]?.Content?.Parts?[0]?.Text?.Trim() ?? "No response.";

                        // Log Save
                        int validUserId = userId;
                        if (!await _db.Users.AnyAsync(u => u.UserID == validUserId))
                            validUserId = await _db.Users.Select(u => u.UserID).FirstOrDefaultAsync();

                        _db.AIAssistantLogs.Add(new AIAssistantLog
                        {
                            UserID = validUserId == 0 ? 1 : validUserId,
                            DomainContext = domainContext,
                            QuestionSummary = question.Length > 500 ? question.Substring(0, 500) : question,
                            AnswerSummary = answer.Length > 1000 ? answer.Substring(0, 1000) : answer,
                            ModelName = "Google-Gemini-3.6-Flash",
                            IsSuccess = true,
                            CreatedAt = DateTime.UtcNow
                        });
                        await _db.SaveChangesAsync();

                        return answer;
                    }
                    catch (Exception ex)
                    {
                        if (i == maxRetries - 1) return $"System Error: {ex.Message}";
                        await Task.Delay(2000);
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Database Error: {ex.Message}";
            }

            return "An unexpected error occurred.";
        }
    }
}