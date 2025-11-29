using CampaignManagement.Models;

namespace CampaignManagement.ViewModels.Users
{
    public class UserListViewModel
    {
        public List<AdminUserViewModel> Users{ get; set; }

        public int CurrentPage { get; set; }       // Current page number
        public int TotalPages { get; set; }         // Total number of pages
        public int PageSize { get; set; }           // Number of items per page
        public int TotalCount { get; set; }         // Total number of campaigns

        public string Message { get; set; }
    }
}
