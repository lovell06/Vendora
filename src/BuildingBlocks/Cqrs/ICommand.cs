using MediatR;
using Vendora.BuildingBlocks.Results;

namespace Vendora.BuildingBlocks.Cqrs;

public interface ICommandBase;

public interface ICommand : IRequest<Result>, ICommandBase;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, ICommandBase;