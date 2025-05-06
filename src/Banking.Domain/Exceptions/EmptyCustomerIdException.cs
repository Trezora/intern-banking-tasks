using Banking.Shared.Exceptions;

namespace Banking.Domain.Exceptions;

public class EmptyCustomerIdException : BankingAppException
{
    public EmptyCustomerIdException() : base("customer Id cannot be empty.")
    {
    }
}