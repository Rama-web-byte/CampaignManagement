namespace CampaignManagement.Services.Contracts
{
    public interface IUserContext
    {
        string UserId { get; set; }

        string UserEmail { get; set; }

        string UserRole { get; set; }


    }
}
