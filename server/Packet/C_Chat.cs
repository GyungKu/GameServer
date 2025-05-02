using MessagePack;

namespace server.Packet;

[MessagePackObject]
public class C_Chat : PacketBase
{
    public override PacketType Type => PacketType.C_Chat;
    
    [Key(1)]
    public int userId { get; set; }
    
    [Key(2)]
    public string chat { get; set; }
}