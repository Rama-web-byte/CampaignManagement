using CampaignManagement.Models;

namespace CampaignManagement.ViewModels.Users
{
    public class CreateUserViewModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public UserRole Role { get; set; }

       

    }
}
