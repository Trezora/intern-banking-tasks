using Banking.Domain.Entities;

namespace Banking.Domain.Services;

public interface IOnboardingLogger
{
    void LogOnboardingSuccess(Customer customer, Guid accountNumber, decimal initialBalance);
    void LogOnboardingFailure(Customer customer, string reason);
}