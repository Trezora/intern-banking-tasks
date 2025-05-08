using Banking.Domain.Entities;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Persistence.Repositories;

public class InMemoryBankAccountRepository : IBankAccountRepository
{   
    private readonly Dictionary<Guid, BankAccount> _bankAccounts = [];
    public Task AddAsync(BankAccount bankAccount)
    {
        _bankAccounts[bankAccount.CustomerId] = bankAccount;
        return Task.CompletedTask;
    }

    public Task<BankAccount?> GetByIdAsync(Guid id)
    {
        _bankAccounts.TryGetValue(id, out var bankAccount);
        return Task.FromResult(bankAccount);
    }
}
