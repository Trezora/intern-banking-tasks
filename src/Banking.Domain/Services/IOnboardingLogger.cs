using Banking.Domain.Entities;
using Banking.Domain.ValueObjects;

namespace Banking.Domain.Services;

public interface IOnboardingLogger
{
    void LogOnboardingSuccess(Customer customer, Guid accountNumber, Money initialBalance);
    void LogOnboardingFailure(Customer customer, string reason);
}