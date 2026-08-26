using MediatR;
using Vendora.BuildingBlocks.Results;

namespace Vendora.BuildingBlocks.Cqrs;

public interface IQueryBase;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>, IQueryBase;