using Banking.Application.DTOs;
using Banking.Application.DTOs.Requests;
using Banking.Application.DTOs.Responses;
using Banking.Domain.Entities;
using Banking.Domain.ValueObjects;

namespace Banking.Application.Mappings;

public static class BankAccountMapper
{
    public static BankAccountCreateResponse ToResponse(this BankAccount bankAccount)
    {   
        return new BankAccountCreateResponse
        {
            AccountNumber = bankAccount.AccountNumber,
            Balance = bankAccount.GetBalance().Value,
            CustomerId = bankAccount.CustomerId.Value
        };
    }

    public static BankAccount ToBankAccountEntity(this CreateBankAccountRequest request)
    {
        return BankAccount.Create(
            Guid.NewGuid(), 
            new CustomerId (request.CustomerId),
            new Money(request.InitialDeposit)
        );
    }

    public static BankAccountDto ToDto(this BankAccount bankAccount)
    {
        return new BankAccountDto{
            AccountNumber = bankAccount.AccountNumber, 
            Balance = bankAccount.GetBalance().Value,
        };
    }
}