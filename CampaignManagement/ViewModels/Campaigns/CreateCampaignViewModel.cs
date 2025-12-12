using System.ComponentModel.DataAnnotations;

namespace CampaignManagement.ViewModels.Campaigns
{
    public class CreateCampaignViewModel
    {
        [Required(ErrorMessage ="CampaignName is required")]
            public string CampaignName { get; set; } // Name of the campaign

        [Required(ErrorMessage ="ProductID is required")]
            public Guid ProductId { get; set; }       // Selected product ID

        [Required(ErrorMessage="StartDate is required")]
            public DateTime StartDate { get; set; }   // Start date of the campaign

        [Required(ErrorMessage ="EndDate is required")]
            public DateTime EndDate { get; set; }     // End date of the campaign
        

    }
}
