using MessagePack;

namespace server.Packet;

[MessagePackObject]
public class S_Pick : PacketBase
{
    public override PacketType Type => PacketType.S_Pick;
    
    [Key(1)]
    public bool Success { get; set; }
    
    [Key(2)]
    public int GameRoomId { get; set; }
}