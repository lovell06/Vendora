namespace Vendora.BuildingBlocks.Messaging.Abstractions;

public interface IEventTypeRegistry
{
    Type this[string typeName] { get; }
    string this[Type clrType] { get; }
    List<string> EventNames();
    List<Type> EventTypes();
}