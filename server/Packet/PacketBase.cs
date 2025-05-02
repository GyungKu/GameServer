using MessagePack;
using server.Game;

namespace server.Packet;

[MessagePackObject]
[Union(0, typeof(S_Response))]
[Union(1, typeof(AuthRequestPacket))]
[Union(2, typeof(C_Chat))]
[Union(3, typeof(C_LobbyEnter))]
[Union(4, typeof(MatchRequestPacket))]
[Union(5, typeof(S_MatchAccept))]
[Union(6, typeof(S_PickStart))]
[Union(7, typeof(C_Pick))]
[Union(8, typeof(S_Pick))]
[Union(9, typeof(S_FixedPick))]
[Union(10, typeof(C_MatchAccept))]
[Union(11, typeof(S_Rematching))]
public abstract class PacketBase
{
    [Key(0)]
    public abstract PacketType Type { get; }
}

public enum PacketType : ushort
{
    S_Response = 0,
    AuthRequest = 1,
    C_Chat = 2,
    C_LobbyEnter = 3,
    MatchRequest = 4,
    S_MatchAccept = 5,
    S_PickStart = 6,
    C_Pick = 7,
    S_Pick = 8,
    S_FixedPick = 9,
    C_MatchAccept = 10,
    S_Rematching = 11,
}