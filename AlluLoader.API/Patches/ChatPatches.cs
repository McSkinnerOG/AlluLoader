using AlluLoader.API.Events;
using AlluLoader.API.Logging;
using Allumeria;
using Allumeria.ChunkManagement;
using Allumeria.EntitySystem.Entities;
using Allumeria.Networking;
using Allumeria.UI;
using HarmonyLib;
using System.Reflection;

namespace AlluLoader.API.Patches;

[HarmonyPatch]
internal static class ChatPatches
{
    static ChatPatches()
    {
        try
        {
            var harmony = new Harmony("alluloader.api.chat");
            harmony.PatchAll();
            Log.Write("Chat API patches applied successfully.");
        }
        catch (Exception ex)
        {
            Log.Write("Failed to apply Chat API patches", ex);
        }
    }

    // ---- Patch ChatLog.NewMessage  ----
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChatLog), nameof(ChatLog.AddMessage))]
    private static bool AddMessagePrefix(ChatMessage message, ref bool __result)
    {
        return true;
    }

    // ---- NewMessage  ----
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChatLog), nameof(ChatLog.NewMessage))]
    private static bool NewMessagePrefix(string txt)
    {
        var args = new ChatMessageEventArgs(txt, false, false, null);
        ChatEvents.InvokeMessageAdded(args);
        if (args.Cancelled)
            return false;
        return true;
    }

    // ---- NewMessageSystem ----
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChatLog), nameof(ChatLog.NewMessageSystem))]
    private static bool NewMessageSystemPrefix(string txt)
    {
        var args = new ChatMessageEventArgs(txt, true, false, null);
        ChatEvents.InvokeMessageAdded(args);
        return !args.Cancelled;
    }

    // ---- NewMessageSystemToAll ----
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChatLog), nameof(ChatLog.NewMessageSystemToAll))]
    private static bool NewMessageSystemToAllPrefix(string txt)
    {
        var args = new ChatMessageEventArgs(txt, true, false, null);
        ChatEvents.InvokeMessageAdded(args);
        return !args.Cancelled;
    }

    // ---- NewMessageSystemToPlayer ----
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChatLog), nameof(ChatLog.NewMessageSystemToPlayer))]
    private static bool NewMessageSystemToPlayerPrefix(string txt, PlayerEntity player)
    {
        var args = new ChatMessageEventArgs(txt, true, false, player);
        ChatEvents.InvokeMessageAdded(args);
        return !args.Cancelled;
    }

    // ---- Player sending a message ----
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChatLog), nameof(ChatLog.PushFromPlayer))]
    private static bool PushFromPlayerPrefix(string txt)
    {
        var player = Game.clientState.player;
        if (player == null) return true;
        var args = new PlayerChatEventArgs(player, txt);
        ChatEvents.InvokePlayerSendMessage(args);
        if (args.Cancelled)
            return false;
        return true;
    }

    // RecieveChatFromClient is when a server receives a client message.
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChatLog), nameof(ChatLog.RecieveChatFromClient))]
    private static bool RecieveChatFromClientPrefix(string txt, PlayerConnection connection)
    {
        if (connection.associatedPlayer == null) return true;
        var args = new PlayerChatEventArgs(connection.associatedPlayer, txt);
        ChatEvents.InvokePlayerSendMessage(args);
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