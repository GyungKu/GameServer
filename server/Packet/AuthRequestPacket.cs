using MessagePack;

namespace server.Packet;

[MessagePackObject]
public class AuthRequestPacket : PacketBase
{
    public override PacketType Type => PacketType.AuthRequest;
    
    [Key(1)]
    public int UserId { get; set; }
    
    [Key(2)]
    public string Nickname { get; set; }
    
}