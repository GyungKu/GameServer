using MessagePack;

namespace server.Packet;

[MessagePackObject]
public class S_Response : PacketBase
{
    public override PacketType Type => PacketType.S_Response;
    
    [Key(1)]
    public bool Success { get; set; }
}