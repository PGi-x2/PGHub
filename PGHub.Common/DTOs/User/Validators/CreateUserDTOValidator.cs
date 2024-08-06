using FluentValidation;

namespace PGHub.Common.DTOs.User.Validators
{
    public class CreateUserDTOValidator : AbstractValidator<CreateUserDTO>
    {
        public CreateUserDTOValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.FirstName).NotEmpty().MinimumLength(3).MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MinimumLength(3).MaximumLength(100);
        }
    }
}
