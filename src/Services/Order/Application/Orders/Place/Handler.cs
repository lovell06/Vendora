namespace Vendora.Services.Order.Application.Orders.Place;

public sealed class Handler : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}
