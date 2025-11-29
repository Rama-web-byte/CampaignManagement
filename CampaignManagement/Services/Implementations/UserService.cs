using CampaignManagement.Models;
using CampaignManagement.Repositories.Contracts;
using CampaignManagement.Services.Contracts;
using CampaignManagement.ViewModels.Users;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace CampaignManagement.Services.Implementations
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserService(IUserRepository repo,IMapper mapper, IConfiguration config)
        {
            this._repo = repo;
            this._mapper = mapper;
            _config = config;
        }
        public async Task<UserViewModel> GetUserByEmailAsync(string email)
        {
            var user= await _repo.GetUserByEmailAsync(email);

            return _mapper.Map<UserViewModel>(user); 

        }
        public async Task<UserViewModel> GetUserByIdAsync(Guid userId)
        {
            var user = await _repo.GetUserByIdAsync(userId);
            return _mapper.Map<UserViewModel>(user);

        }
        public async Task<UserViewModel> CreateUserAsync(CreateUserViewModel user)
        {

            var newuser = _mapper.Map<User>(user);
            newuser.Password=BCrypt.Net.BCrypt.HashPassword(user.Password);
                await _repo.CreateUserAsync(newuser);
            var result=_mapper.Map<UserViewModel>(newuser);
            return result;

        }
        
        public async Task<bool> EmailExistsAsync(string email)
        {
            bool isEmailExist=await _repo.EmailExistsAsync(email);
            return isEmailExist;
        }

        public async Task<UserListViewModel> GetAllUsersAsync()
        {
            var users = await _repo.GetAllUsersAsync();
            return _mapper.Map<UserListViewModel>(users);
        }

        public async Task<bool> DeleteUserAsync(Guid userid)
        {
            var user = await _repo.GetUserByIdAsync(userid);
            if (user == null)
            {
                return false;  
            }
            await _repo.DeleteUserAsync(userid);
            return true;
            
        }

        public async Task<bool> UpdateUserAsync(UpdateUserViewModel user)

        {
            var isUpdated=await _repo.UpdateUserAsync(user);
            return isUpdated;
        }


        public async Task<bool> UpdateUserByAdminAsync(AdminUpdateUserViewModel user)

        {
            var isUpdated = await _repo.UpdateUserByAdminAsync(user);
            return isUpdated;
        }

        public async Task<AuthResponseViewModel>LoginAsync(LoginViewModel login)
        {
            var user=await _repo.GetUserByEmailAsync(login.Email);
            if (user == null) throw new UnauthorizedAccessException("Invalid Email");

            bool isPassword=BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
            if (!isPassword)
            throw new UnauthorizedAccessException("Invalid Password");


            var token = GenerateJwtToken(user);
            return new AuthResponseViewModel
            {
                Token = token,
                Role = user.Role.ToString(),
                UserName = user.UserName


            };
                       
        }

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role.ToString()),
                new Claim("UserId",user.UserId.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenhandler = new JwtSecurityTokenHandler();
            var token = tokenhandler.CreateToken(tokenDescriptor);
            return tokenhandler.WriteToken(token);

        }
    }
}

