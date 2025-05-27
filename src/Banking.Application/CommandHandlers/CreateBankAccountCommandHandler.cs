using Banking.Application.Commands;
using Banking.Application.DTOs.Responses;
using Banking.Application.Mappings;
using Banking.Application.Services;
using Banking.Domain.Repositories;
using Banking.Domain.Shared;
using MediatR;

namespace Banking.Application.CommandHandlers;

public class CreateBankAccountCommandHandler : IRequestHandler<CreateBankAccountCommand, Result<BankAccountCreateResponse>>
{
    private readonly ICustomerService _customerService;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBankAccountCommandHandler(
        ICustomerService customerService,
        IBankAccountRepository bankAccountRepository,
        IUnitOfWork unitOfWork)
    {
        _customerService = customerService;
        _bankAccountRepository = bankAccountRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<BankAccountCreateResponse>> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
    {
        var result = await _customerService.TryGetCustomerByIdAsync(request.BankAccountRequest.CustomerId);

        if (!result.IsSuccess || result.Value == null)
            return Result<BankAccountCreateResponse>
                .FailureWith($"Customer with ID {request.BankAccountRequest.CustomerId} not found.");

        var bankAccount = request.BankAccountRequest.ToBankAccountEntity();

        await _bankAccountRepository.AddAsync(bankAccount);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<BankAccountCreateResponse>.Success(bankAccount.ToResponse());
    }
}