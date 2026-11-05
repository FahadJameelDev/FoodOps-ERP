using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOpsERP.Controllers
{
    public class SearchController : AppController
    {
        private readonly ApplicationDbContext _db;

        public SearchController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string q)
        {
            // 1. Agar query khali hai
            if (string.IsNullOrWhiteSpace(q))
            {
                ViewBag.Query = "";
                return View(new List<dynamic>());
            }

            ViewBag.Query = q;
            var term = q.ToLower().Trim();
            var results = new List<dynamic>();

            try
            {
                // ─── 2. DATABASE SEARCH (Products, Suppliers, etc.) ───
                // Products
                var products = await _db.Products
                    .Where(p => p.ProductName.ToLower().Contains(term) || p.ProductCode.ToLower().Contains(term))
                    .Select(p => new { Name = p.ProductName, Type = "Product", Url = "/Products/Details/" + p.ProductID, Desc = "Code: " + p.ProductCode })
                    .Take(5).ToListAsync();
                results.AddRange(products);

                // Suppliers
                var suppliers = await _db.Suppliers
                    .Where(s => s.SupplierName.ToLower().Contains(term) || s.SupplierCode.ToLower().Contains(term))
                    .Select(s => new { Name = s.SupplierName, Type = "Supplier", Url = "/Suppliers/Details/" + s.SupplierID, Desc = "Code: " + s.SupplierCode })
                    .Take(5).ToListAsync();
                results.AddRange(suppliers);

                // Employees
                var employees = await _db.Employees
                    .Where(e => e.FirstName.ToLower().Contains(term) || e.LastName.ToLower().Contains(term) || e.EmployeeCode.ToLower().Contains(term))
                    .Select(e => new { Name = e.FirstName + " " + e.LastName, Type = "Employee", Url = "/Employees/Details/" + e.EmployeeID, Desc = "Code: " + e.EmployeeCode })
                    .Take(5).ToListAsync();
                results.AddRange(employees);

                // Purchase Orders
                var purchaseOrders = await _db.PurchaseOrders
                    .Where(p => p.POCode.ToLower().Contains(term))
                    .Select(p => new { Name = p.POCode, Type = "Purchase Order", Url = "/PurchaseOrders/Details/" + p.POID, Desc = "Status: " + p.Status })
                    .Take(5).ToListAsync();
                results.AddRange(purchaseOrders);

                // ─── 3. STATIC MENU SEARCH (The Real Fix) ───
                var menuItems = new List<(string Name, string Url, string Type, string Desc)>
                {
                    ("Dashboard", "/Home/Index", "Module", "Main Dashboard"),
                    ("Factory Overview", "/Dashboard/Index", "Feature", "Executive Dashboard"),
                    ("Production KPIs", "/Dashboard/ProductionKpis", "Feature", "Executive Dashboard"),
                    ("Inventory KPIs", "/Dashboard/InventoryKpis", "Feature", "Executive Dashboard"),
                    ("Quality KPIs", "/Dashboard/QualityKpis", "Feature", "Executive Dashboard"),
                    ("Workforce KPIs", "/Dashboard/WorkforceKpis", "Feature", "Executive Dashboard"),
                    ("Supplier Management", "/Suppliers/Index", "Feature", "Supply Chain"),
                    ("Purchase Management", "/PurchaseOrders/Index", "Feature", "Supply Chain"),
                    ("Goods Receiving (GRN)", "/GoodsReceiving/Index", "Feature", "Supply Chain"),
                    ("Incoming QC", "/IncomingQC/Index", "Feature", "Supply Chain"),
                    ("Stock Management", "/Stock/Balance", "Feature", "Inventory"),
                    ("Stock Movements", "/Stock/Movements", "Feature", "Inventory"),
                    ("Lot & Expiry Tracking", "/Lots/Index", "Feature", "Inventory"),
                    ("Material Issue & Return", "/MaterialIssue/Index", "Feature", "Inventory"),
                    ("Inventory Wastage", "/Wastage/Index", "Feature", "Inventory"),
                    ("Product Catalogue", "/Products/Index", "Feature", "Products & Recipes"),
                    ("Packaging Configuration", "/PackagingConfigs/Index", "Feature", "Products & Recipes"),
                    ("Recipe Management", "/Recipes/Index", "Feature", "Products & Recipes"),
                    ("Recipe Approval", "/Recipes/PendingApprovals", "Feature", "Products & Recipes"),
                    ("Recipe Change Control", "/RecipeChangeRequests/Index", "Feature", "Products & Recipes"),
                    ("Demand Management", "/Demands/Index", "Feature", "Production"),
                    ("Production Planning", "/ProductionPlans/Index", "Feature", "Production"),
                    ("Batch Management", "/ProductionOrders/Index", "Feature", "Production"),
                    ("QC Setup", "/QCParameters/Index", "Feature", "Quality Control"),
                    ("Production QC", "/QualityDecisions/ProductionQC", "Feature", "Quality Control"),
                    ("Quality Decision", "/QualityDecisions/QualityDecisions", "Feature", "Quality Control"),
                    ("Release Control", "/QualityDecisions/ReleaseControl", "Feature", "Quality Control"),
                    ("Batch Costing", "/BatchCostings/Index", "Feature", "Costing"),
                    ("Cost Components", "/CostComponents/Index", "Feature", "Costing"),
                    ("Employee Management", "/Employees/Index", "Feature", "Workforce"),
                    ("Workforce Setup", "/WorkforceSetup/Index", "Feature", "Workforce"),
                    ("Compensation Setup", "/Compensation/Index", "Feature", "Workforce"),
                    ("Leave Management", "/Leave/Index", "Feature", "Workforce"),
                    ("Attendance", "/Attendance/Index", "Feature", "Workforce"),
                    ("Overtime", "/Overtime/Index", "Feature", "Workforce"),
                    ("Payroll Processing", "/Payroll/Index", "Feature", "Workforce"),
                    ("Payroll Review", "/Payroll/Review", "Feature", "Workforce"),
                    ("Payroll Finalization", "/Payroll/Finalize", "Feature", "Workforce"),
                    ("AI Assistant Hub", "/AIAssistant/Index", "Module", "AI Engine"),
                    ("Inventory Assistant", "/AIAssistant/Inventory", "Feature", "AI Engine"),
                    ("Production Assistant", "/AIAssistant/Production", "Feature", "AI Engine"),
                    ("Quality Assistant", "/AIAssistant/Quality", "Feature", "AI Engine"),
                    ("Cost Assistant", "/AIAssistant/Cost", "Feature", "AI Engine"),
                    ("Payroll Assistant", "/AIAssistant/Payroll", "Feature", "AI Engine"),
                    ("Management Assistant", "/AIAssistant/Management", "Feature", "AI Engine"),
                    ("AI Audit", "/AIAudit/Index", "Feature", "AI Engine"),
                };

                // ✅ SMART WORD MATCHING
                var matchedMenu = menuItems
                    .Where(m => IsWordMatch(term, m.Name) || IsWordMatch(term, m.Desc))
                    .Select(m => new { Name = m.Name, Type = m.Type, Url = m.Url, Desc = m.Desc })
                    .Take(10)
                    .ToList();

                results.AddRange(matchedMenu);

                // ✅ SUGGESTION (Agar result khali hai)
                if (results.Count == 0)
                {
                    var suggestion = GetSuggestion(term, menuItems.Select(m => m.Name).ToList());
                    ViewBag.Suggestion = suggestion;
                }
            }
            catch (Exception ex)
            {
                // Error aaye toh crash na ho
                ViewBag.Error = "Search failed: " + ex.Message;
            }

            return View(results);
        }

        // ✅ Word Matching Logic (Google Style)
        private bool IsWordMatch(string searchTerm, string target)
        {
            var s = searchTerm.ToLower().Trim();
            var t = target.ToLower().Trim();

            // 1. Direct Match
            if (t.Contains(s)) return true;

            // 2. Reverse Match
            if (s.Contains(t)) return true;

            // 3. Word by Word Match
            var searchWords = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var targetWords = t.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var sw in searchWords)
            {
                bool found = false;
                foreach (var tw in targetWords)
                {
                    // Direct word match
                    if (tw == sw || tw.StartsWith(sw) || sw.StartsWith(tw))
                    {
                        found = true;
                        break;
                    }
                    // Fuzzy word match (spelling mistake)
                    if (LevenshteinDistance(sw, tw) <= 2)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) return false;
            }
            return true;
        }

        private string? GetSuggestion(string input, List<string> options)
        {
            string? best = null;
            int bestDist = int.MaxValue;

            foreach (var opt in options)
            {
                int dist = LevenshteinDistance(input.ToLower(), opt.ToLower());
                if (dist < bestDist && dist <= 6)
                {
                    bestDist = dist;
                    best = opt;
                }
            }
            return best;
        }

        private int LevenshteinDistance(string s, string t)
        {
            int n = s.Length, m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; d[i, 0] = i++) { }
            for (int j = 0; j <= m; d[0, j] = j++) { }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }
    }
}