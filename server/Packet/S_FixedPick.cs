using MessagePack;
using server.Data;

namespace server.Packet;

[MessagePackObject]
public class S_FixedPick : PacketBase
{
    public override PacketType Type => PacketType.S_FixedPick;
    
    [Key(1)]
    public int PickedCharacterId { get; set; }
    
    [Key(2)]
    public Team Team { get; set; }
    
    [Key(3)]
    public int PlayerId { get; set; }
}