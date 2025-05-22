using Banking.Application.DTOs;
using Banking.Shared.OperationResults;
using MediatR;

namespace Banking.Application.Queries;

public class GetAllCustomersQuery : IRequest<Result<IEnumerable<CustomerDto>>> {}