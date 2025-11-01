
using CampaignManagement.Models;
using CampaignManagement.ViewModels.Campaigns;
using CampaignManagement.ViewModels.Products;
namespace CampaignManagement.Services.Contracts
{


    public interface ICampaignService
    {
        Task<IEnumerable<CampaignsListViewModel>> GetAllCampaignsAsync(int page, int pageSize,bool active);
        Task<CampaignViewModel> GetCampaignByIdAsync(Guid id);
        Task<CampaignViewModel> AddCampaignAsync(CreateCampaignViewModel campaign);
        Task<IEnumerable<ViewModels.Products.ProductViewModel>> GetActiveProducts();
        
        Task UpdateCampaignAsync(CampaignViewModel campaign);
        Task DeleteCampaignAsync(Guid id);
    }

}
