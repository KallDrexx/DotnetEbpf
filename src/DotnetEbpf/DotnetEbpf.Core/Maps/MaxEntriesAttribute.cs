namespace DotnetEbpf.Core.Maps;

/// <summary>
/// Specifies the number of entries a map can hold
/// </summary>
[AttributeUsage(AttributeTargets.Struct)]
public class MaxEntriesAttribute(int count) : Attribute
{
    
}