using CampaignManagement.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CampaignManagement.Helper
{
    public class MessageConstants
    {

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
    }
}
