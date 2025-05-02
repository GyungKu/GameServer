using MessagePack;

namespace server.Packet;

[MessagePackObject]
public class C_LobbyEnter : PacketBase
{
    public override PacketType Type => PacketType.C_LobbyEnter;
    [Key(1)] public int userId { get; set; }

}