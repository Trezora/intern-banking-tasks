using Banking.Domain.Services;
using Banking.Domain.ValueObjects;

namespace Banking.Infrasturcture.Notifiers;

public class ConsoleAccountNotifier : IAccountNotifier
{
    public void NotifySuccessfulDeposit(Guid accountNumber, CustomerId customerId, Money amount)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] NOTIFICATION: Deposit confirmation sent to {customerId} for deposit of {amount.Value:C} to account {accountNumber}");
        Console.ResetColor();
    }

    public void NotifySuccessfulWithdrawal(Guid accountNumber, CustomerId customerId, Money amount)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] NOTIFICATION: Withdrawal confirmation sent to {customerId} or withdrawal of {amount.Value:C} from account {accountNumber}");
        Console.ResetColor();
    }
}
