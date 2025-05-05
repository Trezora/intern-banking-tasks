using Banking.Domain.Entities;
using Banking.Domain.Services;

namespace Banking.Infrastructure.Logging;

public class ConsoleOnboardingLogger : IOnboardingLogger
{
    public void LogOnboardingSuccess(Customer customer, Guid accountNumber, decimal initialBalance)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ONBOARDING SUCCESS: " +
                          $"Customer {customer.FullName} ({customer.EmailAddress}) successfully onboarded. " +
                          $"Account {accountNumber} created with initial balance of {initialBalance:C}");
        Console.ResetColor();
    }

    public void LogOnboardingFailure(Customer customer, string reason)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ONBOARDING FAILED: " +
                          $"Customer {customer.FullName} ({customer.EmailAddress}) onboarding failed. " +
                          $"Reason: {reason}");
        Console.ResetColor();
    }
}