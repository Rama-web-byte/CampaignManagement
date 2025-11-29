using CampaignManagement.Data;
using CampaignManagement.Models;
using CampaignManagement.Repositories.Contracts;
using CampaignManagement.ViewModels.Users;
using Microsoft.EntityFrameworkCore;

namespace CampaignManagement.Repositories.Implementations
{
    public class UserRepository:IUserRepository
    {

        private readonly AppDbContext _context;
        public UserRepository(AppDbContext appDbContext) 
        {
            _context = appDbContext;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(s => s.Email == email && !s.IsDeleted);
            return user;
        }
        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(s => s.UserId == userId && !s.IsDeleted);
            return user;
        }

        public async Task<User> CreateUserAsync(User user)
        {
           await _context.Users.AddAsync(user);
            _context .SaveChanges();
            return user;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var emailexist=await _context.Users.AnyAsync(s => s.Email == email);
            return emailexist;

        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> DeleteUserAsync(Guid guid)
        {
            bool IsDeleted = false;
            var softdeleteuser = await _context.Users.FindAsync(guid);
            if (softdeleteuser != null)
            {
                softdeleteuser.IsDeleted = true;
                IsDeleted = true;
                _context.SaveChanges();
            }
            return IsDeleted;
        }

        public async Task<bool> UpdateUserAsync(UpdateUserViewModel user)

        {
            var userExist = await _context.Users.FindAsync(user.UserId);

            if (userExist == null)
            {
                return false;
            }
            userExist.UserName = user.UserName;
            userExist.Email = user.UserEmail;
            userExist.Password = user.Password;

            _context .SaveChanges();
            return true;
        }

        public async Task<bool> UpdateUserByAdminAsync(AdminUpdateUserViewModel user)

        {
            var userExist = await _context.Users.FindAsync(user.UserId);

            if (userExist == null)
            {
                return false;
            }
            userExist.UserName = user.UserName;
            userExist.Email = user.UserEmail;
            userExist.Password = user.Password;
            userExist.Role = user.UserRole;
            userExist.IsActive = user.IsActive;
            userExist.IsDeleted = user.IsDeleted;

            _context.SaveChanges();
            return true;
        }
    }
}
