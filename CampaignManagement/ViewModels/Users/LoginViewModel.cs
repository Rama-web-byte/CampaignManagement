namespace CampaignManagement.ViewModels.Users
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class AuthResponseViewModel
    {
         public string Token { get;set; }

        public string UserName { get; set; }

        public string Role { get; set; }
    }
}
