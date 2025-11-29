namespace CampaignManagement.ViewModels.Campaigns
{
    public class UpdateCampaignViewModel
    {
        public Guid CampaignId { get; set; }

        public string CampaignName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Guid ProductId { get; set; }
    }
}
