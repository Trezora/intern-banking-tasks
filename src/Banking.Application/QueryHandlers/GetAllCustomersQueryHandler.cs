using Banking.Application.DTOs;
using Banking.Application.DTOs.Responses;
using Banking.Application.Mappings;
using Banking.Application.Queries;
using Banking.Domain.Repositories;
using Banking.Shared.OperationResults;
using MediatR;

namespace Banking.Application.QueryHandlers;

public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, Result<IEnumerable<CustomerDto>>>
{   
    private readonly ICustomerRespository _customerRespository;
    
    public GetAllCustomersQueryHandler(ICustomerRespository customerRespository)
    {
        _customerRespository = customerRespository;
    }
    public async Task<Result<IEnumerable<CustomerDto>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {        
        var customers = await _customerRespository.GetAllCustomerAsync();

        if (customers == null || !customers.Any())
            return Result<IEnumerable<CustomerDto>>.FailureWith("No customers found.");

        return Result<IEnumerable<CustomerDto>>.Success(customers.Select(c => c.ToDto()));
    }

}