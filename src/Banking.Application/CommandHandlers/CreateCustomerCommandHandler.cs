using Banking.Application.Commands;
using Banking.Application.DTOs.Responses;
using Banking.Application.Mappings;
using Banking.Domain.Repositories;
using Banking.Shared.OperationResults;
using MediatR;

namespace Banking.Application.CommandHandlers;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CustomerCreateResponse>>
{   
    private readonly ICustomerRespository _customerRespository;

    public CreateCustomerCommandHandler(ICustomerRespository customerRespository)
    {
        _customerRespository = customerRespository;
    }
    public async Task<Result<CustomerCreateResponse>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = request.CustomerRequest.ToCustomerEntity();

        await _customerRespository.AddAsync(customer);

        return Result<CustomerCreateResponse>.Success(customer.ToCustomerCreateResponse());
    }
}
