using Banking.Application.Abstractions.Caching;
using Banking.Application.DTOs;
using Banking.Application.Mappings;
using Banking.Application.Queries;
using Banking.Domain.Repositories;
using Banking.Domain.Shared;
using MediatR;

namespace Banking.Application.QueryHandlers;

public class GetBankAccountByAccountNumberQueryHandler
    : IRequestHandler<GetBankAccountByAccountNumberQuery, Result<BankAccountDto>>
{
    private readonly IBankAccountRepository _bankAccountRespository;
    private readonly ICacheService _cacheService;

    public GetBankAccountByAccountNumberQueryHandler(
        IBankAccountRepository bankAccountRepository,
        ICacheService cacheService)
    {
        _bankAccountRespository = bankAccountRepository;
        _cacheService = cacheService;
    }
    public async Task<Result<BankAccountDto>> Handle(GetBankAccountByAccountNumberQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"bank account: {request.AccountNumber}";

        var cachedBankAccount = await _cacheService.GetAsync<BankAccountDto>(cacheKey, cancellationToken);

        if (cachedBankAccount != null)
        {
            return Result<BankAccountDto>.Success(cachedBankAccount);
        }

        var bankAccount = await _bankAccountRespository.GetByAccountNumberAsync(request.AccountNumber);

        if (bankAccount == null)
            return Result<BankAccountDto>
                .FailureWith("Bank account.", $"Bank account with account number {request.AccountNumber} not found.");

        var bankAccountDto = bankAccount.ToDto();

        await _cacheService.SetAsync(cacheKey, bankAccountDto, cancellationToken);

        return Result<BankAccountDto>.Success(bankAccount.ToDto());
    }
}
