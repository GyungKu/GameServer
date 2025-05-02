using server.Game;
using server.Network;

namespace server.Packet;

public class PacketHandler
{
    public static async Task HandleAsync(ClientSession session, PacketBase packet)
    {
        Console.WriteLine($"[PacketHandler] {packet.GetType().Name}");
        switch (packet)
        {
            case C_Auth auth:
                await HandleAuth(session, auth);
                break;
            
            case C_LobbyEnter lobby:
                await HandleLobbyEnter(session, lobby);
                break;
            
            case C_Chat chat:
                await HandleChat(session, chat);
                break;
            
            case C_Match match:
                await HandleMatchRequest(session, match);
                break;
            
            case C_Pick pick:
                await HandlePick(session, pick);
                break;
            
            case C_MatchAccept accept:
                await HandleMatchAccept(session, accept);
                break;
        }
    }
    
    private static async Task HandleAuth(ClientSession session, C_Auth packet)
    {
        // int userId = DummyTokenStore.GetUserIdFromToken(packet.Token);
        int userId = packet.UserId;
        Console.WriteLine($"userId: {userId}");

        var response = new S_Response
        {
            Success = userId != -1
        };

        if (response.Success)
            session.Authenticate(userId);

        await session.SendAsync(response);
    }

    private static Task HandleLobbyEnter(ClientSession session, C_LobbyEnter packet)
    {
        LobbyManager.Instance.Lobby.Enter(session);
        
        var response = new S_Response{ Success = true };
        return session.SendAsync(response);
    }

    private static async Task HandleMatchRequest(ClientSession session, C_Match packet)
    {
        Console.WriteLine($"[매칭요청] userId: {session.UserId}, mode: {packet.Mode}");
        
        MatchManager.Instance.Enqueue(session, packet.Mode);
        var response = new S_Response{ Success = true };
        await session.SendAsync(response);
        
    }

    private static async Task HandlePick(ClientSession session, C_Pick packet)
    {
        Console.WriteLine($"[픽 요청] userId: {session.UserId}, character: {packet.CharacterId}");
        
        GameRoomManager.Instance.Pick(session, packet.CharacterId, packet.GameRoomId, packet.UserId);
    }

    private static async Task HandleMatchAccept(ClientSession session, C_MatchAccept packet)
    {
        Console.WriteLine($"매칭 수락요청: {packet.IsAccept}");
        MatchManager.Instance.MatchAccept(session, packet.IsAccept, packet.MatchAcceptRoomId);
    }

    private static async Task HandleChat(ClientSession session, C_Chat packet)
    {
        Console.WriteLine($"클라이언트 채팅시도 userId: {session.UserId}");
        var response = new S_Response { Success = false };
        if (session.UserId != packet.UserId)
        {
            await session.SendAsync(response);
            return;
        }
        response.Success = true;
        SessionManager.Instance.Broadcast(packet.UserId, packet.Chat);
        await session.SendAsync(response);
        
    }
}