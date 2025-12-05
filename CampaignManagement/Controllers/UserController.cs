using CampaignManagement.Helper;
using CampaignManagement.Models;
using CampaignManagement.Services.Contracts;
using CampaignManagement.ViewModels.Users;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;


namespace CampaignManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly TelemetryClient _telemetryClient;
        private readonly IUserContext _userContext;

        public UserController(IUserService userService, ILogger<UserController> logger,TelemetryClient client, IUserContext userContext)
        {
            _userService = userService;
            _logger = logger;
            _telemetryClient = client;
            _userContext = userContext;
        }

        //  [Authorize(Roles = "Admin")]

        [HttpGet]

        public async Task<ActionResult<UserListViewModel>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                if (users == null)
                {
                    _logger.LogWarning(MessageConstants.NotFoundError);
                    return NotFound(MessageConstants.NotFoundError);
                }
                return users;
            }


            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(MessageConstants.InternalError, MessageConstants.GeneralError);
            }

        }
        [Authorize(Roles = "Admin")]
        [HttpGet("by-email")]
        public async Task<ActionResult<UserViewModel>> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(MessageConstants.InternalError, MessageConstants.GeneralError);
            }


        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewModel>> GetUserById(Guid userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);

                return user;

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(MessageConstants.InternalError, MessageConstants.GeneralError);

            }

        }
        [EnableRateLimiting("WritePolicy")]
        [HttpPost]
        public async Task<ActionResult<UserViewModel>> CreateUser(CreateUserViewModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning(MessageConstants.InvalidModelState);
                    return BadRequest(ModelState);
                }

                var existinguser=await _userService.EmailExistsAsync(user.Email);
                if(existinguser)
                {
                    return Conflict(MessageConstants.UserExist);
                }

                var newuser = await _userService.CreateUserAsync(user);
                _telemetryClient.TrackEvent("UserCreation", new Dictionary<string, string>()
                {
                    {"Email",newuser.Email },
                    {"Role", newuser.Role.ToString()}

                 
                });

                return newuser;
            }
            catch(DbUpdateException ex)
            {
                _telemetryClient.TrackException(ex);
                _logger.LogError(ex,"Error in user creation by {userid}",_userContext.UserId);
                return StatusCode(MessageConstants.InternalError, MessageConstants.ErrorCreatingUser);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                _logger.LogError(ex, "Error in user creation by {userid}", _userContext.UserId);

                return StatusCode(MessageConstants.InternalError, MessageConstants.GeneralError);

            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("email-exists")]
        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            try
            {
                var isEmailExist=await _userService.EmailExistsAsync(email);

                return Ok(isEmailExist);
            }
            catch(Exception ex)
            {
                return StatusCode(MessageConstants.InternalError, MessageConstants.GeneralError);

            }
        }

        [EnableRateLimiting("WritePolicy")]
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteUser(Guid userId)
        {
            try
            {
                var isDeleted=await _userService.DeleteUserAsync(userId);
                return Ok(isDeleted);
            }
            catch(Exception ex)
            {
                _telemetryClient.TrackException(ex);
                _logger.LogError(ex, "Error in user{userId} deletion by {userid}",userId, _userContext.UserId);
                return StatusCode(MessageConstants.InternalError, MessageConstants.GeneralError);
            }
        }
        [EnableRateLimiting("WritePolicy")]
        [Authorize]
        [HttpPut("self")]
        public async Task<ActionResult<bool>> UpdateUser(UpdateUserViewModel  user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning(MessageConstants.InvalidModelState);
                    return BadRequest(ModelState);
                }
                var isUpdated = await _userService.UpdateUserAsync(user);
                return Ok(isUpdated);
            }
            catch (DbUpdateException ex)
            {
                _telemetryClient.TrackException(ex);
                _logger.LogError(ex, "Error in user{userId} updation by {userid}", user.UserId, _userContext.UserId);
                return StatusCode(MessageConstants.InternalError, MessageConstants.ErrorUpdatingUser);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                _logger.LogError(ex, "Error in user{userId} updation by {userid}", user.UserId, _userContext.UserId);
                return StatusCode(MessageConstants.InternalError, MessageConstants.GeneralError);
            }

        }

        [EnableRateLimiting("WritePolicy")]
        [Authorize(Roles ="Admin")]
        [HttpPut("Admin")]
        public async Task<ActionResult<bool>> UpdateUserByAdmin(AdminUpdateUserViewModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning(MessageConstants.InvalidModelState);
                    return BadRequest(ModelState);
                }
            
                var isUpdated = await _userService.UpdateUserByAdminAsync(user);
                return Ok(isUpdated);
            }
            catch (DbUpdateException ex)
            {
                _telemetryClient.TrackException(ex);
                _logger.LogError(ex, "Error in user{userId} updation by {userid}", user.UserId, _userContext.UserId);
                return StatusCode(MessageConstants.InternalError, MessageConstants.ErrorUpdatingUser);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                _logger.LogError(ex, "Error in user{userId} updation by {userid}", user.UserId, _userContext.UserId);
                return StatusCode(MessageConstants.InternalError, MessageConstants.GeneralError);
            }

        }

        [HttpPost("login")]
        public async Task <IActionResult>Login(LoginViewModel model)
        {
            try
            {
                var result=await _userService.LoginAsync(model);
                return Ok(result);
            }
            catch(UnauthorizedAccessException ex)
            {
                _telemetryClient.TrackException(ex);
                _logger.LogWarning(ex.Message);
                return Unauthorized("Invalid Email or Password");
            }

            catch(Exception ex)
            {
                _telemetryClient.TrackException(ex);
                _logger.LogError(ex.Message);
                return StatusCode(MessageConstants.InternalError, MessageConstants.ErrorUserLogin);
            }
        }

    }
}
