using server.Data.Models;
using server.Network;

namespace server.Game;

public class LobbyRoom
{
    private List<ClientSession> _players = new();
    private object _lock = new();

    public void Enter(ClientSession session)
    {
        lock (_lock)
        {
            _players.Add(session);
        }
    }

    public void Leave(ClientSession session)
    {
        lock (_lock)
        {
            _players.Remove(session);
        }
    }
}