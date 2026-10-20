using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Infrastructure;

public abstract class AppController : Controller
{
    protected bool IsAjax => Request.IsAjax();

    // GET: modal ko sirf form partial, direct URL ko poora page
    protected IActionResult FormView(string partial, string page, object model) =>
        IsAjax ? PartialView(partial, model) : View(page, model);

    // Validation fail: modal ko 422 + form (errors ke saath)
    protected IActionResult Invalid(string partial, string page, object model)
    {
        if (!IsAjax) return View(page, model);
        Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        return PartialView(partial, model);
    }

    protected IActionResult Saved(string message, string action = "Index")
    {
        if (IsAjax) return Json(new { success = true, message });
        TempData["Success"] = message;
        return RedirectToAction(action);
    }

    protected IActionResult Failed(string message, string action = "Index")
    {
        if (IsAjax)
        {
            Response.StatusCode = StatusCodes.Status409Conflict;
            return Json(new { success = false, message });
        }
        TempData["Error"] = message;
        return RedirectToAction(action);
    }

    // null = success, warna user ko dikhane wala message
    protected async Task<string?> TryAsync(Func<Task> work)
    {
        try { await work(); return null; }
        catch (InvalidOperationException ex) { return ex.Message; }
        catch (DbUpdateConcurrencyException) { return "This record was changed by someone else. Please reload and try again."; }
    }
    // FIX: Identity se asal UserID nikalne ke liye
    protected int GetCurrentUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (claim != null && int.TryParse(claim.Value, out var id))
            return id;
        return 0; // Not logged in
    }
}