using System.Net;
using System.Net.Sockets;
using MessagePack;

namespace server.Network;

public class TcpServer
{
    private TcpListener _listener;

    public async Task StartAsync()
    {
        _listener = new TcpListener(IPAddress.Any, 7777);
        _listener.Start();
        Console.WriteLine("TCP 서버 시작됨");

        while (true)
        {
            try
            {
                var client = await _listener.AcceptTcpClientAsync();
                var session = new ClientSession(client);
                _ = session.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AcceptTcpClientAsync 예외: {ex}");
            }
        }
    }
}