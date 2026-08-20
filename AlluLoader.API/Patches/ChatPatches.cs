using AlluLoader.API.Events;
using Allumeria;
using Allumeria.ChunkManagement;
using Allumeria.EntitySystem.Entities;
using Allumeria.Networking;
using Allumeria.UI;
using HarmonyLib;

namespace AlluLoader.API.Patches;

[HarmonyPatch]
internal static class ChatPatches
{
    // ---- NewMessage  ----
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChatLog), nameof(ChatLog.NewMessage))]
    private static bool NewMessagePrefix(ref string txt)
    {
        var args = new ChatMessageEventArgs(txt, false, false, null);
        ChatEvents.InvokeMessageAdded(args);
        txt = args.Message;
        return !args.Cancelled;
    }

    // ---- NewMessageSystem ----
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChatLog), nameof(ChatLog.NewMessageSystem))]
    private static bool NewMessageSystemPrefix(ref string txt)
    {
        var args = new ChatMessageEventArgs(txt, true, false, null);
        ChatEvents.InvokeMessageAdded(args);
        txt = args.Message;
        return !args.Cancelled;
    }

    // ---- NewMessageSystemToAll ----
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChatLog), nameof(ChatLog.NewMessageSystemToAll))]
    private static bool NewMessageSystemToAllPrefix(ref string txt)
    {
        var args = new ChatMessageEventArgs(txt, true, false, null);
        ChatEvents.InvokeMessageAdded(args);
        txt = args.Message;
        return !args.Cancelled;
    }

    // ---- NewMessageSystemToPlayer ----
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChatLog), nameof(ChatLog.NewMessageSystemToPlayer))]
    private static bool NewMessageSystemToPlayerPrefix(ref string txt, PlayerEntity player)
    {
        var args = new ChatMessageEventArgs(txt, true, false, player);
        ChatEvents.InvokeMessageAdded(args);
        txt = args.Message;
        return !args.Cancelled;
    }

    // ---- Player sending a message ----
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChatLog), nameof(ChatLog.PushFromPlayer))]
    private static bool PushFromPlayerPrefix(ref string txt)
    {
        var player = Game.clientState.player;
        if (player == null) return true;
        var args = new PlayerChatEventArgs(player, txt);
        ChatEvents.InvokePlayerSendMessage(args);
        txt = args.Message;
        return !args.Cancelled;
    }

    // RecieveChatFromClient is when a server receives a client message.
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChatLog), nameof(ChatLog.RecieveChatFromClient))]
    private static bool RecieveChatFromClientPrefix(ref string txt, PlayerConnection connection)
    {
        if (connection.associatedPlayer == null) return true;
        var args = new PlayerChatEventArgs(connection.associatedPlayer, txt);
        ChatEvents.InvokePlayerSendMessage(args);
        txt = args.Message;
        return !args.Cancelled;
    }

    // ---- Commands ----
    // ParseCommand is where commands are handled.
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChatLog), nameof(ChatLog.ParseCommand))]
    private static bool ParseCommandPrefix(string txt, World world, PlayerEntity player)
    {
        string[] args = txt.Split(' ', StringSplitOptions.None);
        string command = args[0];
        var eventArgs = new CommandEventArgs(player, command, args);
        ChatEvents.InvokeCommand(eventArgs);
        if (eventArgs.Handled)
            return false;
        return true;
    }
}