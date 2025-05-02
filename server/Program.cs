using MessagePack;
using server.Network;

namespace server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== 게임 서버 시작 ===");

            TcpServer server = new TcpServer();
            await server.StartAsync(); // 원하는 포트

            Console.WriteLine("=== 서버 종료 ===");
        }
    }
}
