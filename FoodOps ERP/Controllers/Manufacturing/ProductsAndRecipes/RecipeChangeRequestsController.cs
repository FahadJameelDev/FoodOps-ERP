using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;
using FoodOpsERP.Services.Manufacturing.ProductsAndRecipes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Controllers.Manufacturing.ProductsAndRecipes;

public class RecipeChangeRequestsController : AppController
{
    private readonly IRecipeChangeRequestService _service;
    private readonly ApplicationDbContext _db;

    public RecipeChangeRequestsController(IRecipeChangeRequestService service, ApplicationDbContext db)
    {
        _service = service;
        _db = db;
    }

    public async Task<IActionResult> Index(int? recipeId = null)
    {
        var list = await _service.GetAllAsync(recipeId);
        ViewBag.RecipeId = recipeId;
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? recipeId = null)
    {
        var model = new RecipeChangeRequestViewModel
        {
            RecipeID = recipeId ?? 0,
            Status = "Requested"
        };
        await PopulateDropdownsAsync(model); // ✅ Populate into ViewModel
        return FormView("_ChangeRequestForm", "Create", model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RecipeChangeRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model);
            return Invalid("_ChangeRequestForm", "Create", model);
        }

        var userId = 1; // TODO: Identity User
        var result = await _service.CreateAsync(model, userId);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateDropdownsAsync(model);
            return Invalid("_ChangeRequestForm", "Create", model);
        }

        return Saved(result.Message, "Index");
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int id, string? comments)
    {
        var userId = 1;
        var result = await _service.ApproveAsync(id, userId, comments ?? "");

        if (!result.Success)
            return Failed(result.Message);

        return Saved(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Reject(int id, string? comments)
    {
        if (string.IsNullOrWhiteSpace(comments))
            return Failed("Rejection reason is required.");

        var userId = 1;
        var result = await _service.RejectAsync(id, userId, comments);

        if (!result.Success)
            return Failed(result.Message);

        return Saved(result.Message);
    }

    // ✅ Reusable Strongly-Typed Populate Method
    private async Task PopulateDropdownsAsync(RecipeChangeRequestViewModel model)
    {
        model.RecipeList = await _db.Recipes.AsNoTracking()
            .Where(r => r.Status == "Approved")
            .OrderBy(x => x.RecipeCode)
            .Select(x => new SelectListItem
            {
                Value = x.RecipeID.ToString(),
                Text = x.RecipeCode
            })
            .ToListAsync();
    }
}