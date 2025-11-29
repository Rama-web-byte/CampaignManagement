using CampaignManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace CampaignManagement.ViewModels.Users
{
    public class UpdateUserViewModel
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]

        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        public string Password { get; set; }

    }


    public class AdminUpdateUserViewModel
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]

        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        public string Password { get; set; }

        [Required]

        public UserRole UserRole { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}
