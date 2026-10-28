using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.AI.ViewModels;
using FoodOpsERP.Services.AI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // <-- YEH ADD KAREIN

namespace FoodOpsERP.Controllers.AI;

[Authorize(Roles = "Administrator, AI Assistant")] // <-- YEH ADD KAREIN


public class AIAssistantController : AppController
{
    private readonly IAIAssistantService _service;

    public AIAssistantController(IAIAssistantService service) => _service = service;

    // 1. Main AI Hub (6 Assistants ke links)
    public IActionResult Index()
    {
        return View();
    }

    // 2. Inventory Assistant
    public IActionResult Inventory()
    {
        return View(new AIChatViewModel { DomainContext = "Inventory" });
    }

    // 3. Production Assistant
    public IActionResult Production()
    {
        return View(new AIChatViewModel { DomainContext = "Production" });
    }

    // 4. Quality Assistant
    public IActionResult Quality()
    {
        return View(new AIChatViewModel { DomainContext = "Quality" });
    }

    // 5. Cost Assistant
    public IActionResult Cost()
    {
        return View(new AIChatViewModel { DomainContext = "Cost" });
    }

    // 6. Payroll Assistant
    public IActionResult Payroll()
    {
        return View(new AIChatViewModel { DomainContext = "Payroll" });
    }

    // 7. Management Assistant
    public IActionResult Management()
    {
        return View(new AIChatViewModel { DomainContext = "Management" });
    }

    // 8. Ask Question (AJAX Call)
    [HttpPost]
    public async Task<IActionResult> Ask(string domainContext, string question)
    {
        if (string.IsNullOrWhiteSpace(question))
            return Json(new { success = false, message = "Question is required." });

        var answer = await _service.AskAssistantAsync(domainContext, question, GetCurrentUserId());
        return Json(new { success = true, answer });
    }
}