using Allumeria.Networking;

namespace AlluLoader.Events;

public enum PacketDirection
{
    ClientToServer,
    ServerToClient,
    Incoming
}

public class PacketEventArgs(IPacket packet, PacketDirection direction, PlayerConnection? connection = null) : EventArgs
{
    public IPacket Packet { get; set; } = packet;
    public PacketDirection Direction { get; } = direction;
    public PlayerConnection? Connection { get; } = connection;
    public bool Cancelled { get; set; }
}

public class PacketDecodedEventArgs(byte packetId, IPacket? packet, PlayerConnection? connection) : EventArgs
{
    public byte PacketId { get; } = packetId;
    public IPacket? Packet { get; } = packet;
    public PlayerConnection? Connection { get; } = connection;
}

public class PlayerConnectionEventArgs(PlayerConnection connection) : EventArgs
{ public PlayerConnection Connection { get; } = connection; }

public class PlayerKickEventArgs(PlayerConnection connection, string reason) : PlayerConnectionEventArgs(connection)
{
    public string Reason { get; set; } = reason;
    public bool Cancelled { get; set; }
}

public static class NetworkEvents
{
    public static event EventHandler<PacketDecodedEventArgs>? PacketDecoded;
    public static event EventHandler<PacketEventArgs>? PacketSending;
    public static event EventHandler<PlayerConnectionEventArgs>? ConnectionAdded;
    public static event EventHandler<PlayerKickEventArgs>? PlayerKicking;
    public static event EventHandler<PlayerKickEventArgs>? PlayerKicked;

    internal static void InvokePacketDecoded(PacketDecodedEventArgs e) => PacketDecoded?.Invoke(null, e);
    internal static void InvokePacketSending(PacketEventArgs e) => PacketSending?.Invoke(null, e);
    internal static void InvokeConnectionAdded(PlayerConnectionEventArgs e) => ConnectionAdded?.Invoke(null, e);
    internal static void InvokePlayerKicking(PlayerKickEventArgs e) => PlayerKicking?.Invoke(null, e);
    internal static void InvokePlayerKicked(PlayerKickEventArgs e) => PlayerKicked?.Invoke(null, e);
}