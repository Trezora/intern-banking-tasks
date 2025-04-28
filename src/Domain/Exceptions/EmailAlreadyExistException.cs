using Shared.Exceptions;

namespace Domain.Exceptions;

public class EmailAlreadyExistException : BankingAppException
{   
    private readonly string _email;
    public EmailAlreadyExistException(string email) : base($"A customer with email: '{email}' already exists.")
    {
        _email = email;
    }
}
