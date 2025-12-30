namespace CampaignManagement.Models
{
    public class CampaignStatusSummary
    {

        public int TotalCampaigns { get; set; }

        public int ActiveCampaigns { get; set; }

        public int UpcomingCampaigns { get; set; }

        public int ExpiredCampaigns { get; set; }
    }

    public class CampaignsByProduct
    {
        public Guid  ProductId { get; set; }

        public string ProductName { get; set; }

        public int CampaignCount { get; set; }
    }
}
