using System.ComponentModel.DataAnnotations;

namespace CampaignManagement.Models
{
    

    public class Campaign
    {
        [Key]
        public Guid CampaignId { get; set; }  // Campaign ID

  
        public string CampaignsName { get; set; }  // Campaign Name

    
        public DateTime StartDate { get; set; }  // Start Date

      
        public DateTime EndDate { get; set; }  // End Date
        public bool IsActive { get; set; }  // Is Active

        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }
    }

}
