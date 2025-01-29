using Dntc.Attributes;

namespace DotnetEbpf.Core;

public static class BpfUtils
{
    private const string HelperHeaders = "vmlinux.h,<bpf/bpf_helpers.h>";

    /// <summary>
    /// Allows for calling `bpf_printk` with a single templated argument
    /// </summary>
    [NativeFunctionCall("bpf_printk", "vmlinux.h")]
    public static void Printk<T>(string message, T param1)
    {
    }
    
    /// <summary>
    /// Allows for calling `bpf_printk` with two templated arguments
    /// </summary>
    [NativeFunctionCall("bpf_printk", "vmlinux.h")]
    public static void Printk<T, U>(string message, T param1, U param2)
    {
    }

    /// <summary>
    /// Returns a 64-bit number where the high 32 bits are the bpf tgid and the low 32 bits
    /// are the bpf pid.
    /// </summary>
    [NativeFunctionCall("bpf_get_current_pid_tgid", HelperHeaders)]
    public static long GetCurrentPidTgid()
    {
        return 0;
    }

    [NativeFunctionCall("bpf_get_smp_processor_id", HelperHeaders)]
    public static UInt32 GetSmpProcessorId()
    {
        return 0;
    }

    [NativeFunctionCall("bpf_get_current_comm", HelperHeaders)]
    public static long GetCurrentComm<T>(ref T buffer, uint bufferSize)
    {
        return 0;
    }

    [NativeFunctionCall("bpf_get_stack", HelperHeaders)]
    public static long GetStack<TContext, TBuffer>(ref TContext context, ref TBuffer buffer, uint size, ulong flags)
    {
        return 0;
    }
}