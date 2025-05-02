using MessagePack;

namespace server.Packet;

[MessagePackObject]
public class C_Chat : PacketBase
{
    public override PacketType Type => PacketType.C_Chat;
    
    [Key(1)]
    public int UserId { get; set; }
    
    [Key(2)]
    public string Chat { get; set; }
}