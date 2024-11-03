namespace CampaignManagement.ViewModels.Campaigns
{
    public class CreateCampaignViewModel
    {
        
            public string CampaignsName { get; set; } // Name of the campaign
            public Guid ProductId { get; set; }       // Selected product ID
            public DateTime StartDate { get; set; }   // Start date of the campaign
            public DateTime EndDate { get; set; }     // End date of the campaign
        

    }
}
