using System.Threading.Tasks;
using Banking.Application.CommandHandlers;
using Banking.Application.Commands;
using Banking.Application.DTOs.Requests;
using Banking.Application.DTOs.Responses;
using Banking.Domain.Entities;
using Banking.Domain.Repositories;
using Banking.Domain.Shared;
using Banking.Domain.ValueObjects;
using Moq;

namespace Banking.Application.Tests;

public class CreateCustomerCommandHandlerTests
{
    private readonly Mock<ICustomerRespository> _customerRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public CreateCustomerCommandHandlerTests()
    {
        _customerRepositoryMock = new();
        _unitOfWorkMock = new();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenEmailIsNotUnique()
    {
        // Arrange
        var request = new CreateCustomerRequest
        {
            FullName = "user1",
            Email = "user1@gmail.com",
            DateOfBirth = new DateTime(1999, 5, 27)
        };

        var command = new CreateCustomerCommand(request);

        _customerRepositoryMock.Setup(
                x => x.CustomerExistsWithSameEmailAsync(
                    It.IsAny<Email>()))
            .ReturnsAsync(true);


        var handler = new CreateCustomerCommandHandler(
            _customerRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Act
        Result<CustomerCreateResponse> result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Email.", result.Error.Code);
        Assert.Equal("A customer with the same email already exists.", result.Error.Description);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenEmailIsUnique()
    {
        // Arrange
        var request = new CreateCustomerRequest
        {
            FullName = "user1",
            Email = "user1@gmail.com",
            DateOfBirth = new DateTime(1999, 5, 27)
        };

        var command = new CreateCustomerCommand(request);

        _customerRepositoryMock.Setup(
                x => x.CustomerExistsWithSameEmailAsync(
                    It.IsAny<Email>()))
            .ReturnsAsync(false);


        var handler = new CreateCustomerCommandHandler(
            _customerRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Act
        Result<CustomerCreateResponse> result = await handler.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("", result.Error.Code);
        Assert.Null(result.Error.Description);
    }

    [Fact]
    public async Task Handle_Should_CallAddAsyncRepository_WhenEmailIsUnique()
    {
        // Arrange
        var request = new CreateCustomerRequest
        {
            FullName = "user1",
            Email = "user1@gmail.com",
            DateOfBirth = new DateTime(1999, 5, 27)
        };

        var command = new CreateCustomerCommand(request);

        _customerRepositoryMock.Setup(
                x => x.CustomerExistsWithSameEmailAsync(
                    It.IsAny<Email>()))
            .ReturnsAsync(false);


        var handler = new CreateCustomerCommandHandler(
            _customerRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Act
        Result<CustomerCreateResponse> result = await handler.Handle(command, default);

        // Assert
        _customerRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Customer>(m => m.CustomerId == result.Value!.CustomerId)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_NotCallUnitOfWork_WhenEmailIsNotUnique()
    {
        // Arrange
        var request = new CreateCustomerRequest
        {
            FullName = "user1",
            Email = "user1@gmail.com",
            DateOfBirth = new DateTime(1999, 5, 27)
        };

        var command = new CreateCustomerCommand(request);

        _customerRepositoryMock.Setup(
                x => x.CustomerExistsWithSameEmailAsync(
                    It.IsAny<Email>()))
            .ReturnsAsync(true);


        var handler = new CreateCustomerCommandHandler(
            _customerRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Act
        Result<CustomerCreateResponse> result = await handler.Handle(command, default);

        // Assert
        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }
}

