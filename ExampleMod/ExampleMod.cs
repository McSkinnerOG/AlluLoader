using AlluLoader;
using AlluLoader.Events;
using AlluLoader.Logging;
using Allumeria.EntitySystem.Components;
using Allumeria.EntitySystem.Entities;

namespace ExampleMod;

/// <summary>
/// Demonstrates observational, cancellable, and mutable AlluLoader events.
/// Type "eventdemo" in chat to enable or disable the gameplay-changing examples.
/// </summary>
public sealed class ExampleMod : IMod
{
    private static bool _gameplayHooksEnabled = true;

    public void Initialize()
    {
        Log.Write("ExampleMod started; registering event handlers.");

        // Player lifecycle and actions.
        PlayerEvents.BeforeDamage += BeforeDamage;
        PlayerEvents.AfterDamage += AfterDamage;
        PlayerEvents.AfterDeath += AfterDeath;
        PlayerEvents.BeforeTeleport += BeforeTeleport;
        PlayerEvents.AfterTeleport += AfterTeleport;
        PlayerEvents.BeforeConsumeStamina += BeforeConsumeStamina;
        PlayerEvents.AfterConsumeStamina += AfterConsumeStamina;
        PlayerEvents.OnBreakBlock += BeforeBreakBlock;
        PlayerEvents.AfterAttackEntity += AfterAttackEntity;

        // Inventory and entity observations.
        InventoryEvents.OnBeforeAddItem += BeforeAddItem;
        InventoryEvents.OnAfterAddItem += AfterAddItem;
        InventoryEvents.OnPlayerInventoryOpened += InventoryOpened;
        EntityEvents.ComponentAdded += ComponentAdded;

        // Lifecycle and chat command examples.
        GameStateEvents.InternalStateSetup += InternalStateSetup;
        GameStateEvents.InternalStateShuttingDown += InternalStateShuttingDown;
        ChatEvents.OnCommand += OnCommand;

        Log.Write("ExampleMod event handlers registered successfully.");
    }

    private static void BeforeDamage(object? sender, PlayerDamageEventArgs e)
    {
        Log.Write($"BeforeDamage: Player={e.Player.name}, Amount={e.DamageAmount}, Type={e.DamageType}");

        // Cancellation example: protect players from drowning while the demo is enabled.
        if (_gameplayHooksEnabled && e.DamageType == HealthComponent.DamageType.Drowning)
        {
            e.WasCancelled = true;
            Log.Write($"Cancelled drowning damage for {e.Player.name}.");
        }
    }

    private static void AfterDamage(object? sender, PlayerDamageEventArgs e)
    {
        // AfterDamage only fires when HealthComponent.TakeDamage reports success.
        Log.Write($"AfterDamage: {e.Player.name} received {e.DamageAmount} {e.DamageType} damage.");
    }

    private static void AfterDeath(object? sender, PlayerDeathEventArgs e)
    {
        Log.Write($"AfterDeath: {e.Player.name} died at {e.Player.position}.");
    }

    private static void BeforeTeleport(object? sender, PlayerTeleportEventArgs e)
    {
        if (!_gameplayHooksEnabled || e.Destination.Y >= 1f)
            return;

        // Mutation example: keep demo teleports above the bottom of the world.
        var destination = e.Destination;
        destination.Y = 1f;
        e.Destination = destination;
        Log.Write($"Adjusted {e.Player.name}'s teleport destination to {destination}.");
    }

    private static void AfterTeleport(object? sender, PlayerTeleportEventArgs e)
    {
        Log.Write($"AfterTeleport: {e.Player.name} moved from {e.OldPosition} to {e.Destination}.");
    }

    private static void BeforeConsumeStamina(object? sender, PlayerStaminaConsumeEventArgs e)
    {
        if (!_gameplayHooksEnabled)
            return;

        // Mutation example: halve stamina costs, while keeping positive costs at least one.
        e.Amount = Math.Max(1, e.Amount / 2);
    }

    private static void AfterConsumeStamina(object? sender, PlayerStaminaConsumeEventArgs e)
    {
        Log.Write($"Stamina: Player={e.Player.name}, Cost={e.Amount}, " +
                  $"Success={e.Success}, Before={e.OldStamina}, After={e.NewStamina}");
    }

    private static void BeforeBreakBlock(object? sender, PlayerBreakBlockEventArgs e)
    {
        // Cancellation example: protect the bottom layer of the world.
        if (_gameplayHooksEnabled && e.Position.Y <= 0)
        {
            e.Cancelled = true;
            Log.Write($"Prevented {e.Player.name} from breaking a block at {e.Position}.");
        }
    }

    private static void AfterAttackEntity(object? sender, PlayerAttackEntityEventArgs e)
    {
        Log.Write($"Attack: {e.Player.name} attacked {e.Target.GetType().Name}; " +
                  $"next attack delay={e.AttackDelay}.");
    }

    private static void BeforeAddItem(object? sender, InventoryAddItemEventArgs e)
    {
        Log.Write($"Inventory add attempt: Item={e.StackToAdd.GetItem().strID}, " +
                  $"Amount={e.StackToAdd.amount}, IgnoreHotbar={e.IgnoreHotbar}");
    }

    private static void AfterAddItem(object? sender, InventoryAddItemEventArgs e)
    {
        int remainder = e.Remainder?.amount ?? 0;
        Log.Write($"Inventory add result: Success={e.Success}, Remainder={remainder}, " +
                  $"Cancelled={e.Cancelled}");
    }

    private static void InventoryOpened(object? sender, PlayerInventoryOpenEventArgs e)
    {
        string inventoryType = e.Chest is null ? "player inventory" : "chest";
        Log.Write($"{e.Player.name} opened a {inventoryType}.");
    }

    private static void ComponentAdded(object? sender, EntityComponentEventArgs e)
    { 
        if (e.Entity is PlayerEntity player)
        {
            Log.Write($"Player component added: Player={player.name}, " +
                      $"Component={e.Component.GetType().Name}");
        }
    }

    private static void InternalStateSetup(object? sender, InternalGameStateEventArgs e)
    {
        Log.Write("Internal game state setup completed.");
    }

    private static void InternalStateShuttingDown(object? sender, InternalGameStateEventArgs e)
    {
        Log.Write("Internal game state is shutting down.");
    }

    private static void OnCommand(object? sender, CommandEventArgs e)
    {
        if (!e.Command.Equals("eventdemo", StringComparison.OrdinalIgnoreCase))
            return;

        _gameplayHooksEnabled = !_gameplayHooksEnabled;
        string state = _gameplayHooksEnabled ? "enabled" : "disabled";
        Chat.SendSystemMessage($"ExampleMod gameplay hooks are now {state}.");
        Log.Write($"ExampleMod gameplay hooks {state} by {e.Player.name}.");
        e.Handled = true;
    }
}