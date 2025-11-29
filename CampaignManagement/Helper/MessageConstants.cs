using CampaignManagement.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CampaignManagement.Helper
{
    public class MessageConstants
    {
        //Campaigns

        // Success Messages
        public static readonly string CampaignCreatedSuccessfully = "New campaign created successfully";
        public static readonly string CampaignDeletedSuccessfully = "Campaign with ID {0} deleted successfully";
        public static readonly string CampaignUpdatedSuccessfully = "Campaign with ID {0} updated successfully";


        //Error Messages
        public static readonly string ErrorRetrievingCampaign = "Error occurred while retrieving campaign with ID {0}";
        public static readonly string ErrorCreatingCampaign = "Error occurred while creating a new campaign";
        public static readonly string ErrorUpdatingCampaign = "Error occurred while updating campaign with ID {0}";
        public static readonly string ErrorDeletingCampaign = "Error occurred while deleting campaign with ID {0}";
        public static readonly string InternalServerError = "Internal server error";

        // Warning Messages
        public static readonly string CampaignNotFound = "Campaign with ID {0} not found";
        //public static readonly string DuplicateCampaignName = "A campaign with the same name already exists.";
        public static readonly string InvalidModelState = "Invalid model state for the new campaign";

        //Status Codes
        public const int NotFound = 404;
        public const int InternalError = 500;
        public const int BadRequest = 400;

        //Users

        public static readonly string UserCreatedSuccessfully = "New User created successfully";
        public static readonly string UserDeletedSuccessfully = "User with ID {0} deleted successfully";
        public static readonly string UserUpdatedSuccessfully = "User with ID {0} updated successfully";

        public static readonly string ErrorRetrievingUser = "Error occurred while retrieving user with ID {0}";
        public static readonly string ErrorCreatingUser = "Error occurred while creating a new user";
        public static readonly string ErrorUpdatingUser = "Error occurred while updating user with ID {0}";
        public static readonly string ErrorDeletingUser = "Error occurred while deleting user with ID {0}";

        public static readonly string GeneralError="Unexpected error occured , please try aftersometime";

        public static readonly string NotFoundError = "There are no user records available";

        public static readonly string ErrorUserLogin = "Error occurred while Login a new user";

        public static readonly string UserExist = "A user with this email already exists";
    }

}
