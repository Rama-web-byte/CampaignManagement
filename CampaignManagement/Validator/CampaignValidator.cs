using FluentValidation;
using CampaignManagement.Models;
namespace CampaignManagement.Validator
{
    public class CampaignValidator:AbstractValidator<Campaign>
    { 
        public CampaignValidator() 
        {
            RuleFor(c => c.CampaignName)
                    .NotEmpty().WithMessage("Campaign Name Required")
                    .MinimumLength(3).WithMessage("Campaign name must be atleast 3 chars")
                    .MaximumLength(50).WithMessage("Campaign name cannot exceed 50 chars");

            RuleFor(c => c.StartDate)
                .NotEmpty().WithMessage("StartDate is Required")
                .GreaterThan(DateTime.Now).WithMessage("Start date must be future date.");

            RuleFor(c => c.EndDate)
                .NotEmpty().WithMessage("EndDate is Required")
                .GreaterThan(c => c.StartDate).WithMessage("End date must be after start date");

            RuleFor(c => c.ProductId)
                .NotEmpty().WithMessage("Product Id is required");



        
        }
    }
}
