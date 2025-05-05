using Banking.Shared.Exceptions;

namespace Banking.Domain.Exceptions;

public class InvalidEmailFormatException : BankingAppException
{
    public InvalidEmailFormatException() : base("email format is invalid.")
    {
    }
}