using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Identity.Application.Features.Authentication.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FullName,
    string PhoneNumber) : ICommand;