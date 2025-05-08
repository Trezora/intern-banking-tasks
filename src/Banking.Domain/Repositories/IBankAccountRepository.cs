using Banking.Domain.Entities;

namespace Banking.Domain.Repositories;

public interface IBankAccountRepository
{
    Task<BankAccount?> GetByIdAsync(Guid id);
    Task AddAsync(BankAccount bankAccount);
}