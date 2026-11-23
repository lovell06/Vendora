namespace Vendora.Services.Inventory.Application.Abstractions.Persistence.Exceptions;

public sealed class ConcurrencyConflictException(string message, Exception? innerException = null) 
    : Exception(message, innerException);