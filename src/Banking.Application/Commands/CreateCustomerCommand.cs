using Banking.Application.DTOs.Requests;
using Banking.Application.DTOs.Responses;
using Banking.Domain.Shared;
using MediatR;

namespace Banking.Application.Commands;

public class CreateCustomerCommand : IRequest<Result<CustomerCreateResponse>>
{
    public CreateCustomerRequest CustomerRequest { get; }

    public CreateCustomerCommand(CreateCustomerRequest customerRequest)
    {
        CustomerRequest = customerRequest;
    }
}