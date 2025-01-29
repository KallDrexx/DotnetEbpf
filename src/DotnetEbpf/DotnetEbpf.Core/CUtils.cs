using Dntc.Attributes;

namespace DotnetEbpf.Core;

public static class CUtils
{
    [NativeFunctionCall("sizeof", null)]
    public static int SizeOf(Type type)
    {
        return 0;
    }
}