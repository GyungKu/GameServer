using server.Network;

namespace server.Game;

public class MatchManager
{
    private static MatchManager _instance = new();
    public static MatchManager Instance => _instance;

    private Dictionary<int, Queue<ClientSession>> _queues = new();
    
    private Dictionary<int, MatchAcceptRoom> _matchAcceptRooms = new();
    
    private int _matchAcceptRoomId = 0;
    
    private object _lock = new();

    public void Enqueue(ClientSession clientSession, int mode)
    {
        // 해당 게임모드를 돌리고 있는 유저들의 Queue가 없으면 새로 생성하고 있다면 그 queue에 요청한 session을 넣는다.
        if (!_queues.ContainsKey(mode)) _queues[mode] = new Queue<ClientSession>();
        _queues[mode].Enqueue(clientSession);
        
        TryMatch(mode);
    }

    public void GetQueue(int needPlayers, int mode, int matchAcceptRoomId)
    {
        var matchAcceptRoom = _matchAcceptRooms[matchAcceptRoomId];

        while (needPlayers > 0)
        {
            lock (_lock)
            {
                if (_queues[mode].Count > 0)
                {
                    matchAcceptRoom.AddPlayer(_queues[mode].Dequeue());
                    needPlayers--;
                }
            }
        }

        matchAcceptRoom.StartAsync();
    }

    public void MatchAccept(ClientSession session, bool isAccept, int matchAcceptRoomId)
    {
        var matchAcceptRoom = _matchAcceptRooms[matchAcceptRoomId];
        if (isAccept)
        {
            matchAcceptRoom.Accept(session);
            return;
        }
        matchAcceptRoom.Reject(session);
    }

    private void TryMatch(int mode)
    {
        int teamSize = mode switch
        {
            1 => 2,
            2 => 4,
            3 => 6,
            _ => 2
        };

        lock (_lock)
        {
            var queue = _queues[mode];
            if (queue.Count >= teamSize)
            {
                var matched = new List<ClientSession>();
                for (int i = 0; i < teamSize; i++)
                {
                    matched.Add(queue.Dequeue());
                }

                CreateMatchRoom(matched, mode);
            }
        }
    }

    private async void CreateMatchRoom(List<ClientSession> players, int mode)
    {
        var matchAcceptRoom = new MatchAcceptRoom(players, ++_matchAcceptRoomId, mode);
        _matchAcceptRooms.Add(matchAcceptRoom.MatchAcceptRoomId, matchAcceptRoom);
        await matchAcceptRoom.StartAsync();
    }
}