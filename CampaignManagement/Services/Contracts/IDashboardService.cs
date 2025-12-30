using CampaignManagement.Models;

namespace CampaignManagement.Services.Contracts
{
    public interface IDashboardService
    {
        Task<List<CampaignStatusSummary>> GetCampaignStatusSummaryAsync();
        Task<List<CampaignsByProduct>> GetCampaignsByProductAsync();
    }
}
