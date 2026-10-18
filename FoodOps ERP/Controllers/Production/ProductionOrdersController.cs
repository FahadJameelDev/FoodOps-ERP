using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Manufacturing.Production.ViewModels;
using FoodOpsERP.Services.Manufacturing.Production;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Controllers.Manufacturing.Production;

public class ProductionOrdersController : AppController
{
    private readonly IProductionOrderService _service;
    private readonly ApplicationDbContext _db;

    public ProductionOrdersController(IProductionOrderService service, ApplicationDbContext db)
    {
        _service = service;
        _db = db;
    }

    public async Task<IActionResult> Index(string? status = null)
    {
        var list = await _service.GetAllAsync(status);
        ViewBag.Status = status;
        return View(list);
    }

    public async Task<IActionResult> Details(int id)
    {
        var model = await _service.GetByIdAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    public async Task<IActionResult> Execution(int id)
    {
        var model = await _service.GetExecutionAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    public async Task<IActionResult> Wastage(int id)
    {
        var order = await _service.GetByIdAsync(id);
        if (order == null) return NotFound();
        ViewBag.OrderId = id;
        ViewBag.OrderNumber = order.OrderNumber;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RecordWastage(int id, string wastageType, decimal qty, string reason)
    {
        var result = await _service.RecordWastageAsync(id, wastageType, qty, reason, 1);
        // FIX: 3 arguments hata kar 2 kar diye
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    public async Task<IActionResult> Exceptions(int id)
    {
        var order = await _service.GetByIdAsync(id);
        if (order == null) return NotFound();
        ViewBag.OrderId = id;
        ViewBag.OrderNumber = order.OrderNumber;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RecordException(int id, string exceptionType, string severity, string description)
    {
        var result = await _service.RecordExceptionAsync(id, exceptionType, severity, description, 1);
        // FIX: 3 arguments hata kar 2 kar diye
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadLookupsAsync();
        var model = new ProductionOrderViewModel
        {
            PlannedStart = DateTime.Today,
            PlannedEnd = DateTime.Today.AddDays(1),
            Priority = 3,
            Status = "Planned"
        };
        return FormView("_OrderForm", "Create", model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductionOrderViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadLookupsAsync();
            return Invalid("_OrderForm", "Create", model);
        }

        var userId = 1;
        var result = await _service.CreateAsync(model, userId);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await LoadLookupsAsync();
            return Invalid("_OrderForm", "Create", model);
        }

        // FIX: Saved 2 arguments
        return Saved(result.Message, "Index");
    }

    [HttpPost]
    public async Task<IActionResult> Release(int id)
    {
        var result = await _service.ReleaseAsync(id, 1);
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Start(int id)
    {
        var result = await _service.StartAsync(id, 1);
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Complete(int id)
    {
        var result = await _service.CompleteAsync(id, 1);
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Close(int id)
    {
        var result = await _service.CloseAsync(id, 1);
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Hold(int id)
    {
        var result = await _service.HoldAsync(id, 1);
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _service.CancelAsync(id, 1);
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> StartOperation(int id)
    {
        var result = await _service.StartOperationAsync(id, 1);
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> CompleteOperation(int id)
    {
        var result = await _service.CompleteOperationAsync(id, 1);
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    private async Task LoadLookupsAsync()
    {
        ViewBag.Plants = new SelectList(await _db.Plants.AsNoTracking().OrderBy(p => p.PlantName).ToListAsync(), "PlantID", "PlantName");
        ViewBag.Products = new SelectList(await _db.Products.AsNoTracking().OrderBy(p => p.ProductName).ToListAsync(), "ProductID", "ProductName");
        ViewBag.Recipes = new SelectList(await _db.Recipes.AsNoTracking().Where(r => r.Status == "Approved").OrderBy(r => r.RecipeCode).ToListAsync(), "RecipeID", "RecipeCode");
        ViewBag.UOMs = new SelectList(await _db.UnitsOfMeasure.AsNoTracking().OrderBy(u => u.UOMName).ToListAsync(), "UOMID", "UOMName");
    }
}