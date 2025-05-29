using Banking.Application.Abstractions.Caching;
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
    private readonly ICacheService _cacheService;

    public GetCustomerByIdQueryHandler(
        ICustomerRespository customerRespository,
        ICacheService cacheService)
    {
        _customerRespository = customerRespository;
        _cacheService = cacheService;
    }
    public async Task<Result<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"customer:{request.CustomerId}";

        var cachedCustomer = await _cacheService.GetAsync<CustomerDto>(cacheKey, cancellationToken);

        if (cachedCustomer != null)
        {
            return Result<CustomerDto>.Success(cachedCustomer);
        }

        var customer = await _customerRespository.GetByCustomerIdAsync(request.CustomerId);

        if (customer is null)
        {
            return Result<CustomerDto>.FailureWith("Customer", $"Customer with ID {request.CustomerId} was not found.");
        }

        var customerDto = customer.ToDto();

        await _cacheService.SetAsync(cacheKey, customerDto, cancellationToken);

        return Result<CustomerDto>.Success(customerDto);
    }
}