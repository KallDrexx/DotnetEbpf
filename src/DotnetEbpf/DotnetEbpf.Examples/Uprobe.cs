using Dntc.Attributes;
using DotnetEbpf.Core;
using DotnetEbpf.Core.TranspilerPlugin;

namespace DotnetEbpf.Examples;

public static class Uprobe
{
    private const string TracingHeaders = "vmlinux.h,<bpf/bpf_tracing.h>";
    
    [BpfLicense(BpfLicenseAttribute.DualBsdGpl)]
    public static string? License;

    [BpfSection("uprobe")]
    [CustomDeclaration("int BPF_KPROBE(uprobe_add, int a, int b)", null, TracingHeaders)]
    public static int UProbeAdd(int a, int b)
    {
        BpfUtils.Printk("uprobed_add ENTRY: a = %d, b = %d", a, b);
        return 0;
    }

    [BpfSection("uretprobe")]
    [CustomDeclaration("int BPF_KRETPROBE(uretprobe_add, int ret)", null, TracingHeaders)]
    public static int URetProbeAdd(int ret)
    {
        BpfUtils.Printk("uprobed_add EXIT: return = %d", ret);
        return 0;
    }

    [BpfSection("uprobe//proc/self/exe:uprobed_sub")]
    [CustomDeclaration("int BPF_KPROBE(uprobe_sub, int a, int b)", null, TracingHeaders)]
    public static int UProbeSub(int a, int b)
    {
        BpfUtils.Printk("uprobed_sub ENTRY: a = %d, b = %d", a, b);
        return 0;
    }

    [BpfSection("uretprobe//proc/self/exe:uprobed_sub")]
    [CustomDeclaration("int BPF_KRETPROBE(uretprobe_sub, int ret)", null, TracingHeaders)]
    public static int URetProbeSub(int ret)
    {
        BpfUtils.Printk("uprobed_sub EXIT: return = %d", ret);
        return 0;
    }
}