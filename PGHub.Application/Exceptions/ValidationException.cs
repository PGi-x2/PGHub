﻿using FluentValidation.Results;

namespace PGHub.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public ValidationException(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Errors.Add(error.ErrorMessage);
            }
        }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
