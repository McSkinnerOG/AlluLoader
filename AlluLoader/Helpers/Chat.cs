using Allumeria.UI;
using Allumeria.EntitySystem.Entities;

namespace AlluLoader;

public static class Chat
{
    /// <summary>
    /// Sends a system message to the local player's chat.
    /// </summary>
    public static void SendSystemMessage(string message)
    {
        ChatLog.NewMessageSystem(message);
    }

    /// <summary>
    /// Sends a system message to all players (server only).
    /// </summary>
    public static void SendSystemMessageToAll(string message)
    {
        ChatLog.NewMessageSystemToAll(message);
    }

    /// <summary>
    /// Sends a system message to a specific player (server only).
    /// </summary>
    public static void SendSystemMessageToPlayer(string message, PlayerEntity player)
    {
        ChatLog.NewMessageSystemToPlayer(message, player);
    }

    /// <summary>
    /// Sends a normal (non-system) message to the local chat log.
    /// </summary>
    public static void SendMessage(string message)
    {
        ChatLog.NewMessage(message);
    }

    /// <summary>
    /// Sends a message as if from a player (adds the player's name prefix).
    /// Use this on server to broadcast a player's message.
    /// </summary>
    public static void SendPlayerMessage(PlayerEntity player, string message)
    {
        string formatted = $" {player.name}{ChatLog.userSuffix}{message} ";
        ChatLog.AddMessage(new ChatMessage(formatted));
    }
}