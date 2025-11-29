namespace CampaignManagement.ViewModels.Campaigns
{
    public class CampaignViewModel
    {

         public Guid CampaignId { get; set; }       // Unique identifier for the campaign
        public string CampaignName { get; set; }  // Name of the campaign
        public DateTime StartDate { get; set; }    // Start date of the campaign
        public DateTime EndDate { get; set; }      // End date of the campaign
        public bool IsActive { get; set; }         // Indicates if the campaign is active

        public Guid ProductId { get; set; }
        public string ProductName { get; set; }    // Name of the associated product



    }
}
