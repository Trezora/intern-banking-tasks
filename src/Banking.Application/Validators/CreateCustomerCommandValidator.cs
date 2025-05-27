using Banking.Application.Commands;
using FluentValidation;

namespace Banking.Application.Validators;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerRequest.Email)
            .NotEmpty()
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.CustomerRequest.FullName)
            .NotEmpty().WithMessage("Full name is required.");

        RuleFor(x => x.CustomerRequest.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.");
    }
}