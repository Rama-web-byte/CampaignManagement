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
                .Must(BeTodayOrFutureDate).WithMessage("StartDate cannot be in the past");

            RuleFor(c => c.EndDate)
                .NotEmpty().WithMessage("EndDate is Required")
                .GreaterThan(c => c.StartDate).WithMessage("End date must be after start date")
                .Must((c, endDate) => BeWithin12Months(endDate)).WithMessage("Enddate cannot be more than 12 months from today");

            RuleFor(c => c.ProductId)
                .NotEmpty().WithMessage("Product Id is required");

            //RuleFor(c => c).Custom((c, context) =>
            //{
            //    if (c.CampaignId != null)

            //    {
            //        var today = DateTime.Today;
            //        string status;
            //        if (c.EndDate < today)
            //        {
            //            status = "Expired";
            //        }
            //        else if (c.StartDate <= today && c.EndDate >= today)
            //        {
            //            status = "Ongoing";
            //        }
            //        else
            //        {
            //            status = "Future";
            //        }

            //        if ((status == "Ongoing" || status == "Expired") && c.StartDate != c.StartDate)
            //        {
            //            context.AddFailure("StartDate", "Cannot change StartDate for ongoing or expired campaigns");
            //        }

            //        if (status == "Ongoing" && c.EndDate < today)
            //        {
            //            context.AddFailure("EndDate", "EndDate cannot be in the past for ongoing campaigns");
            //        }
            //    }
            //}

            //);


        
        }

        private bool BeTodayOrFutureDate(DateTime startDate)
        {
            var today = DateTime.Today;
            return startDate.Date >= today;

        }

        private bool BeWithin12Months(DateTime endDate)
        {
            var maxDate = DateTime.Today.AddMonths(12);
            return endDate.Date <= maxDate;
        }
    }
}
