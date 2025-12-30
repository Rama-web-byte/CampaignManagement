using CampaignManagement.Models;

namespace CampaignManagement.Repositories.Contracts
{
    public interface IDashboardRepository
    {

            Task<List<CampaignStatusSummary>> GetCampaignStatusSummaryAsync();
            Task<List<CampaignsByProduct>> GetCampaignsByProductAsync();
        
    }
}
