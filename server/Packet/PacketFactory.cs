using MessagePack;

namespace server.Packet;

public class PacketFactory
{
    public static byte[] Serialize(PacketBase packet)
    {
        return MessagePackSerializer.Serialize<PacketBase>(packet);
    }

    public static PacketBase Deserialize(byte[] data)
    {
        return MessagePackSerializer.Deserialize<PacketBase>(data);
    }
}