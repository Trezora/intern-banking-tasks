using Banking.Application.Abstractions.Caching;
using Banking.Application.DTOs;
using Banking.Application.Queries;
using Banking.Domain.Entities;
using Banking.Domain.Repositories;
using Banking.Domain.ValueObjects;
using Moq;

namespace Banking.Application.QueryHandlers;

public class GetCustomerByIdQueryHandlerTest
{
    private readonly Mock<ICustomerRespository> _customerRepositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;

    public GetCustomerByIdQueryHandlerTest()
    {
        _customerRepositoryMock = new();
        _cacheServiceMock = new();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenCustomerDoesNotExist_AndCacheIsEmpty()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetCustomerByIdQuery(id);

        // no data in cache
        _cacheServiceMock.Setup(x => x.GetAsync<CustomerDto>(It.IsAny<string>(), default))
            .ReturnsAsync((CustomerDto?)null);

        // no data in repository
        _customerRepositoryMock.Setup(x => x.GetByCustomerIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Customer?)null);

        var handler = new GetCustomerByIdQueryHandler(
            _customerRepositoryMock.Object,
            _cacheServiceMock.Object);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Customer", result.Error.Code);
        Assert.Equal($"Customer with ID {id} was not found.", result.Error.Description);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_FromCache_WhenCacheHasCustomer()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetCustomerByIdQuery(id);

        var customerDto = new CustomerDto
        {
            Id = id,
            FullName = "user1",
            EmailAddress = "user1@gmail.com",
            DateOfBirth = new DateTime(1999, 1, 1)
        };

        _cacheServiceMock.Setup(x => x.GetAsync<CustomerDto>(It.IsAny<string>(), default))
            .ReturnsAsync(customerDto);

        var handler = new GetCustomerByIdQueryHandler(
            _customerRepositoryMock.Object,
            _cacheServiceMock.Object);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(customerDto.Id, result.Value.Id);
        Assert.Equal(customerDto.FullName, result.Value.FullName);
        Assert.Equal(customerDto.EmailAddress, result.Value.EmailAddress);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_AndSetCache_WhenCustomerExistsInRepository()
    {
        // Arrange
        var customerId = new CustomerId(Guid.NewGuid());
        var fullName = new Name("user1");
        var email = new Email("user1@gmail.com");
        var dateOfBirth = new DateTime(1999, 1, 1);

        var customer = Customer.Create(customerId, fullName, email, dateOfBirth);

        var query = new GetCustomerByIdQuery(customerId);

        // nothing in cache initially
        _cacheServiceMock.Setup(x => x.GetAsync<CustomerDto>(It.IsAny<string>(), default))
            .ReturnsAsync((CustomerDto?)null);

        // found in repository
        _customerRepositoryMock.Setup(x => x.GetByCustomerIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(customer);

        var handler = new GetCustomerByIdQueryHandler(
            _customerRepositoryMock.Object,
            _cacheServiceMock.Object);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(customerId.Value, result.Value.Id);
        Assert.Equal(fullName.Value, result.Value.FullName);
        Assert.Equal(email.Value, result.Value.EmailAddress);

        // cache should have been updated
        _cacheServiceMock.Verify(x => x.SetAsync(
            $"customer:{customerId.Value}",
            It.Is<CustomerDto>(dto => dto.Id == customerId.Value),
            default), Times.Once);
    }
}
