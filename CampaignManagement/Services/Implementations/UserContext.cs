using CampaignManagement.Services.Contracts;

namespace CampaignManagement.Services.Implementations
{
    public class UserContext : IUserContext
    {
        public string UserId { get; set; } = "Anonymous";
        public string UserEmail { get; set; } = "NA";

        public string UserRole { get; set; } = "None";
    }
}
