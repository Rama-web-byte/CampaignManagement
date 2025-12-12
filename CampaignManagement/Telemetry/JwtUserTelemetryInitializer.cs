using CampaignManagement.Services.Contracts;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CampaignManagement.Telemetry
{
    public class JwtUserTelemetryInitializer:ITelemetryInitializer
    {
        private readonly IServiceProvider _provider;

        public JwtUserTelemetryInitializer(IServiceProvider serviceProvider)
        {
            _provider = serviceProvider;
        }

        public void Initialize(ITelemetry telemetry)
        {   
            using var scope=_provider.CreateScope();
            var userContext=scope.ServiceProvider.GetService<IUserContext>();

            if( userContext!=null)
            {
                telemetry.Context.User.AuthenticatedUserId= userContext.UserId;
            }
        }
    }
}
