using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Security.ViewModels;
using FoodOpsERP.Services.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // <-- YEH ADD KAREIN

namespace FoodOpsERP.Controllers.Security;

[Authorize(Roles = "Administrator")] // <-- SIRF ADMIN

public class RolesController : AppController
{
    private readonly IRoleService _service;

    public RolesController(IRoleService service) => _service = service;

    public async Task<IActionResult> Index()
    {
        var list = await _service.GetAllRolesAsync();
        return View(list);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return FormView("_RoleForm", "Create", new RoleViewModel { IsActive = true });
    }

    [HttpPost]
    public async Task<IActionResult> Create(RoleViewModel model)
    {
        if (!ModelState.IsValid) return Invalid("_RoleForm", "Create", model);

        var result = await _service.CreateRoleAsync(model, GetCurrentUserId());
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            return Invalid("_RoleForm", "Create", model);
        }
        return Saved(result.Message, "Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _service.GetRoleByIdAsync(id);
        if (model == null) return NotFound();
        return FormView("_RoleForm", "Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(RoleViewModel model)
    {
        if (!ModelState.IsValid) return Invalid("_RoleForm", "Edit", model);

        var result = await _service.UpdateRoleAsync(model, GetCurrentUserId());
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            return Invalid("_RoleForm", "Edit", model);
        }
        return Saved(result.Message, "Index");
    }

    [HttpGet]
    public async Task<IActionResult> Permissions(int id)
    {
        var model = await _service.GetRolePermissionsAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Permissions(int roleId, List<int> permissionIds)
    {
        var result = await _service.SaveRolePermissionsAsync(roleId, permissionIds ?? new List<int>(), GetCurrentUserId());
        if (!result.Success) return Failed(result.Message);
        return Saved(result.Message, "Permissions"); // 3rd argument hata dein
    }
}