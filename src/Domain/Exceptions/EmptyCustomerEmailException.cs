using Shared.Exceptions;

namespace Domain.Exceptions;

public class EmptyCustomerEmailException : BankingAppException
{
    public EmptyCustomerEmailException() : base("customer email cannot be empty.")
    {
    }
}