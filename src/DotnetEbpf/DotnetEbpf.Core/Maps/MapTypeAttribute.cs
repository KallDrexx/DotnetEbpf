namespace DotnetEbpf.Core.Maps;

/// <summary>
/// Defines the type of map that should be created
/// </summary>
[AttributeUsage(AttributeTargets.Struct)]
public class MapTypeAttribute(MapType type) : Attribute
{
    
}