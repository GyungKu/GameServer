using MessagePack;

namespace server.Packet;

[MessagePackObject]
public class S_PickStart : PacketBase
{
    public override PacketType Type => PacketType.S_PickStart;
    
    [Key(1)]
    public int GameRoomId { get; set; }

}