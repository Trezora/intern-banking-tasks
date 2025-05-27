using Banking.Application.DTOs;
using Banking.Domain.Shared;
using MediatR;

namespace Banking.Application.Queries;

public class GetAllCustomersQuery : IRequest<Result<IEnumerable<CustomerDto>>> {}