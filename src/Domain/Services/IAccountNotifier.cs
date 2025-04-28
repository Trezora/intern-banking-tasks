namespace Domain.Services;

public interface IAccountNotifier
{
    void NotifySuccessfulDeposit(Guid accountNumber, string customerName, string email, decimal amount);
    void NotifySuccessfulWithdrawal(Guid accountNumber, string customerName, string email, decimal amount);
}