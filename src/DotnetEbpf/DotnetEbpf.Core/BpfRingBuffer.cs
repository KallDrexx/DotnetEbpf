using System.Runtime.InteropServices;
using Dntc.Attributes;

namespace DotnetEbpf.Core;

#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
public static class BpfRingBuffer
{
    [NativeFunctionCall("bpf_ringbuf_return", "<bpf/bpf_helpers.h>")]
    public static unsafe TItemType* Reserve<TItemType, TMapType>(ref TMapType map, int size, int flags)
    {
        return (TItemType*)Marshal.AllocHGlobal(sizeof(TItemType));
    }
}
#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
