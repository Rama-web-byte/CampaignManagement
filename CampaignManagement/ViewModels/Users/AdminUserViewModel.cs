namespace CampaignManagement.ViewModels.Users
{
    public class AdminUserViewModel
    {
       
            public Guid UserId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }
            public DateTime CreatedAt { get; set; }
        

    }
}
