using System.Runtime.InteropServices;
using Dntc.Attributes;

namespace DotnetEbpf.Core;

#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
public static class BpfRingBuffer
{
    [NativeFunctionCall("bpf_ringbuf_reserve", "<bpf/bpf_helpers.h>")]
    public static unsafe TItemType* Reserve<TItemType, TMapType>(ref TMapType map, int size, uint flags)
    {
        return (TItemType*)Marshal.AllocHGlobal(sizeof(TItemType));
    }

    [NativeFunctionCall("bfp_ringbuf_submit", "<bpf/bpf_helpers.h>")]
    public static unsafe void Submit<T>(T* data, uint flags)
    {

    }
}
#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
