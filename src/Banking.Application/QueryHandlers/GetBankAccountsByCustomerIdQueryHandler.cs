using Banking.Application.Abstractions.Caching;
using Banking.Application.DTOs;
using Banking.Application.Mappings;
using Banking.Application.Queries;
using Banking.Domain.Repositories;
using Banking.Domain.Shared;
using MediatR;

namespace Banking.Application.QueryHandlers;

public class GetBankAccountsByCustomerIdQueryHandler
    : IRequestHandler<GetBankAccountsByCustomerIdQuery, Result<IEnumerable<BankAccountDto>>>
{   
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ICacheService _cacheService;
    public GetBankAccountsByCustomerIdQueryHandler(
        IBankAccountRepository bankAccountRepository,
        ICacheService cacheService)
    {
        _bankAccountRepository = bankAccountRepository;
        _cacheService = cacheService;
    }
    public async Task<Result<IEnumerable<BankAccountDto>>> Handle(
        GetBankAccountsByCustomerIdQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"bank accounts: {request.CustomerId}";

        var cachedBankAccounts = await _cacheService.GetAsync<IEnumerable<BankAccountDto>>(cacheKey, cancellationToken);

        if (cachedBankAccounts != null)
        {
            return Result<IEnumerable<BankAccountDto>>.Success(cachedBankAccounts);
        }

        var bankAccounts = await _bankAccountRepository.GetAccountsByCustomerIdAsync(request.CustomerId);

        if (bankAccounts == null || !bankAccounts.Any())
            return Result<IEnumerable<BankAccountDto>>.FailureWith("Bank account", "No bank accounts found.");

        var bankAccountsDtos = bankAccounts.Select(b => b.ToDto());

        await _cacheService.SetAsync(cacheKey, bankAccountsDtos);

        return Result<IEnumerable<BankAccountDto>>.Success(bankAccountsDtos);
    }
}
