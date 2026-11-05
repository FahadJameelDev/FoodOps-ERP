using FoodOpsERP.Models.AI.ViewModels;
using System.Threading.Tasks;

namespace FoodOpsERP.Services.AI
{
    public interface IAIAssistantService
    {
        Task<string> AskAssistantAsync(string domainContext, string question, int userId);
    }
}