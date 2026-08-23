using Allumeria.UI;
using Allumeria.EntitySystem.Entities;

namespace AlluLoader.Events;

public static class ChatEvents
{
    // Fired when a chat message is about to be added to the log.
    public static event EventHandler<ChatMessageEventArgs>? OnMessageAdded;

    // Fired when a player sends a chat message (before processing commands).
    public static event EventHandler<PlayerChatEventArgs>? OnPlayerSendMessage;

    // Fired when a command is parsed (can handle custom commands).
    public static event EventHandler<CommandEventArgs>? OnCommand;
     
    internal static void InvokeMessageAdded(ChatMessageEventArgs e) => OnMessageAdded?.Invoke(null, e);
    internal static void InvokePlayerSendMessage(PlayerChatEventArgs e) => OnPlayerSendMessage?.Invoke(null, e);
    internal static void InvokeCommand(CommandEventArgs e) => OnCommand?.Invoke(null, e);
}

public class ChatMessageEventArgs(string message, bool isSystem, bool isFromPlayer, PlayerEntity? player = null) : EventArgs
{
    public string Message { get; set; } = message;
    public bool IsSystem { get; } = isSystem;
    public bool IsFromPlayer { get; } = isFromPlayer;
    public PlayerEntity? Player { get; } = player;
    public bool Cancelled { get; set; } = false;
}

public class PlayerChatEventArgs(PlayerEntity player, string message) : EventArgs
{
    public PlayerEntity Player { get; } = player;
    public string Message { get; set; } = message;
    public bool Cancelled { get; set; } = false;
}

public class CommandEventArgs(PlayerEntity player, string command, string[] args) : EventArgs
{
    public PlayerEntity Player { get; } = player;
    public string Command { get; } = command;
    public string[] Args { get; } = args;
    public bool Handled { get; set; } = false;
}