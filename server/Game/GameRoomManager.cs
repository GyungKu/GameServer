using server.Network;

namespace server.Game;

public class GameRoomManager
{
    private static GameRoomManager _instance = new();
    public static GameRoomManager Instance => _instance;
    
    private Dictionary<int, GameRoom> _rooms = new();
    private int _roomId = 0;
    private object _lock = new();

    public void CreateRoom(List<ClientSession> players)
    {
        lock (_lock)
        {
            var gameRoom = new GameRoom(players, ++_roomId);
            _rooms.Add(_roomId, gameRoom);
            gameRoom.PickStart();
        }
    }

    public void Pick(ClientSession session, int characterId, int roomId, int playerId)
    {
        var gameRoom = _rooms[roomId];
        gameRoom.PickCharacter(session, characterId, playerId);
    }
}