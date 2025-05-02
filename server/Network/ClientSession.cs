using System.Net.Sockets;
using server.Packet;

namespace server.Network;

public class ClientSession
{
    private TcpClient _client;
    private NetworkStream _stream;
    public SessionManager SessionManager { get; set; }
    public int UserId { get; private set; }

    private byte[] _buffer = new byte[4096];

    public ClientSession(TcpClient client)
    {
        _client = client;
        _stream = client.GetStream();
    }

    public async Task StartAsync()
    {
        while (true)
        {
            int read = await _stream.ReadAsync(_buffer);
            if (read == 0) break;

            try
            {
                // 패킷이 들어오면 역직렬화를 해서 패킷핸들러로 보낸다.
                var packet = PacketFactory.Deserialize(_buffer[..read]);
                await PacketHandler.HandleAsync(this, packet);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[역직렬화 실패] {e}");
            }
        }
    }

    public void Authenticate(int userId)
    {
        UserId = userId;
        SessionManager.Instance.AddSession(this);
    }

    public async Task SendAsync(PacketBase packet)
    {
        // 패킷을 직렬화 해서 내보낸다.
        var data = PacketFactory.Serialize(packet);
        await _stream.WriteAsync(data);
        await _stream.FlushAsync();
    }

    public void Send(PacketBase packet)
    {
        var data = PacketFactory.Serialize(packet);
        _stream.Write(data);
        _stream.Flush();
    }

    public void Disconnect()
    {
        SessionManager.Instance.RemoveSession(this);
        _stream.Close();
        _client.Close();
    }
}