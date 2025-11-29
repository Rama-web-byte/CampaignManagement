using CampaignManagement.Models;
using CampaignManagement.ViewModels.Users;

namespace CampaignManagement.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(Guid userId);

        Task<User>CreateUserAsync(User user);

        Task<bool>EmailExistsAsync(string email);

        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<bool> DeleteUserAsync(Guid guid);

        Task<bool> UpdateUserAsync(UpdateUserViewModel user);
        Task<bool> UpdateUserByAdminAsync(AdminUpdateUserViewModel user);

    }
}
