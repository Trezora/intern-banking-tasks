using Banking.Application.DTOs;
using Banking.Domain.Shared;
using MediatR;

namespace Banking.Application.Queries;

public class GetCustomerByIdQuery : IRequest<Result<CustomerDto>>
{
    public Guid CustomerId { get; }

    public GetCustomerByIdQuery(Guid customerId)
    {
        CustomerId = customerId;
    }
}