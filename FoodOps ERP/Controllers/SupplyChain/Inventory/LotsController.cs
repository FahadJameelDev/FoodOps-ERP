using FoodOpsERP.Services.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace FoodOpsERP.Controllers.SupplyChain.Inventory;

public class LotsController : BaseInventoryController
{
    private readonly IStockService _stock;
    public LotsController(IStockService stock) => _stock = stock;

    [HttpGet]
    public async Task<IActionResult> Index() => View(await _stock.GetLotsAsync());

    [HttpGet]
    public async Task<IActionResult> Expiry(int days = 30)
    {
        days = Math.Clamp(days, 1, 365);
        ViewBag.Days = days;
        return View(await _stock.GetExpiringLotsAsync(days));
    }
}