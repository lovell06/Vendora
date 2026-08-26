using MediatR;
using Vendora.BuildingBlocks.Results;

namespace Vendora.BuildingBlocks.Cqrs;

public interface IQueryHandler<in TQuery, TResponse> 
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;