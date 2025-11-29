using Microsoft.AspNetCore.Mvc;
using CampaignManagement.Models;
using CampaignManagement.Helper;
using Microsoft.Extensions.Logging;
using CampaignManagement.Services.Contracts;
using CampaignManagement.ViewModels.Campaigns;
using Microsoft.EntityFrameworkCore;
using CampaignManagement.ViewModels.Products;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Authorization;

namespace CampaignManagement.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
        #region Fields

        private readonly ICampaignService _campaignService;
        private readonly ILogger<CampaignsController> _logger;

        #endregion

        #region Ctor
        public CampaignsController(ICampaignService campaignService, ILogger<CampaignsController> logger)
        {
            _campaignService = campaignService;
            _logger = logger;
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
                return Ok(await _campaignService.GetAllCampaignsAsync(page, pageSize, isActive));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching campaigns");
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
        [Authorize(Roles = "Admin,CampaignOwner")]
        [HttpPost]
        public async Task<ActionResult<CampaignViewModel>> PostCampaign(CreateCampaignViewModel campaign)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(MessageConstants.InvalidModelState);
                return BadRequest(ModelState);
            }
            try
            {
                // campaign.CampaignName = SanitizationHelper.SanitizeInput(campaign.CampaignName);
                //campaign.CampaignId = Guid.NewGuid();  // Ensure a new ID is generated
                var newModel = await _campaignService.AddCampaignAsync(campaign);
              
                _logger.LogInformation(MessageConstants.CampaignCreatedSuccessfully);
                return CreatedAtAction(nameof(GetCampaign), new { id = newModel.CampaignId }, newModel);
            }
            
            catch (DbUpdateException ex)
            {

                _logger.LogError(ex.Message);
                return StatusCode(MessageConstants.InternalError, "An Unexpected DB error occured");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex,ex.Message);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
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

                var updated=await _campaignService.UpdateCampaignAsync(campaign);
                if (!updated)
                {
                    _logger.LogWarning($"Campaign with ID {campaign.CampaignId} not found.");
                    return NotFound($"Campaign with ID {campaign.CampaignId} not found.");
                }
                _logger.LogInformation($"Campaign with ID {id} updated successfully.");
                return NoContent();
            }
           catch(DbUpdateException ex)
            {

                _logger.LogError(ex.Message);
                return StatusCode(MessageConstants.InternalError, "An Unexpected DB error occured");
            }
            catch(InvalidOperationException ex)
            {
                _logger.LogError(ex.Message);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating campaign with ID {id}");
                return StatusCode(MessageConstants.InternalError, "An Unexpected error occured");
            }
        }

        /// <summary>
        /// Deletes a campaign by its ID.
        /// </summary>
        /// <param name="id">Campaign ID</param>
        /// <returns>Result of the operation</returns>
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

                _logger.LogError(ex.Message);
                return StatusCode(MessageConstants.InternalError, "An Unexpected DB error occured");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                return StatusCode(MessageConstants.InternalError, "An Unexpected error occured");
            }
        }

        #endregion
    }
}
    


