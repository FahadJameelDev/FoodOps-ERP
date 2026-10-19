using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Manufacturing.Costing.ViewModels;
using FoodOpsERP.Services.Manufacturing.Costing;
using Microsoft.AspNetCore.Mvc;

namespace FoodOpsERP.Controllers.Manufacturing.Costing;

public class BatchCostingsController : AppController
{
    private readonly IBatchCostingService _service;

    public BatchCostingsController(IBatchCostingService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _service.GetAllBatchCostsAsync();
        return View(list);
    }

    public async Task<IActionResult> UnitCost()
    {
        var list = await _service.GetUnitCostsAsync();
        return View(list);
    }

    public async Task<IActionResult> Variance()
    {
        var list = await _service.GetAllVariancesAsync();
        return View(list);
    }

    public async Task<IActionResult> VarianceControl()
    {
        var list = await _service.GetVarianceControlsAsync();
        return View(list);
    }

    public async Task<IActionResult> LossAnalysis()
    {
        var model = await _service.GetLossAnalysisAsync();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ReviewVariance(int id)
    {
        var model = await _service.GetVarianceDetailsAsync(id);
        if (model == null) return NotFound();

        return FormView("_ReviewVarianceForm", "ReviewVariance", model);
    }

    [HttpPost]
    public async Task<IActionResult> ReviewVariance(CostVarianceViewModel model, string status, string comments)
    {
        var result = await _service.ReviewVarianceAsync(model.VarianceID, status, comments, 1);
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            return Invalid("_ReviewVarianceForm", "ReviewVariance", model);
        }
        return Saved(result.Message, "VarianceControl");
    }
}