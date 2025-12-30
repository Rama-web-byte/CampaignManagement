using CampaignManagement.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CampaignManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Analyst")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService service)
        {
            _dashboardService = service;
        }

        [HttpGet]
        [Route("GetCampaignStatus")]
        public async Task<ActionResult> GetCampaignsStatusReportAsync()
        {
            return Ok(await _dashboardService.GetCampaignStatusSummaryAsync());
        }

        [HttpGet]
        [Route("GetCampaignProducts")]
        public async Task<ActionResult> GetCampaignProducts()
        {
            return Ok(await _dashboardService.GetCampaignsByProductAsync());
        }

    }
}

