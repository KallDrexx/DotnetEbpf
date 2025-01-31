namespace DotnetEbpf.Core.Maps;

public static class MapTypeUtils
{
    /// <summary>
    /// Converts the map type enumeration value into the eBPF value
    /// </summary>
    public static string ToAttributeString(this MapType type)
    {
        switch (type)
        {
            case MapType.RingBuffer:
                return "BPF_MAP_TYPE_RINGBUF";
            
            default:
                throw new NotSupportedException(type.ToString());
        }
    }
}