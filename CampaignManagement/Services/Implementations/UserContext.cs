using CampaignManagement.Services.Contracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CampaignManagement.Services.Implementations
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContext;

        public UserContext(IHttpContextAccessor accessor)
        {
            _httpContext = accessor;
        }

        public string UserId=>_httpContext.HttpContext?.User?.FindFirst("UserId")?.Value;
        public string UserEmail => _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        public string UserRole => _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;

        public string UserName => _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
    }
}
