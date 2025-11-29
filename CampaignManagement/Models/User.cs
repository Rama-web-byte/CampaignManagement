namespace CampaignManagement.Models
{
    public class User
    {
        public Guid UserId { get; set; } = Guid.NewGuid();

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public UserRole Role { get; set; } = UserRole.Analyst;

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }=DateTime.UtcNow;
    }

    public enum UserRole
    {
        Admin,
        Analyst,
        CampaignOwner

    }
}
