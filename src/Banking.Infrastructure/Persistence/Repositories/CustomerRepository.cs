using Banking.Domain.Entities;
using Banking.Domain.Repositories;
using Banking.Domain.ValueObjects;
using Banking.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Persistence.Repositories;

public class CustomerRepository : ICustomerRespository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
    }

    public async Task<List<Customer>> GetAllCustomerAsync()
    {
        return await _context.Customers
            .Include(c => c.Accounts)
            .ToListAsync();
    }

    public async Task<Customer?> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Customers
            .Include(c => c.Accounts)
            .FirstOrDefaultAsync(c => c.CustomerId == new CustomerId(customerId));
    }

    public async Task<bool> CustomerExistsWithSameEmailAsync(Email email)
    {
        return await _context.Customers
            .AnyAsync(c => c.EmailAddress == email);
    }
}
