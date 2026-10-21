using FoodOpsERP.Infrastructure;
using FoodOpsERP.Services.AI;
using Microsoft.AspNetCore.Mvc;

namespace FoodOpsERP.Controllers.AI;

public class AIAuditController : AppController
{
    private readonly IAIAuditService _service;

    public AIAuditController(IAIAuditService service) => _service = service;

    // 1. AI Audit Logs List
    public async Task<IActionResult> Index(string? domain = null)
    {
        var list = await _service.GetAllLogsAsync(domain);
        ViewBag.Domain = domain;
        return View(list);
    }

    // 2. AI Evidence List (Logs hi hain, bas alag view se dikhayenge)
    public async Task<IActionResult> Evidence()
    {
        var list = await _service.GetEvidenceLogsAsync();
        return View(list);
    }

    // 3. AI Safety (Placeholder Page)
    public IActionResult Safety()
    {
        return View();
    }
}