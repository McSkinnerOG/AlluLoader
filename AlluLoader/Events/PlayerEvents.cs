using AlluLoader.Events;

public static class PlayerEvents
{
    public static event EventHandler<PlayerDamageEventArgs>? BeforeDamage;
    public static event EventHandler<PlayerDamageEventArgs>? AfterDamage;
    public static event EventHandler<PlayerDeathEventArgs>? BeforeDeath;
    public static event EventHandler<PlayerDeathEventArgs>? AfterDeath;
    public static event EventHandler<PlayerDeathEventArgs>? OnDeath;
    public static event EventHandler<PlayerRespawnEventArgs>? BeforeRespawn;
    public static event EventHandler<PlayerRespawnEventArgs>? AfterRespawn;
    public static event EventHandler<PlayerRespawnEventArgs>? OnRespawn;
    public static event EventHandler<PlayerTeleportEventArgs>? BeforeTeleport;
    public static event EventHandler<PlayerTeleportEventArgs>? AfterTeleport;
    public static event EventHandler<PlayerJumpEventArgs>? OnJump;
    public static event EventHandler<PlayerTickEventArgs>? OnTick;
    public static event EventHandler<PlayerTickEventArgs>? OnPreTick;
    public static event EventHandler<PlayerItemSwappedEventArgs>? OnItemSwapped;
    public static event EventHandler<PlayerUseItemEventArgs>? OnUseItem;
    public static event EventHandler<PlayerAttackEntityEventArgs>? OnAttackEntity;
    public static event EventHandler<PlayerAttackEntityEventArgs>? AfterAttackEntity;
    public static event EventHandler<PlayerSweepAttackEventArgs>? OnSweepAttack;
    public static event EventHandler<PlayerPlaceBlockEventArgs>? OnPlaceBlock;
    public static event EventHandler<PlayerBreakBlockEventArgs>? OnBreakBlock;
    public static event EventHandler<PlayerInteractBlockEventArgs>? OnInteractBlock;
    public static event EventHandler<PlayerDropItemEventArgs>? OnDropItem;
    public static event EventHandler<PlayerDropItemEventArgs>? AfterDropItem;
    public static event EventHandler<PlayerStaminaConsumeEventArgs>? BeforeConsumeStamina;
    public static event EventHandler<PlayerStaminaConsumeEventArgs>? AfterConsumeStamina;
    public static event EventHandler<PlayerSneakEventArgs>? OnSneakChanged;
    public static event EventHandler<PlayerNoclipEventArgs>? OnNoclipChanged;
    public static event EventHandler<PlayerGlideEventArgs>? OnGlideChanged;
    public static event EventHandler<PlayerFlyEventArgs>? OnFlyChanged;
    public static event EventHandler<PlayerBlockingEventArgs>? OnBlockingChanged;
    public static event EventHandler<PlayerStaminaEventArgs>? OnStaminaChanged;

    internal static void InvokeBeforeDamage(PlayerDamageEventArgs e) => BeforeDamage?.Invoke(null, e);
    internal static void InvokeAfterDamage(PlayerDamageEventArgs e) => AfterDamage?.Invoke(null, e);
    internal static void InvokeBeforeDeath(PlayerDeathEventArgs e) => BeforeDeath?.Invoke(null, e);
    internal static void InvokeAfterDeath(PlayerDeathEventArgs e) { AfterDeath?.Invoke(null, e); OnDeath?.Invoke(null, e); }
    internal static void InvokeBeforeRespawn(PlayerRespawnEventArgs e) => BeforeRespawn?.Invoke(null, e);
    internal static void InvokeAfterRespawn(PlayerRespawnEventArgs e) { AfterRespawn?.Invoke(null, e); OnRespawn?.Invoke(null, e); }
    internal static void InvokeBeforeTeleport(PlayerTeleportEventArgs e) => BeforeTeleport?.Invoke(null, e);
    internal static void InvokeAfterTeleport(PlayerTeleportEventArgs e) => AfterTeleport?.Invoke(null, e);
    internal static void InvokeJump(PlayerJumpEventArgs e) => OnJump?.Invoke(null, e);
    internal static void InvokeTick(PlayerTickEventArgs e) => OnTick?.Invoke(null, e);
    internal static void InvokePreTick(PlayerTickEventArgs e) => OnPreTick?.Invoke(null, e);
    internal static void InvokeItemSwapped(PlayerItemSwappedEventArgs e) => OnItemSwapped?.Invoke(null, e);
    internal static void InvokeUseItem(PlayerUseItemEventArgs e) => OnUseItem?.Invoke(null, e);
    internal static void InvokeAttackEntity(PlayerAttackEntityEventArgs e) => OnAttackEntity?.Invoke(null, e);
    internal static void InvokeAfterAttackEntity(PlayerAttackEntityEventArgs e) => AfterAttackEntity?.Invoke(null, e);
    internal static void InvokeSweepAttack(PlayerSweepAttackEventArgs e) => OnSweepAttack?.Invoke(null, e);
    internal static void InvokePlaceBlock(PlayerPlaceBlockEventArgs e) => OnPlaceBlock?.Invoke(null, e);
    internal static void InvokeBreakBlock(PlayerBreakBlockEventArgs e) => OnBreakBlock?.Invoke(null, e);
    internal static void InvokeInteractBlock(PlayerInteractBlockEventArgs e) => OnInteractBlock?.Invoke(null, e);
    internal static void InvokeDropItem(PlayerDropItemEventArgs e) => OnDropItem?.Invoke(null, e);
    internal static void InvokeAfterDropItem(PlayerDropItemEventArgs e) => AfterDropItem?.Invoke(null, e);
    internal static void InvokeBeforeConsumeStamina(PlayerStaminaConsumeEventArgs e) => BeforeConsumeStamina?.Invoke(null, e);
    internal static void InvokeAfterConsumeStamina(PlayerStaminaConsumeEventArgs e) => AfterConsumeStamina?.Invoke(null, e);
    internal static void InvokeSneakChanged(PlayerSneakEventArgs e) => OnSneakChanged?.Invoke(null, e);
    internal static void InvokeNoclipChanged(PlayerNoclipEventArgs e) => OnNoclipChanged?.Invoke(null, e);
    internal static void InvokeGlideChanged(PlayerGlideEventArgs e) => OnGlideChanged?.Invoke(null, e);
    internal static void InvokeFlyChanged(PlayerFlyEventArgs e) => OnFlyChanged?.Invoke(null, e);
    internal static void InvokeBlockingChanged(PlayerBlockingEventArgs e) => OnBlockingChanged?.Invoke(null, e);
    internal static void InvokeStaminaChanged(PlayerStaminaEventArgs e) => OnStaminaChanged?.Invoke(null, e);
}