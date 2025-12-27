
using CampaignManagement.Models;
using CampaignManagement.ViewModels.Campaigns;
using CampaignManagement.ViewModels.Products;
namespace CampaignManagement.Services.Contracts
{


    public interface ICampaignService
    {
        Task<CampaignsListViewModel> GetAllCampaignsAsync(int page, int pageSize,bool active);
        Task<CampaignViewModel?> GetCampaignByIdAsync(Guid id);
        Task<CampaignViewModel> AddCampaignAsync(CreateCampaignViewModel campaign);
        Task<IEnumerable<ViewModels.Products.ProductViewModel>> GetActiveProducts();
        
        Task <(bool Success,string Message)>UpdateCampaignAsync(UpdateCampaignViewModel campaign);
        Task<bool> DeleteCampaignAsync(Guid id);
    }

}
