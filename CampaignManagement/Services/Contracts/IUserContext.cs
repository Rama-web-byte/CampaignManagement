namespace CampaignManagement.Services.Contracts
{
    public interface IUserContext
    {
        string UserId { get; }

        string UserEmail { get;}

        string UserRole { get; }

        string UserName { get; }


    }
}
