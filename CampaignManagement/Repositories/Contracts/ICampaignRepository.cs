using CampaignManagement.Models;
namespace CampaignManagement.Repositories.Contracts
{

    public interface ICampaignRepository
    {
        Task<IEnumerable<Campaign>> GetAllCampaignsAsync(int page, int pageSize);
        Task<Campaign> GetCampaignByIdAsync(Guid id);
        Task CreateCampaignAsync(Campaign campaign);

        Task<IEnumerable<Campaign>> GetActiveCampaignsAsync(int page, int pageSize);
        
        Task<IEnumerable<Product>> GetActiveProducts();
        Task UpdateCampaignAsync(Campaign campaign);
        Task DeleteCampaignAsync(Guid id);

        Task<bool>ProductExistAsync(Guid productId);

        Task<int>GetAllCampaignsCountAsync(bool active);

     
    }

}
