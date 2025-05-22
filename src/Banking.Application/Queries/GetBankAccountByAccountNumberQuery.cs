using Banking.Application.DTOs;
using Banking.Shared.OperationResults;
using MediatR;

namespace Banking.Application.Queries;

public class GetBankAccountByAccountNumberQuery : IRequest<Result<BankAccountDto>>
{
    public Guid AccountNumber { get; }

    public GetBankAccountByAccountNumberQuery(Guid accountNumber)
    {
        AccountNumber = accountNumber;
    }
}