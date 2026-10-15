using FoodOpsERP.Services.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace FoodOpsERP.Controllers.SupplyChain.Inventory;

public class InventoryController : BaseInventoryController
{
    private readonly IStockService _stock;
    private readonly IIncomingQCService _qc;

    public InventoryController(IStockService stock, IIncomingQCService qc) { _stock = stock; _qc = qc; }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var balance = await _stock.GetStockBalanceAsync();
        ViewBag.TotalProducts = balance.Count();
        ViewBag.LowStockCount = balance.Count(x => x.StockStatus == "Low" || x.StockStatus == "Out");
        ViewBag.PendingQCCount = (await _qc.GetPendingQCAsync()).Count();
        ViewBag.ExpiringCount = (await _stock.GetExpiringLotsAsync(30)).Count();
        return View();
    }
}