using Banking.Domain.ValueObjects;

namespace Banking.Domain.Services;

public interface IAccountNotifier
{
    void NotifySuccessfulDeposit(Guid accountNumber, CustomerId customerId, Money amount);
    void NotifySuccessfulWithdrawal(Guid accountNumber, CustomerId customerId, Money amount);
}