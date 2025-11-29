using CampaignManagement.Helper;
using CampaignManagement.Models;
using CampaignManagement.Services.Contracts;
using CampaignManagement.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CampaignManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
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

                return newuser;
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(MessageConstants.InternalError, MessageConstants.ErrorCreatingUser);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);

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
                return StatusCode(MessageConstants.InternalError, MessageConstants.GeneralError);
            }
        }
        [Authorize]
        [HttpPut("self")]
        public async Task<ActionResult<bool>> UpdateUser(UpdateUserViewModel  user)
        {
            try
            {
                var isUpdated = await _userService.UpdateUserAsync(user);
                return Ok(isUpdated);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(MessageConstants.InternalError, MessageConstants.ErrorUpdatingUser);
            }
            catch (Exception ex)
            {
                return StatusCode(MessageConstants.InternalError, MessageConstants.GeneralError);
            }

        }

        [Authorize(Roles ="Admin")]
        [HttpPut("Admin")]
        public async Task<ActionResult<bool>> UpdateUserByAdmin(AdminUpdateUserViewModel user)
        {
            try
            {
                var isUpdated = await _userService.UpdateUserByAdminAsync(user);
                return Ok(isUpdated);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(MessageConstants.InternalError, MessageConstants.ErrorUpdatingUser);
            }
            catch (Exception ex)
            {
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
                _logger.LogWarning(ex.Message);
                return Unauthorized("Invalid Email or Password");
            }

            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(MessageConstants.InternalError, MessageConstants.ErrorUserLogin);
            }
        }

    }
}
