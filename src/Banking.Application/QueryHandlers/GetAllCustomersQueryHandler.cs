using Banking.Application.Abstractions.Caching;
using Banking.Application.DTOs;
using Banking.Application.Mappings;
using Banking.Application.Queries;
using Banking.Domain.Repositories;
using Banking.Domain.Shared;
using MediatR;

namespace Banking.Application.QueryHandlers;

public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, Result<IEnumerable<CustomerDto>>>
{   
    private readonly ICustomerRespository _customerRespository;
    private readonly ICacheService _cacheService;

    public GetAllCustomersQueryHandler(
        ICustomerRespository customerRespository,
        ICacheService cacheService)
    {
        _customerRespository = customerRespository;
        _cacheService = cacheService;
    }
    public async Task<Result<IEnumerable<CustomerDto>>> Handle(
        GetAllCustomersQuery request,
        CancellationToken cancellationToken)
    {
        var cachedKey = $"customers";

        var cachedCustomers = await _cacheService.GetAsync<IEnumerable<CustomerDto>>(cachedKey, cancellationToken);

        if (cachedCustomers != null)
        {
            return Result<IEnumerable<CustomerDto>>.Success(cachedCustomers);
        }

        var customers = await _customerRespository.GetAllCustomerAsync();

        if (customers == null || !customers.Any())
            return Result<IEnumerable<CustomerDto>>.FailureWith("Customer.", "No customers found.");

        var customersDtos = customers.Select(c => c.ToDto());

        await _cacheService.SetAsync(cachedKey, customersDtos, cancellationToken);

        return Result<IEnumerable<CustomerDto>>.Success(customersDtos);
    }

}