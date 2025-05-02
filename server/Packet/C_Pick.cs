using MessagePack;

namespace server.Packet;

[MessagePackObject]
public class C_Pick : PacketBase
{
    public override PacketType Type => PacketType.C_Pick;
    
    [Key(1)]
    public int CharacterId { get; set; }
    
    [Key(2)]
    public int UserId { get; set; }
    
    [Key(3)]
    public int GameRoomId { get; set; }
}