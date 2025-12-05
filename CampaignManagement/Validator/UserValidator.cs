using FluentValidation;
using CampaignManagement.Models;
namespace CampaignManagement.Validator
{
    public class UserValidator:AbstractValidator<User>
    {
        public UserValidator() 
        {
            RuleFor(c => c.UserName).NotEmpty().WithMessage("Username Required")
                    .MinimumLength(3).WithMessage("Username must contain atleast 3 characters")
                    .MaximumLength(50).WithMessage("Username cannot exceed 50 chars")
                    .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username can only contains letters,numbers and underscore");

            RuleFor(c => c.Password).NotEmpty().WithMessage("Password Required")
                .MinimumLength(8).WithMessage("Password should contain atleast 8 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character (!@#$%^&* etc).");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid Email format");

        }
    }
}

