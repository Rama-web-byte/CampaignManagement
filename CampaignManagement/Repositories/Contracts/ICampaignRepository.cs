using CampaignManagement.Models;
namespace CampaignManagement.Repositories.Contracts
{

    public interface ICampaignRepository
    {
        Task<IEnumerable<Campaign>> GetAllAsync();
        Task<Campaign> GetByIdAsync(Guid id);
        Task AddAsync(Campaign campaign);

        Task<IEnumerable<Campaign>> GetActiveCampaignsAsync();
        
        Task<IEnumerable<Product>> GetActiveProducts();
        //Task UpdateAsync(Campaign campaign);
        //Task DeleteAsync(Guid id);
    }

}
