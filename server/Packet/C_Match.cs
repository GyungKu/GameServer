using MessagePack;

namespace server.Packet;

[MessagePackObject] 
public class C_Match : PacketBase
{
    public override PacketType Type => PacketType.C_Match;
    
    [Key(1)] public int Mode { get; set; } // 1 = 1vs1, 2 = 2vs2, 3 = 3vs3
}