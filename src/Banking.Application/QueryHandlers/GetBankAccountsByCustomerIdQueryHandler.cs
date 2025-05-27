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
    public GetBankAccountsByCustomerIdQueryHandler(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }
    public async Task<Result<IEnumerable<BankAccountDto>>> Handle(GetBankAccountsByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var bankAccounts = await _bankAccountRepository.GetAccountsByCustomerIdAsync(request.CustomerId);

        if (bankAccounts == null || !bankAccounts.Any())
            return Result<IEnumerable<BankAccountDto>>.FailureWith("No bank accounts found.");

        return Result<IEnumerable<BankAccountDto>>.Success(bankAccounts.Select(b => b.ToDto()));
    }
}
