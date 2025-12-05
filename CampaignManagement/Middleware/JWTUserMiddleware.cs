using CampaignManagement.Services.Contracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
namespace CampaignManagement.Middleware
{
    public class JWTUserMiddleware
    {
        private readonly RequestDelegate _next;

        public JWTUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpcontext,IUserContext userContext)
        {
            if (httpcontext.User.Identity?.IsAuthenticated == true)
            {
                userContext.UserId = httpcontext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                    httpcontext.User.FindFirst("sub")?.Value ?? "Anonymous";
                userContext.UserEmail = httpcontext.User.FindFirst(ClaimTypes.Email)?.Value ??
                    httpcontext.User.FindFirst("ClaimTypes.Email")?.Value ?? "None";
                userContext.UserRole = httpcontext.User.FindFirst(ClaimTypes.Role)?.Value ??
                    httpcontext.User.FindFirst("ClaimTypes.Role")?.Value ?? "None";
                        
            }

            await _next(httpcontext);
        }

    }
}
