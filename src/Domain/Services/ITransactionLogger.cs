namespace Domain.Services;

public interface ITransactionLogger
{
    void LogDeposit(Guid accountNumber, decimal amount, decimal newBalance);
    void LogWithdrawal(Guid accountNumber, decimal amount, decimal newBalance);
    void LogFailedTransaction(Guid accountNumber, string failureReason);
}