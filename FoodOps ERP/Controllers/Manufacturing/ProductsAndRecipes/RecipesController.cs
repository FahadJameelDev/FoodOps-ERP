using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;
using FoodOpsERP.Services.Manufacturing.ProductsAndRecipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Controllers.Manufacturing.ProductsAndRecipes;

[Authorize(Roles = "Administrator, Production Manager")]

public class RecipesController : AppController
{
    private readonly IRecipeService _service;
    private readonly ApplicationDbContext _db;

    public RecipesController(IRecipeService service, ApplicationDbContext db)
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

    public async Task<IActionResult> PendingApprovals()
    {
        var list = await _service.GetPendingApprovalsAsync();
        return View(list);
    }

    public async Task<IActionResult> Details(int id)
    {
        var model = await _service.GetByIdAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new RecipeViewModel
        {
            VersionNo = 1,
            Status = "Draft",
            Items = new List<RecipeItemViewModel>()
        };
        await PopulateDropdownsAsync(model); // ✅ Naya method call kiya
        return FormView("_RecipeForm", "Create", model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RecipeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model);
            return Invalid("_RecipeForm", "Create", model);
        }

        var userId = 1; // TODO
        var result = await _service.CreateAsync(model, userId);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateDropdownsAsync(model);
            return Invalid("_RecipeForm", "Create", model);
        }

        return Saved(result.Message, "Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _service.GetByIdAsync(id);
        if (model == null) return NotFound();

        if (model.Status != "Draft" && model.Status != "Rejected")
            return Failed("Only Draft or Rejected recipes can be edited.");

        await PopulateDropdownsAsync(model);
        return FormView("_RecipeForm", "Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(RecipeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model);
            return Invalid("_RecipeForm", "Edit", model);
        }

        var userId = 1; // TODO
        var result = await _service.UpdateAsync(model, userId);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateDropdownsAsync(model);
            return Invalid("_RecipeForm", "Edit", model);
        }

        return Saved(result.Message, "Index");
    }

    // ... (SubmitForApproval, Approve, Reject, SetCurrent wese hi rahenge)
    [HttpPost]
    public async Task<IActionResult> SubmitForApproval(int id)
    {
        var userId = 1;
        var result = await _service.SubmitForApprovalAsync(id, userId);
        if (!result.Success) return Failed(result.Message);
        return Saved(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int id, string? comments = null)
    {
        var userId = 1;
        var result = await _service.ApproveAsync(id, userId, comments);
        if (!result.Success) return Failed(result.Message);
        return Saved(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Reject(int id, string? comments = null)
    {
        if (string.IsNullOrWhiteSpace(comments))
            comments = "Rejected from list view.";

        var userId = 1;
        var result = await _service.RejectAsync(id, userId, comments);
        if (!result.Success) return Failed(result.Message);
        return Saved(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> SetCurrent(int id)
    {
        var userId = 1;
        var result = await _service.SetCurrentAsync(id, userId);
        if (!result.Success) return Failed(result.Message);
        return Saved(result.Message);
    }

    // ✅ Naya Method (Purana LoadLookupsAsync hata dein)
    private async Task PopulateDropdownsAsync(RecipeViewModel model)
    {
        model.ProductList = await _db.Products.AsNoTracking()
            .Where(p => p.IsActive)
            .OrderBy(x => x.ProductName)
            .Select(x => new SelectListItem { Value = x.ProductID.ToString(), Text = x.ProductName })
            .ToListAsync();

        model.UOMList = await _db.UnitsOfMeasure.AsNoTracking()
            .OrderBy(x => x.UOMName)
            .Select(x => new SelectListItem { Value = x.UOMID.ToString(), Text = x.UOMName })
            .ToListAsync();
    }
}