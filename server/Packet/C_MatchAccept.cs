using MessagePack;

namespace server.Packet;

[MessagePackObject]
public class C_MatchAccept : PacketBase
{
    public override PacketType Type => PacketType.C_MatchAccept;
    
    [Key(1)]
    public bool IsAccept { get; set; }
    
    [Key(2)]
    public int MatchAcceptRoomId { get; set; }
}