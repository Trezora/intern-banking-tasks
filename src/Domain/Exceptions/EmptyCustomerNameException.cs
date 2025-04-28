using Shared.Exceptions;

namespace Domain.Exceptions;

public class EmptyCustomerNameException : BankingAppException
{
    public EmptyCustomerNameException() : base("customer name cannot be empty.")
    {
    }
}