using server.Network;
using server.Packet;

namespace server.Game;

public class MatchAcceptRoom
{
    private List<ClientSession> _players;
    private HashSet<int> _acceptPlayers = new();
    private CancellationTokenSource _cts = new();
    private const int AcceptTimeoutSeconds = 20;
    
    public int MatchAcceptRoomId { get; set; }
    public int Mode { get; set; }

    public MatchAcceptRoom(List<ClientSession> players, int matchAcceptRoomId, int mode)
    {
        _players = players;
        MatchAcceptRoomId = matchAcceptRoomId;
        Mode = mode;
    }

    public async Task StartAsync()
    {
        Broadcast(new S_MatchAccept{ Success = true, RoomId = MatchAcceptRoomId });

        try
        {
            await Task.Delay(AcceptTimeoutSeconds * 1000, _cts.Token);
        }
        catch (TaskCanceledException) { }

        if (_acceptPlayers.Count == _players.Count)
        {
            GameRoomManager.Instance.CreateRoom(_players);
        }
        else
        {
            Broadcast(new S_Rematching());
            Console.WriteLine("재 매칭 요청");
            int needPlayers = Mode * 2 - _players.Count;
            MatchManager.Instance.GetQueue(needPlayers, Mode, MatchAcceptRoomId);
        }
    }

    public void Accept(ClientSession session)
    {
        _acceptPlayers.Add(session.UserId);
        if (_acceptPlayers.Count == _players.Count) _cts.Cancel();
    }

    public void Reject(ClientSession session)
    {
        _players.Remove(session);
        _acceptPlayers.Remove(session.UserId);
        _cts.Cancel();
    }

    public void AddPlayer(ClientSession player)
    {
        _players.Add(player);
    }

    private void Broadcast(PacketBase packet)
    {
        foreach (var player in _players)
        {
            player.Send(packet);
        }
    }
}