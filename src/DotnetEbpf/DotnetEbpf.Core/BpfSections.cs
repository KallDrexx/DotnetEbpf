namespace DotnetEbpf.Core;

/// <summary>
/// Contains attribute macro strings for different BPF related sections
/// </summary>
public static class BpfSections
{
    public const string License = "SEC(\"license\")";
    public const string SysEnterWrite = "SEC(\"tp/syscalls/sys_enter_write\")";
}