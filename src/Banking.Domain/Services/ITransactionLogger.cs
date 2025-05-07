using Banking.Domain.ValueObjects;

namespace Banking.Domain.Services;

public interface ITransactionLogger
{
    void LogDeposit(Guid accountNumber, Money amount, Money newBalance);
    void LogWithdrawal(Guid accountNumber, Money amount, Money newBalance);
    void LogFailedTransaction(Guid accountNumber, string failureReason);
}