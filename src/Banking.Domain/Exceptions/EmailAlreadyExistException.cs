using Banking.Domain.Shared.Exceptions;

namespace Banking.Domain.Exceptions;

public class EmailAlreadyExistException : BankingAppException
{   
    private readonly string _email;
    public EmailAlreadyExistException(string email) : base($"A customer with email: '{email}' already exists.")
    {
        _email = email;
    }
}
