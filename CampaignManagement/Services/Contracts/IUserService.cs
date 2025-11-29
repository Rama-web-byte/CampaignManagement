using CampaignManagement.Models;
using CampaignManagement.ViewModels.Users;

namespace CampaignManagement.Services.Contracts
{
     public interface IUserService
    {
        Task<UserViewModel> GetUserByEmailAsync(string email);
        Task<UserViewModel> GetUserByIdAsync(Guid userId);

        Task <UserViewModel> CreateUserAsync(CreateUserViewModel user);

        Task<bool> EmailExistsAsync(string email);

        Task<UserListViewModel> GetAllUsersAsync();

        Task<bool> DeleteUserAsync(Guid guid);

        Task<bool> UpdateUserAsync(UpdateUserViewModel user);

        Task<bool> UpdateUserByAdminAsync(AdminUpdateUserViewModel user);

        Task<AuthResponseViewModel> LoginAsync(LoginViewModel login);
    }
}
