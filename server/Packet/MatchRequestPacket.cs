using MessagePack;

namespace server.Packet;

[MessagePackObject] 
public class MatchRequestPacket : PacketBase
{
    public override PacketType Type => PacketType.MatchRequest;
    
    [Key(1)] public int Mode { get; set; } // 1 = 1vs1, 2 = 2vs2, 3 = 3vs3
}