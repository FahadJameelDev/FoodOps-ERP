using FoodOpsERP.Models.Executive.ViewModels;

namespace FoodOpsERP.Services.Executive;

public interface IDashboardService
{
    Task<DashboardViewModel> GetDashboardDataAsync();
}