namespace Banking.Shared.Exceptions;

public abstract class BankingAppException : Exception
{
    protected BankingAppException(string message) : base(message)
    {
    }
}