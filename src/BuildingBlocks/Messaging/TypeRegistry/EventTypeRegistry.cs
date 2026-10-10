namespace Vendora.BuildingBlocks.Messaging.TypeRegistry;

public sealed class EventTypeRegistry : IEventTypeRegistry
{
    private readonly Dictionary<string, Type> _nameToType = new();
    private readonly Dictionary<Type, string> _typeToName = new();

    public Type this[string typeName] => _nameToType[typeName];

    public string this[Type clrType] => _typeToName[clrType];

    public void Add(string name, Type type)
    {
        _nameToType.Add(name, type);
        _typeToName.Add(type, name);
    }

    public void Add(Type type, string name)
    {
        _nameToType.Add(name, type);
        _typeToName.Add(type, name);
    }

    public List<string> EventNames()
    {
        return [.. _nameToType.Select(pair => pair.Key)];
    }

    public List<Type> EventTypes()
    {
        return [.. _nameToType.Select(pair => pair.Value)];
    }
}