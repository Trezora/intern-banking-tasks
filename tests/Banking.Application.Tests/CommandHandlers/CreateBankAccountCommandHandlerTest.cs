using System.Threading.Tasks;
using Banking.Application.CommandHandlers;
using Banking.Application.Commands;
using Banking.Application.DTOs;
using Banking.Application.DTOs.Requests;
using Banking.Application.DTOs.Responses;
using Banking.Application.Services;
using Banking.Domain.Entities;
using Banking.Domain.Repositories;
using Banking.Domain.Shared;
using Moq;

namespace Banking.Application.Tests;

public class CreateBankAccountCommandHandlerTest
{
    private readonly Mock<IBankAccountRepository> _bankAccountRepositoryMock;
    private readonly Mock<ICustomerService> _customerServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public CreateBankAccountCommandHandlerTest()
    {
        _bankAccountRepositoryMock = new();
        _customerServiceMock = new();
        _unitOfWorkMock = new();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenCustomerDoesNotExist()
    {
        // Arrange
        var request = new CreateBankAccountRequest
        {
            InitialDeposit = 100.00m,
            CustomerId = Guid.NewGuid(),
        };

        var command = new CreateBankAccountCommand(request);

        _customerServiceMock.Setup(
            x => x.TryGetCustomerByIdAsync(
                It.IsAny<Guid>())
        ).ReturnsAsync(Result<CustomerDto>.FailureWith("Customer ID", "Customer not found."));


        var handler = new CreateBankAccountCommandHandler(
            _customerServiceMock.Object,
            _bankAccountRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Act  
        Result<BankAccountCreateResponse> result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Customer ID.", result.Error.Code);
        Assert.Equal($"Customer with ID {request.CustomerId} not found.", result.Error.Description);

    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenCustomerExists()
    {
        // Arrange
        var request = new CreateBankAccountRequest
        {
            InitialDeposit = 100.00m,
            CustomerId = Guid.NewGuid(),
        };

        var command = new CreateBankAccountCommand(request);

        var customerDto = new CustomerDto
        {
            Id = Guid.NewGuid(),
            FullName = "user1",
            EmailAddress = "user1@gmail.com",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        _customerServiceMock.Setup(
                x => x.TryGetCustomerByIdAsync(
                    It.IsAny<Guid>())
            ).ReturnsAsync(Result<CustomerDto>.Success(customerDto));

        var handler = new CreateBankAccountCommandHandler(
            _customerServiceMock.Object,
            _bankAccountRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        // Act
        Result<BankAccountCreateResponse> result = await handler.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("", result.Error.Code);
        Assert.Null(result.Error.Description);
    }

    [Fact]
    public async Task Handle_Should_CallAddAsyncRepository_WhenCustomerExists()
    {
        // Arrange
        var request = new CreateBankAccountRequest
        {
            InitialDeposit = 100.00m,
            CustomerId = Guid.NewGuid(),
        };

        var command = new CreateBankAccountCommand(request);

        var customerDto = new CustomerDto
        {
            Id = Guid.NewGuid(),
            FullName = "user1",
            EmailAddress = "user1@gmail.com",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        _customerServiceMock.Setup(
                x => x.TryGetCustomerByIdAsync(
                    It.IsAny<Guid>())
            ).ReturnsAsync(Result<CustomerDto>.Success(customerDto));

        var handler = new CreateBankAccountCommandHandler(
            _customerServiceMock.Object,
            _bankAccountRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        // Act
        Result<BankAccountCreateResponse> result = await handler.Handle(command, default);

        // Assert
        _bankAccountRepositoryMock.Verify(
            x => x.AddAsync(It.Is<BankAccount>(m => m.AccountNumber == result.Value!.AccountNumber)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_NotCallUnitOfWork_WhenCustomerDoesNotExist()
    {
        // Arrange
        var request = new CreateBankAccountRequest
        {
            InitialDeposit = 100.00m,
            CustomerId = Guid.NewGuid(),
        };

        var command = new CreateBankAccountCommand(request);

        _customerServiceMock.Setup(
            x => x.TryGetCustomerByIdAsync(
                It.IsAny<Guid>())
        ).ReturnsAsync(Result<CustomerDto>.FailureWith("Customer ID", "Customer not found."));


        var handler = new CreateBankAccountCommandHandler(
            _customerServiceMock.Object,
            _bankAccountRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Act  
        Result<BankAccountCreateResponse> result = await handler.Handle(command, default);

        // Assert
        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }
}