using CampaignManagement.Services.Contracts;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CampaignManagement.Telemetry
{
    public class JwtUserTelemetryInitializer:ITelemetryInitializer
    {
        private readonly IUserContext _userContext;

        public JwtUserTelemetryInitializer(IUserContext   userContext)
        {
            _userContext = userContext;
        }

        public void Initialize(ITelemetry telemetry)
        {   
            if( !string.IsNullOrEmpty(_userContext.UserId))
            {
                telemetry.Context.User.AuthenticatedUserId= _userContext.UserId;
            }
        }
    }
}
