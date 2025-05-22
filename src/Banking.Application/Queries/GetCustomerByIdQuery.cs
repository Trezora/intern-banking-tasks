using Banking.Application.DTOs;
using Banking.Application.DTOs.Responses;
using Banking.Shared.OperationResults;
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