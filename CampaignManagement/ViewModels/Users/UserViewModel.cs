using CampaignManagement.Models;

namespace CampaignManagement.ViewModels.Users
{
    public class UserViewModel
    {
        public Guid UserId { get; set; } 

        public string UserName { get; set; }

        public string Email { get; set; }

        public UserRole Role { get; set; }
    }
}
