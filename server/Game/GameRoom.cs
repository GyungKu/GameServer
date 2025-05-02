using server.Data;
using server.Network;
using server.Packet;

namespace server.Game;

public class GameRoom
{
    private Dictionary<int, PickPlayer> _players = new();
    private List<ClientSession> _sessions;
    private Dictionary<Team, List<ClientSession>> _teams = new()
    {
        { Team.TeamA, new List<ClientSession>() },
        { Team.TeamB, new List<ClientSession>() }
        
    };

    private Dictionary<Team, Dictionary<int, bool>> _teamPickedCharacters = new()
    {
        { Team.TeamA, new Dictionary<int, bool>() },
        { Team.TeamB, new Dictionary<int, bool>() }
    };
    private int _roomId;
    private const int MAX_CHARACTERS = 3;
    
    private object _lock = new();
    
    private HashSet<int> _pickedPlayers = new();

    public GameRoom(List<ClientSession> sessions, int roomId)
    {
        _sessions = sessions;
        _roomId = roomId;
        for (int i = 0; i < sessions.Count; i++)
        {
            var pickPlayer = new PickPlayer
            {
                PlayerId = sessions[i].UserId,
                Team = Team.TeamA
            };
            if (i % 2 == 0) pickPlayer.Team = Team.TeamB;
            _teams[pickPlayer.Team].Add(sessions[i]);
            _players.Add(pickPlayer.PlayerId, pickPlayer);
        }
    }

    public void PickStart()
    {
        Console.WriteLine($"Starting game: {_roomId}");
        foreach (var session in _sessions)
        {
            session.Send(new S_PickStart { GameRoomId = _roomId });
        }
    }

    public void PickCharacter(ClientSession session, int characterId, int playerId)
    {
        // C_Pick -> characterId, playerId가 들어온다.
        // 요청이 들어온 playerId와 세션이 갖고있는 userId가 다르거나, characterId가 전체 캐릭터수 보다 크다면 불가능 판정
        if (playerId != session.UserId || characterId >= MAX_CHARACTERS)
        {
            session.Send(new S_Pick { Success = false, GameRoomId = _roomId});
            return;
        }
        
        var pickPlayer = _players[session.UserId];

        lock (_lock)
        {
            // 이미 팀에서 누군가가 픽한 것 이므로 실패처리
            if (_teamPickedCharacters[pickPlayer.Team].ContainsKey(characterId) && 
                _teamPickedCharacters[pickPlayer.Team][characterId])
            {
                Console.WriteLine($"[픽 실패] 방: {_roomId}, 캐릭터: {characterId}, 플레이어: {playerId}");
                session.Send(new S_Pick { Success = false, GameRoomId = _roomId});
                return;
            }
            
            // 픽 완료 처리
            _teamPickedCharacters[pickPlayer.Team].Add(characterId, true);
            pickPlayer.PickedCharacterId = characterId;
            
            session.Send(new S_Pick { Success = true, GameRoomId = _roomId});
            Console.WriteLine($"[픽 성공] 방: {_roomId}, 캐릭터: {characterId}, 플레이어: {playerId}");
            
            TeamBroadCast(new S_FixedPick
            {
                PickedCharacterId = pickPlayer.PickedCharacterId,
                Team = pickPlayer.Team,
                UserId = pickPlayer.PlayerId
            });

            _pickedPlayers.Add(playerId);
            if (_players.Count == _pickedPlayers.Count)
            {
                // 게임진입, DB에 각종 픽 정보들 집어 넣으면 될듯
            }
        }
    }

    private void TeamBroadCast(S_FixedPick packet)
    {
        foreach (var session in _teams[packet.Team])
        {
            if (session.UserId == packet.UserId) return;
            session.Send(packet);
        }
    }
}