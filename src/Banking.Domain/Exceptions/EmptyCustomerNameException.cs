using Banking.Domain.Shared.Exceptions;

namespace Banking.Domain.Exceptions;

public class EmptyCustomerNameException : BankingAppException
{
    public EmptyCustomerNameException() : base("customer name cannot be empty.")
    {
    }
}