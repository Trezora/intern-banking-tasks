using Banking.Application.DTOs;
using Banking.Application.Mappings;
using Banking.Application.Queries;
using Banking.Domain.Repositories;
using Banking.Domain.Shared;
using MediatR;

namespace Banking.Application.QueryHandlers;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Result<CustomerDto>>
{   
    private readonly ICustomerRespository _customerRespository;

    public GetCustomerByIdQueryHandler(ICustomerRespository customerRespository)
    {
        _customerRespository = customerRespository;
    }
    public async Task<Result<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRespository.GetByCustomerIdAsync(request.CustomerId);

        if (customer is null)
        {
            return Result<CustomerDto>.FailureWith("Customer.", $"Customer with ID {request.CustomerId} was not found.");
        }

        return Result<CustomerDto>.Success(customer.ToDto());
    }
}