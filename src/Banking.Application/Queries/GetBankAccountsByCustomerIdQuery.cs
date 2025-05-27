using Banking.Application.DTOs;
using Banking.Domain.Shared;
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