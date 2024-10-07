using FluentValidation;

namespace PGHub.Application.DTOs.User.Validators
{
    public class CreateUserDTOValidator : AbstractValidator<CreateUserDTO>
    {
        public CreateUserDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MinimumLength(5).WithMessage("Email must be at least 5 characters long.")
                .MaximumLength(100).WithMessage("Email must be less than 100 characters long.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Email must have a valid domain (e.g., .com, .ro, etc.).")
                .Matches(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$").WithMessage("Invalid email format.");  

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MinimumLength(3).WithMessage("First name must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("First name must be less than 100 characters long.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MinimumLength(3).WithMessage("Last name must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("Last name must be less than 100 characters long.");
        }
    }
}
