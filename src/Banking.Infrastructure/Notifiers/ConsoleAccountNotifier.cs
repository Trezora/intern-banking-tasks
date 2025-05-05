using Banking.Domain.Services;

namespace Banking.Infrasturcture.Notifiers;

public class ConsoleAccountNotifier : IAccountNotifier
{
    public void NotifySuccessfulDeposit(Guid accountNumber, string customerName, string email, decimal amount)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] NOTIFICATION: Deposit confirmation sent to {customerName} ({email}) for deposit of {amount:C} to account {accountNumber}");
        Console.ResetColor();
    }

    public void NotifySuccessfulWithdrawal(Guid accountNumber, string customerName, string email, decimal amount)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] NOTIFICATION: Withdrawal confirmation sent to {customerName} ({email}) for withdrawal of {amount:C} from account {accountNumber}");
        Console.ResetColor();
    }
}
