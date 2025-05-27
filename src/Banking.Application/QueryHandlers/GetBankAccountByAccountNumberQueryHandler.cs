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

    public GetBankAccountByAccountNumberQueryHandler(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRespository = bankAccountRepository;
    }
    public async Task<Result<BankAccountDto>> Handle(GetBankAccountByAccountNumberQuery request, CancellationToken cancellationToken)
    {
        var bankAccount = await _bankAccountRespository.GetByAccountNumberAsync(request.AccountNumber);

        if (bankAccount == null)
            return Result<BankAccountDto>
                .FailureWith("Bank account.", $"Bank account with account number {request.AccountNumber} not found.");

        return Result<BankAccountDto>.Success(bankAccount.ToDto());
    }
}
