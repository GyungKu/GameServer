using MessagePack;

namespace server.Packet;

[MessagePackObject]
public class S_MatchAccept : PacketBase
{
    public override PacketType Type => PacketType.S_MatchAccept;
    
    [Key(1)]
    public bool Success { get; set; }
    
    [Key(2)]
    public int RoomId { get; set; }
}