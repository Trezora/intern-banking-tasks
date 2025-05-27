using Banking.Application.DTOs.Requests;
using Banking.Application.DTOs.Responses;
using Banking.Domain.Shared;
using MediatR;

namespace Banking.Application.Commands;

public class CreateBankAccountCommand : IRequest<Result<BankAccountCreateResponse>>
{
    public CreateBankAccountRequest BankAccountRequest { get; }

    public CreateBankAccountCommand(CreateBankAccountRequest bankAccountRequest)
    {
        BankAccountRequest = bankAccountRequest;
    }
    
}