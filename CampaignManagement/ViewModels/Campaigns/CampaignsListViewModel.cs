using CampaignManagement.Models;
using CampaignManagement.ViewModels;
namespace CampaignManagement.ViewModels.Campaigns
{
    public class CampaignsListViewModel
    {

        public List<CampaignViewModel> Campaigns { get; set; }
        // Pagination properties
        public int CurrentPage { get; set; }       // Current page number
        public int TotalPages { get; set; }         // Total number of pages
        public int PageSize { get; set; }           // Number of items per page
        public int TotalCount { get; set; }         // Total number of campaigns

        public string Message { get; set; }
    }
}

    

