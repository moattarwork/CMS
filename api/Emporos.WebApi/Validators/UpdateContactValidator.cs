using System;
using FluentValidation;
using Emporos.Core.Dto;

namespace Emporos.WebApi.Validators
{
    public class UpdateContactValidator : AbstractValidator<ContactRequestDto>
    {
        public UpdateContactValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(m => m.Surname).NotEmpty().MaximumLength(100) ;
            RuleFor(m => m.DateOfBirth).NotEmpty().Must(m => m < DateTime.Today);
            RuleFor(m => m.Email).NotEmpty().EmailAddress();
        }
    }
}