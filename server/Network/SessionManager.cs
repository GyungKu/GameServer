using server.Packet;

namespace server.Network;

public class SessionManager
{
    private static SessionManager _instance = new SessionManager();
    public static SessionManager Instance => _instance;
    
    private Dictionary<int, ClientSession> _sessions = new Dictionary<int, ClientSession>();
    private object _lock = new object();
    

    public void AddSession(ClientSession session)
    {
        lock (_lock)
        {
            session.SessionManager = this;
            _sessions.Add(session.UserId, session);
        }
    }

    public void RemoveSession(ClientSession session)
    {
        lock (_lock)
        {
            session.SessionManager = null;
            _sessions.Remove(session.UserId);
        }
    }
    
    public void Broadcast(int userId, string chat)
    { 
        foreach (var session in _sessions.Values)
        {
            if (session.UserId == userId) return;
            var req = new C_Chat {UserId = userId, Chat = chat};
            lock (_lock)
            {
                session.Send(req);
            }
        }
    }
}