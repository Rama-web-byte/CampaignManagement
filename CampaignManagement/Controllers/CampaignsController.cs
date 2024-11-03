using Microsoft.AspNetCore.Mvc;
using CampaignManagement.Models;
using CampaignManagement.Helper;
using Microsoft.Extensions.Logging;
using CampaignManagement.Services.Contracts;
using CampaignManagement.ViewModels.Campaigns;
using Microsoft.EntityFrameworkCore;
using CampaignManagement.ViewModels.Products;

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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Campaign>>> GetCampaigns(int page = 1, int pageSize = 10, bool isActive = false)
        {
            try
            {
                return Ok(await _campaignService.GetAllCampaignsAsync(page, pageSize, isActive));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(MessageConstants.InternalError, MessageConstants.InternalServerError);
            }
        }

        /// <summary>
        /// Retrieves a specific campaign by its ID.
        /// </summary>
        /// <param name="id">Campaign ID</param>
        /// <returns>Campaign</returns>
        // GET: api/campaigns/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Campaign>> GetCampaign(Guid id)
        {
            try
            {
                var campaign = await _campaignService.GetCampaignByIdAsync(id);
                if (campaign == null)
                {
                    _logger.LogWarning(string.Format(MessageConstants.CampaignNotFound, id));
                    return NotFound();
                }

                return Ok(campaign);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageConstants.ErrorRetrievingCampaign,id));
                return StatusCode(MessageConstants.InternalError, MessageConstants.InternalServerError);
            }
        }

       
        /// <summary>
        /// Adds a new campaign.
        /// </summary>
        /// <param name="campaign">Campaign data</param>
        /// <returns>The created campaign</returns>
        [HttpPost]
        public async Task<ActionResult<Campaign>> PostCampaign(CreateCampaignViewModel campaign)
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
                 var newmodel=await _campaignService.AddCampaignAsync(campaign);
                _logger.LogInformation(MessageConstants.CampaignCreatedSuccessfully);
                return CreatedAtAction(nameof(GetCampaign), new { id = newmodel.CampaignId }, newmodel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(MessageConstants.InternalError, MessageConstants.InternalServerError);
            }
        }

        [HttpGet("activeProducts")]
        public async Task<ActionResult<IEnumerable<ViewModels.Products.ProductViewModel>>> GetActiveProducts()
        {                                                                                        
            var products = await _campaignService.GetActiveProducts();
            return Ok(products);
        }

        // <summary>
        // Updates an existing campaign.
        // </summary>
        // <param name="id">Campaign ID</param>
        // <param name="campaign">Updated campaign data</param>
        // <returns>Result of the operation</returns>
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCampaign(Guid id, Campaign campaign)
        //{
        //    try
        //    {
        //        if (id != campaign.CampaignId)
        //        {
        //            _logger.LogWarning($"ID mismatch in campaign update request: {id} != {campaign.CampaignId}");
        //            return BadRequest("Campaign ID mismatch");
        //        }

        //        await _campaignService.UpdateCampaignAsync(campaign);
        //        _logger.LogInformation($"Campaign with ID {id} updated successfully.");
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error occurred while updating campaign with ID {id}");
        //        return StatusCode(MessageConstants.InternalError, MessageConstants.InternalServerError);
        //    }
        //}

        ///// <summary>
        ///// Deletes a campaign by its ID.
        ///// </summary>
        ///// <param name="id">Campaign ID</param>
        ///// <returns>Result of the operation</returns>
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCampaign(Guid id)
        //{
        //    try
        //    {
        //        await _campaignService.DeleteCampaignAsync(id);
        //        _logger.LogInformation($"Campaign with ID {id} deleted successfully.");
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error occurred while deleting campaign with ID {id}");
        //        return StatusCode(MessageConstants.InternalError, MessageConstants.InternalServerError);
        //    }
        //}

        #endregion
    }
}
    


