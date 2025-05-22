using Banking.Application.DTOs;
using Banking.Shared.OperationResults;
using MediatR;

namespace Banking.Application.Queries;

public class GetBankAccountsByCustomerIdQuery : IRequest<Result<IEnumerable<BankAccountDto>>>
{
    public Guid CustomerId { get; }

    public GetBankAccountsByCustomerIdQuery(Guid customerId)
    {
        CustomerId = customerId;
    }
}