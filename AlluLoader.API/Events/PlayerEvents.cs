using System;
using System.Collections.Generic;
using System.Text;

namespace AlluLoader.API.Events;
public static class PlayerEvents
{
    // Fired before damage is applied; can cancel by setting WasCancelled = true.
    public static event EventHandler<PlayerDamageEventArgs>? BeforeDamage;
    // Fired after damage is applied.
    public static event EventHandler<PlayerDamageEventArgs>? AfterDamage;
    
    public static event EventHandler<PlayerDeathEventArgs>? OnDeath;
    public static event EventHandler<PlayerRespawnEventArgs>? OnRespawn;
    public static event EventHandler<PlayerJumpEventArgs>? OnJump;
    public static event EventHandler<PlayerTickEventArgs>? OnTick;
    public static event EventHandler<PlayerTickEventArgs>? OnPreTick; // Called before base.Tick
    public static event EventHandler<PlayerItemSwappedEventArgs>? OnItemSwapped;

    // Use item (left or right click)
    public static event EventHandler<PlayerUseItemEventArgs>? OnUseItem;

    // Attack entity (punch)
    public static event EventHandler<PlayerAttackEntityEventArgs>? OnAttackEntity;

    // Sweep attack
    public static event EventHandler<PlayerSweepAttackEventArgs>? OnSweepAttack;

    // Place block
    public static event EventHandler<PlayerPlaceBlockEventArgs>? OnPlaceBlock;

    // Break block
    public static event EventHandler<PlayerBreakBlockEventArgs>? OnBreakBlock;

    // Interact with block (right‑click)
    public static event EventHandler<PlayerInteractBlockEventArgs>? OnInteractBlock;

    // Drop item
    public static event EventHandler<PlayerDropItemEventArgs>? OnDropItem;

    // Sneak toggle
    public static event EventHandler<PlayerSneakEventArgs>? OnSneakChanged;

    // Noclip toggle
    public static event EventHandler<PlayerNoclipEventArgs>? OnNoclipChanged;

    // Glide start/stop
    public static event EventHandler<PlayerGlideEventArgs>? OnGlideChanged;

    // Flying start/stop
    public static event EventHandler<PlayerFlyEventArgs>? OnFlyChanged;

    // Blocking start/stop
    public static event EventHandler<PlayerBlockingEventArgs>? OnBlockingChanged;

    // Stamina changed
    public static event EventHandler<PlayerStaminaEventArgs>? OnStaminaChanged;


    // Internal methods to invoke events safely.
    internal static void InvokeBeforeDamage(PlayerDamageEventArgs e) => BeforeDamage?.Invoke(null, e);
    internal static void InvokeAfterDamage(PlayerDamageEventArgs e) => AfterDamage?.Invoke(null, e);
    internal static void InvokeDeath(PlayerDeathEventArgs e) => OnDeath?.Invoke(null, e);
    internal static void InvokeRespawn(PlayerRespawnEventArgs e) => OnRespawn?.Invoke(null, e);
    internal static void InvokeJump(PlayerJumpEventArgs e) => OnJump?.Invoke(null, e);
    internal static void InvokeTick(PlayerTickEventArgs e) => OnTick?.Invoke(null, e);
    internal static void InvokePreTick(PlayerTickEventArgs e) => OnPreTick?.Invoke(null, e);
    internal static void InvokeItemSwapped(PlayerItemSwappedEventArgs e) => OnItemSwapped?.Invoke(null, e);
    internal static void InvokeUseItem(PlayerUseItemEventArgs e) => OnUseItem?.Invoke(null, e);
    internal static void InvokeAttackEntity(PlayerAttackEntityEventArgs e) => OnAttackEntity?.Invoke(null, e);
    internal static void InvokeSweepAttack(PlayerSweepAttackEventArgs e) => OnSweepAttack?.Invoke(null, e);
    internal static void InvokePlaceBlock(PlayerPlaceBlockEventArgs e) => OnPlaceBlock?.Invoke(null, e);
    internal static void InvokeBreakBlock(PlayerBreakBlockEventArgs e) => OnBreakBlock?.Invoke(null, e);
    internal static void InvokeInteractBlock(PlayerInteractBlockEventArgs e) => OnInteractBlock?.Invoke(null, e);
    internal static void InvokeDropItem(PlayerDropItemEventArgs e) => OnDropItem?.Invoke(null, e);
    internal static void InvokeSneakChanged(PlayerSneakEventArgs e) => OnSneakChanged?.Invoke(null, e);
    internal static void InvokeNoclipChanged(PlayerNoclipEventArgs e) => OnNoclipChanged?.Invoke(null, e);
    internal static void InvokeGlideChanged(PlayerGlideEventArgs e) => OnGlideChanged?.Invoke(null, e);
    internal static void InvokeFlyChanged(PlayerFlyEventArgs e) => OnFlyChanged?.Invoke(null, e);
    internal static void InvokeBlockingChanged(PlayerBlockingEventArgs e) => OnBlockingChanged?.Invoke(null, e);
    internal static void InvokeStaminaChanged(PlayerStaminaEventArgs e) => OnStaminaChanged?.Invoke(null, e);
} 
