using Allumeria.Networking;

namespace AlluLoader.API.Events;

public class InternalGameStateEventArgs(InternalGameState state) : EventArgs
{ public InternalGameState State { get; } = state; }

public class ClientGameStateEventArgs(ClientGameState state) : EventArgs
{ public ClientGameState State { get; } = state; }

public static class GameStateEvents
{
    public static event EventHandler<InternalGameStateEventArgs>? InternalStateSetup;
    public static event EventHandler<InternalGameStateEventArgs>? InternalStateShuttingDown;
    public static event EventHandler<ClientGameStateEventArgs>? ClientStateSetup;
    public static event EventHandler<ClientGameStateEventArgs>? ClientStateShuttingDown;

    internal static void InvokeInternalStateSetup(InternalGameStateEventArgs e) => InternalStateSetup?.Invoke(null, e);
    internal static void InvokeInternalStateShuttingDown(InternalGameStateEventArgs e) => InternalStateShuttingDown?.Invoke(null, e);
    internal static void InvokeClientStateSetup(ClientGameStateEventArgs e) => ClientStateSetup?.Invoke(null, e);
    internal static void InvokeClientStateShuttingDown(ClientGameStateEventArgs e) => ClientStateShuttingDown?.Invoke(null, e);
}