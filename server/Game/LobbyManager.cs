namespace server.Game;

public class LobbyManager
{
    private static LobbyManager _instance = new();
    public static LobbyManager Instance => _instance;

    public LobbyRoom Lobby { get; } = new();
}