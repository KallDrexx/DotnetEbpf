using Dntc.Attributes;
using DotnetEbpf.Core;

namespace DotnetEbpf.Examples;

public static class Minimal
{
    public static int MyPid;

    /// <summary>
    /// Prints the process that triggered the BPF trace point
    /// </summary>
    [CustomFunctionName("handle_tp")]
    [WithCAttribute(BpfSections.SysEnterWrite)]
    public static int HandleTracePoint(ref CTypes.CVoid ctx)
    {
        var pid = (int)(BpfUtils.GetCurrentPidTgid() >> 32);
        if (pid != MyPid)
        {
            // only activate for the process that opened the bpf application
            BpfUtils.Printk1("BPF triggered from PID %d.\n", pid);
        }

        return 0;
    }
}