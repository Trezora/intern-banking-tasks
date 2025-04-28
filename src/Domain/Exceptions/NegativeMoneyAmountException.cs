using Shared.Exceptions;

namespace Domain.Exceptions;

public class NegativeMoneyAmountException : BankingAppException
{
    public NegativeMoneyAmountException() : base("money amount cannot be negative.")
    {
    }
}