using Banking.Shared.Exceptions;

namespace Banking.Domain.Exceptions;

public class EmptyCustomerEmailException : BankingAppException
{
    public EmptyCustomerEmailException() : base("customer email cannot be empty.")
    {
    }
}