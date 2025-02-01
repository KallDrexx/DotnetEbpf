using Dntc.Attributes;
using DotnetEbpf.Core;
using DotnetEbpf.Core.Maps;

namespace DotnetEbpf.Examples;

public static class Profile
{
    private const int TaskCommLen = 16;
    private const int MaxStackDepth = 128;
    
    [BpfLicense(BpfLicenseAttribute.DualBsdGpl)]
    public static string? License;

    [NativeType("stacktrace_event", "../profile.h")]
    public struct StacktraceEvent
    {
        [CustomFieldName("pid")]
        public uint Pid;
        
        [CustomFieldName("cpu_id")]
        public uint CpuId;
        
        [CustomFieldName("comm")]
        [StaticallySizedArray(TaskCommLen)]
        public char[] Comm;
        
        [CustomFieldName("kstack_sz")]
        public int KStackSize;
        
        [CustomFieldName("ustack_sz")]
        public int UStackSize;
        
        [CustomFieldName("kstack")]
        [StaticallySizedArray(MaxStackDepth)]
        public ulong[] KStack;
        
        [CustomFieldName("ustack")]
        [StaticallySizedArray(MaxStackDepth)]
        public ulong[] UStack;
    }

    [MapType(MapType.RingBuffer)]
    [MaxEntries(256 * 1024)]
    public static BpfMap Events;

    [BpfSection("perf_event")]
    public static int RunProfile(ref CTypes.CVoid context)
    {
        return 0;
    }
}