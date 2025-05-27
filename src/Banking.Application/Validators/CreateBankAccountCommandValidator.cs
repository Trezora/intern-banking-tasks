using System.Data;
using Banking.Application.Commands;
using FluentValidation;

namespace Banking.Application.Validators;

public class CrateBankAccountCommandValidator :  AbstractValidator<CreateBankAccountCommand>
{
    public CrateBankAccountCommandValidator()
    {
        RuleFor(x => x.BankAccountRequest.CustomerId)
            .NotEmpty().WithMessage("Customer id is required.")
            .Must(id => Guid.TryParse(id.ToString(), out _))
            .WithMessage("Customer id must be a valid GUID.");

        RuleFor(x => x.BankAccountRequest.InitialDeposit)
            .GreaterThanOrEqualTo(100.00m).WithMessage("Initial deposit must be >= 100");
    }
}