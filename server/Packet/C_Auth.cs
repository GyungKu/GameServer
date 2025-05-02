using MessagePack;

namespace server.Packet;

[MessagePackObject]
public class C_Auth : PacketBase
{
    public override PacketType Type => PacketType.C_Auth;
    
    [Key(1)]
    public int UserId { get; set; }
    
    [Key(2)]
    public string Nickname { get; set; }
    
}