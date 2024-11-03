using System.ComponentModel.DataAnnotations;

namespace CampaignManagement.Models
{

    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }  // Product ID
        public string ProductName { get; set; }  // Product Name
        public string ProductDescription { get; set; }  // Product Description
        public bool IsActive { get; set; }  // Is Active
    }

}
