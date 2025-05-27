using System.Threading.Tasks;
using Banking.Application.Queries;
using Banking.Domain.Entities;
using Banking.Domain.Repositories;
using Banking.Domain.Shared;
using Banking.Domain.ValueObjects;
using Moq;

namespace Banking.Application.QueryHandlers;

public class GetCustomerByIdQueryHandlerTest
{
    private readonly Mock<ICustomerRespository> _customerRepositoryMock;

    public GetCustomerByIdQueryHandlerTest()
    {
        _customerRepositoryMock = new();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenCustomerDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        var query = new GetCustomerByIdQuery(id);

        _customerRepositoryMock.Setup(
                x => x.GetByCustomerIdAsync(
                    It.IsAny<Guid>()))
            .ReturnsAsync((Customer?)null);

        var handler = new GetCustomerByIdQueryHandler(_customerRepositoryMock.Object);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Customer.", result.Error.Code);
        Assert.Equal($"Customer with ID {id} was not found.", result.Error.Description);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenCustomerExists()
    {
        // Arrange
        var customerId = new CustomerId(Guid.NewGuid());
        var fullName = new Name("user1");
        var email = new Email("user1@gmail.com");
        var dateOfBirth = new DateTime(1999, 1, 1);

        var customer = Customer.Create(
            customerId,
            fullName,
            email,
            dateOfBirth
        );

        var query = new GetCustomerByIdQuery(customerId);

        _customerRepositoryMock.Setup(
                x => x.GetByCustomerIdAsync(
                    It.IsAny<Guid>()))
            .ReturnsAsync(customer);

        var handler = new GetCustomerByIdQueryHandler(_customerRepositoryMock.Object);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("", result.Error.Code);
        Assert.Null(result.Error.Description);
        Assert.Equal(customerId.Value, result.Value.Id);
        Assert.Equal("user1", result.Value.FullName);
        Assert.Equal("user1@gmail.com", result.Value.EmailAddress);
    }
}

