namespace DotnetEbpf.Core.Maps;

/// <summary>
/// Specifies the number of entries a map can hold
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class MaxEntriesAttribute(int count) : Attribute
{
    
}