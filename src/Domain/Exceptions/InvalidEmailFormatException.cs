using Shared.Exceptions;

namespace Domain.Exceptions;

public class InvalidEmailFormatException : BankingAppException
{
    public InvalidEmailFormatException() : base("email format is invalid.")
    {
    }
}