using Microsoft.AspNetCore.Mvc;
using CampaignManagement.Models;
using CampaignManagement.Helper;
using Microsoft.Extensions.Logging;
using CampaignManagement.Services.Contracts;
using CampaignManagement.ViewModels.Campaigns;
using Microsoft.EntityFrameworkCore;
using CampaignManagement.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.RateLimiting;

namespace CampaignManagement.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
        #region Fields

        private readonly ICampaignService _campaignService;
        private readonly ILogger<CampaignsController> _logger;
        private readonly TelemetryClient _telemetryClient;
        private readonly IUserContext _userContext;

        #endregion

        #region Ctor
        public CampaignsController(ICampaignService campaignService, ILogger<CampaignsController> logger,TelemetryClient client, IUserContext userContext)
        {
            _campaignService = campaignService;
            _logger = logger;
            _telemetryClient = client;
            _userContext = userContext;
        }

        #endregion


        #region Methods
        // GET: api/campaigns

        /// <summary>
        /// Retrieves a paginated list of campaigns.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Number of records per page.</param>
        ///  /// <param name="isActive">active campaigns.</param>
        /// <returns>List of campaigns</returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<CampaignsListViewModel>> GetCampaigns(int page = 1, int pageSize = 10, bool isActive = false)
        {
            try
            {
                var campaigns = _campaignService.GetAllCampaignsAsync(page, pageSize, isActive);
                _telemetryClient.GetMetric("ListofCampaigns").TrackValue(campaigns.Result.Campaigns.Count);
                return Ok(await _campaignService.GetAllCampaignsAsync(page, pageSize, isActive));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching campaigns by{UserId}",_userContext.UserId);
                return StatusCode(MessageConstants.InternalError, "An Unexpected error occured");
            }
        }

        /// <summary>
        /// Retrieves a specific campaign by its ID.
        /// </summary>
        /// <param name="id">Campaign ID</param>
        /// <returns>Campaign</returns>
        // GET: api/campaigns/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CampaignViewModel>> GetCampaign(Guid id)
        {
            try
            {
                var campaign = await _campaignService.GetCampaignByIdAsync(id);
                if (campaign == null)
                {
                    _logger.LogWarning(string.Format(MessageConstants.CampaignNotFound, id));
                    return NotFound(string.Format(MessageConstants.CampaignNotFound, id));
                }

                return Ok(campaign);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageConstants.ErrorRetrievingCampaign,id));
                return StatusCode(MessageConstants.InternalError, "An Unexpected error occured");
            }
        }


        /// <summary>
        /// Adds a new campaign.
        /// </summary>
        /// <param name="campaign">Campaign data</param>
        /// <returns>The created campaign</returns>
        /// 

        [EnableRateLimiting("WritePolicy")]
        [Authorize(Roles = "Admin,CampaignOwner")]
        [HttpPost]
        public async Task<ActionResult<CampaignViewModel>> CreateCampaign(CreateCampaignViewModel campaign)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(MessageConstants.InvalidModelState);
                return BadRequest(ModelState);
            }
            try
            {
                // campaign.CampaignName = SanitizationHelper.SanitizeInput(campaign.CampaignName);
               
                var newModel = await _campaignService.AddCampaignAsync(campaign);
                _telemetryClient.TrackEvent("CampaignCreated", new Dictionary<string, string>
                {
                    { "CampaignName",newModel.CampaignName },
                    {"ProductId",newModel.ProductId.ToString() }
                });

                _logger.LogInformation(MessageConstants.CampaignCreatedSuccessfully);
                return CreatedAtAction(nameof(GetCampaign), new { id = newModel.CampaignId }, newModel);
            }

            catch (DbUpdateException ex)
            {
                _telemetryClient.TrackException(ex);
                _logger.LogError(ex,"Error occured while fetching campaign by {userid}",_userContext.UserId);
                return StatusCode(MessageConstants.InternalError, "An Unexpected DB error occured");
            }
            catch (InvalidOperationException ex)
            {
                _telemetryClient.TrackException(ex);
                _logger.LogError(ex,ex.Message);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                _logger.LogError(ex, ex.Message);
                return StatusCode(MessageConstants.InternalError, "An Unexpected error occured");
            }
        }
        [Authorize]
        [HttpGet("activeProducts")]
        public async Task<ActionResult<IEnumerable<ViewModels.Products.ProductViewModel>>> GetActiveProducts()
        {
            try
            {
                var products = await _campaignService.GetActiveProducts();
                if (products == null || !products.Any())
                {
                    return NotFound("No active products found");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                _logger?.LogError(ex, ex.Message);
                return StatusCode(MessageConstants.InternalError, "An Unexpected error occured");
            }
        }


        // <summary>
        // Updates an existing campaign.
        // </summary>
        // <param name="id">Campaign ID</param>
        // <param name="campaign">Updated campaign data</param>
        // <returns>Result of the operation</returns>
        [EnableRateLimiting("WritePolicy")]
        [Authorize(Roles = "Admin,CampaignOwner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCampaign(Guid id, UpdateCampaignViewModel campaign)
        {
            try
            {
                if (id != campaign.CampaignId)
                {
                    _logger.LogWarning($"ID mismatch in campaign update request: {id} != {campaign.CampaignId}");
                    return BadRequest("Campaign ID mismatch");
                }
                if(!ModelState.IsValid)
                {
                    _logger.LogWarning(MessageConstants.InvalidModelState);
                    return BadRequest();
                }

                var updated=await _campaignService.UpdateCampaignAsync(campaign);
                if (!updated)
                {
                    _logger.LogWarning("Campaign with ID {CampaignId} not found",campaign.CampaignId);
                    return NotFound($"Campaign with ID {campaign.CampaignId} not found.");
                }
                _logger.LogInformation("Campaign with ID {id} updated successfully.",campaign.CampaignId);
                return NoContent();
            }
           catch(DbUpdateException ex)
            {

                _telemetryClient.TrackException(ex);
                _logger.LogError(ex,"Error updating a campaign{id} by {userid}",campaign.CampaignId,_userContext.UserId);
                return StatusCode(MessageConstants.InternalError, "An Unexpected DB error occured");
            }
            catch(InvalidOperationException ex)
            {
                _telemetryClient.TrackException(ex);
                _logger.LogError(ex, "Error updating a campaign{id} by {userid}", campaign.CampaignId, _userContext.UserId);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                _logger.LogError(ex, "Error updating a campaign{id} by {userid}", campaign.CampaignId, _userContext.UserId);
                return StatusCode(MessageConstants.InternalError, "An Unexpected error occured");
            }
        }

        /// <summary>
        /// Deletes a campaign by its ID.
        /// </summary>
        /// <param name="id">Campaign ID</param>
        /// <returns>Result of the operation</returns>
        /// 
        [EnableRateLimiting("WritePolicy")]
        [Authorize(Roles = "Admin,CampaignOwner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampaign(Guid id)
        {
            try
            {
                var isDeleted=await _campaignService.DeleteCampaignAsync(id);
                if (!isDeleted)
                {
                    _logger.LogInformation($"Campaign with ID {id} not found.");
                    return NotFound($"The Campaign Id{id} not found");
                }
                _logger.LogInformation($"Campaign with ID {id} deleted successfully.");
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _telemetryClient.TrackException(ex);
                _logger.LogError(ex, "Error deleting a campaign{id} by {userid}", id, _userContext.UserId);
                return StatusCode(MessageConstants.InternalError, "An Unexpected DB error occured");
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                _logger.LogError(ex, "Error deleting a campaign{id} by {userid}", id, _userContext.UserId);
                return StatusCode(MessageConstants.InternalError, "An Unexpected error occured");
            }
        }

        #endregion
    } 
}
    


