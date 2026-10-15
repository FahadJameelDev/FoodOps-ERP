using FoodOpsERP.Infrastructure;

namespace FoodOpsERP.Controllers.SupplyChain.Inventory;

public abstract class BaseInventoryController : AppController
{
    // TODO (Identity aane par): int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
    // Abhi har audit column (CreatedByUserID, AdjustedByUserID…) mein user 1 ja raha hai.
    protected int GetCurrentUserId() => 1;
}