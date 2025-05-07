
using Banking.Domain.Services;
using Banking.Domain.ValueObjects;

namespace Banking.Infrasturcture.Logging;

public class ConsoleTransactionLogger : ITransactionLogger
{
    public void LogDeposit(Guid accountNumber, Money amount, Money newBalance)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] DEPOSIT: Account {accountNumber} deposited {amount:C}. New balance: {newBalance:C}");
        Console.ResetColor();
    }

    public void LogWithdrawal(Guid accountNumber, Money amount, Money newBalance)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] WITHDRAWAL: Account {accountNumber} withdrew {amount:C}. New balance: {newBalance:C}");
        Console.ResetColor();
    }

    public void LogFailedTransaction(Guid accountNumber, string failureReason)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] FAILED: Account {accountNumber} - {failureReason}");
        Console.ResetColor();
    }
}
