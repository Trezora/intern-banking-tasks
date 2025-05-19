using Banking.Domain.Entities;
using Banking.Domain.Repositories;
using Banking.Domain.ValueObjects;
using Banking.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Persistence.Repositories;

public class BankAccountRepository : IBankAccountRepository
{   
    private readonly AppDbContext _context;

    public BankAccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(BankAccount bankAccount)
    {
        await _context.BankAccounts.AddAsync(bankAccount);
        await _context.SaveChangesAsync(); 
    }
    
    public async Task<BankAccount?> GetByAccountNumberAsync(Guid accountNumber)
    {
        return await _context.BankAccounts.FirstOrDefaultAsync(b => b.AccountNumber == accountNumber);
    }
    
    public async Task<List<BankAccount>> GetAccountsByCustomerIdAsync(Guid customerId)
    {
        return await _context.BankAccounts
            .Where(b => b.CustomerId == new CustomerId(customerId))
            .ToListAsync();
    }
}