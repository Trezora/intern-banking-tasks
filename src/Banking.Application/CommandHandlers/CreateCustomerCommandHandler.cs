using Banking.Application.Commands;
using Banking.Application.DTOs.Responses;
using Banking.Application.Mappings;
using Banking.Domain.Repositories;
using Banking.Domain.Shared;
using Banking.Domain.ValueObjects;
using MediatR;

namespace Banking.Application.CommandHandlers;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CustomerCreateResponse>>
{
    private readonly ICustomerRespository _customerRespository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerCommandHandler(ICustomerRespository customerRespository, IUnitOfWork unitOfWork)
    {
        _customerRespository = customerRespository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<CustomerCreateResponse>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var exists = await _customerRespository.CustomerExistsWithSameEmailAsync(new Email (request.CustomerRequest.Email));

        if (exists)
        {
            return Result<CustomerCreateResponse>.FailureWith("A customer with the same email already exists.");
        }

        var customer = request.CustomerRequest.ToCustomerEntity();

        await _customerRespository.AddAsync(customer);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CustomerCreateResponse>.Success(customer.ToCustomerCreateResponse());
    }
}
