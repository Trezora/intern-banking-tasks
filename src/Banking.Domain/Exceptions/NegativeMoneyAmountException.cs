using Banking.Shared.Exceptions;

namespace Banking.Domain.Exceptions;

public class NegativeMoneyAmountException : BankingAppException
{
    public NegativeMoneyAmountException() : base("money amount cannot be negative.")
    {
    }
}