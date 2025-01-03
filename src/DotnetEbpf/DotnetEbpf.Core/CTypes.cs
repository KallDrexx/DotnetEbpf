using Dntc.Attributes;

namespace DotnetEbpf.Core;

public static class CTypes
{
    /// <summary>
    /// Represents a void type in C. This is required because a function might take
    /// a void pointer, and this can't be represented with a c# `void`.
    /// </summary>
    [NativeType("void", null)]
    public struct CVoid { }
}