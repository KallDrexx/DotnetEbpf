using Dntc.Attributes;

namespace DotnetEbpf.Core;

public static class BpfUtils
{
    /// <summary>
    /// Allows for calling `bpf_printk` with a single templated argument
    /// </summary>
    [NativeFunctionCall("bpf_printk", "<linux/bpf.h>")]
    public static void Printk<T>(string message, T param1)
    {
    }

    /// <summary>
    /// Returns a 64-bit number where the high 32 bits are the bpf tgid and the low 32 bits
    /// are the bpf pid.
    /// </summary>
    [NativeFunctionCall("bpf_get_current_pid_tgid", "<linux/bpf.h>,<bpf/bpf_helpers.h>")]
    public static long GetCurrentPidTgid()
    {
        return 0;
    }
}