using Dntc.Attributes;
using DotnetEbpf.Core;
using DotnetEbpf.Core.TranspilerPlugin;

namespace DotnetEbpf.Examples;

public static class Minimal
{
    [BpfLicense(BpfLicenseAttribute.DualBsdGpl)]
    public static string? License;
    
    [CustomFieldName("my_pid")]
    public static int MyPid;

    /// <summary>
    /// Prints the process that triggered the BPF trace point
    /// </summary>
    [CustomFunctionName("handle_tp")]
    [BpfSection("tp/syscalls/sys_enter_write")]
    public static int HandleTracePoint(ref CTypes.CVoid ctx)
    {
        var pid = (int)(BpfUtils.GetCurrentPidTgid() >> 32);
        if (pid == MyPid)
        {
            // only activate for the process that opened the bpf application
            BpfUtils.Printk("BPF triggered from PID %d.\n", pid);
        }

        return 0;
    }
}