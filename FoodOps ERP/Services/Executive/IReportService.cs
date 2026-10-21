using FoodOpsERP.Models.Executive.ViewModels;

namespace FoodOpsERP.Services.Executive;

public interface IReportService
{
    Task<ReportResultViewModel> GenerateReportAsync(ReportFilterViewModel filter);
}