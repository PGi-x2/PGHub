﻿using FluentValidation;

namespace PGHub.Common.DTOs.User.Validators
{
    public class CreateUserDTOValidator : AbstractValidator<CreateUserDTO>
    {
        public CreateUserDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
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
