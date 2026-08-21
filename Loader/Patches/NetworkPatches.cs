using AlluLoader.API.Events;
using Allumeria.Networking;
using HarmonyLib;

namespace AlluLoader.API.Patches;

[HarmonyPatch]
internal static class NetworkPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Client), nameof(Client.SendPacketToServer))]
    private static bool SendPacketToServerPrefix(ref IPacket packet)
    {
        var args = new PacketEventArgs(packet, PacketDirection.ClientToServer);
        NetworkEvents.InvokePacketSending(args);
        packet = args.Packet;
        return !args.Cancelled;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Server), nameof(Server.SendPacketTo))]
    private static bool SendPacketToPrefix(ref IPacket packet, PlayerConnection connection)
    {
        var args = new PacketEventArgs(packet, PacketDirection.ServerToClient, connection);
        NetworkEvents.InvokePacketSending(args);
        packet = args.Packet;
        return !args.Cancelled;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Server), nameof(Server.SendPacketToAll))]
    private static bool SendPacketToAllPrefix(ref IPacket packet)
    {
        var args = new PacketEventArgs(packet, PacketDirection.ServerToClient);
        NetworkEvents.InvokePacketSending(args);
        packet = args.Packet;
        return !args.Cancelled;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Server), nameof(Server.SendPacketToAllExcept))]
    private static bool SendPacketToAllExceptPrefix(ref IPacket packet, PlayerConnection excludedPlayer)
    {
        var args = new PacketEventArgs(packet, PacketDirection.ServerToClient);
        NetworkEvents.InvokePacketSending(args);
        packet = args.Packet;
        return !args.Cancelled;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(NetworkManager), nameof(NetworkManager.DecodePacket))]
    private static void DecodePacketPostfix(byte packetID, PlayerConnection? playerConnection, IPacket? __result) =>
        NetworkEvents.InvokePacketDecoded(new PacketDecodedEventArgs(packetID, __result, playerConnection));

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Server), nameof(Server.AddConnection))]
    private static void AddConnectionPostfix(PlayerConnection connection) =>
        NetworkEvents.InvokeConnectionAdded(new PlayerConnectionEventArgs(connection));

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerConnection), "Kick")]
    private static bool KickPrefix(PlayerConnection __instance, ref string reason,
        out PlayerKickEventArgs __state)
    {
        __state = new PlayerKickEventArgs(__instance, reason ?? string.Empty);
        NetworkEvents.InvokePlayerKicking(__state);
        reason = __state.Reason;
        return !__state.Cancelled;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerConnection), "Kick")]
    private static void KickPostfix(PlayerKickEventArgs __state)
    {
        if (!__state.Cancelled)
            NetworkEvents.InvokePlayerKicked(__state);
    }
}