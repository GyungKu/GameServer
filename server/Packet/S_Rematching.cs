using MessagePack;
using server.Packet;

namespace server.Game;

[MessagePackObject]
public class S_Rematching : PacketBase
{
    public override PacketType Type => PacketType.S_Rematching;
}