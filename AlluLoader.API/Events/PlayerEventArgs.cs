using Allumeria.Blocks.Blocks;
using Allumeria.ChunkManagement;
using Allumeria.EntitySystem;
using Allumeria.EntitySystem.Components;
using Allumeria.EntitySystem.Entities;
using Allumeria.Items;
using OpenTK.Mathematics;

namespace AlluLoader.API.Events;

public class PlayerEventArgs(PlayerEntity player) : EventArgs
{
    public PlayerEntity Player { get; } = player;
}

public class PlayerDamageEventArgs(PlayerEntity player, int damage, HealthComponent.DamageType type, bool stun, World world, bool ignoreIframes) : PlayerEventArgs(player)
{
    public int DamageAmount { get; } = damage;
    public HealthComponent.DamageType DamageType { get; } = type;
    public bool Stun { get; } = stun;
    public World World { get; } = world;
    public bool IgnoreIframes { get; } = ignoreIframes;
    public bool WasCancelled { get; set; } = false;
}

public class PlayerDeathEventArgs(PlayerEntity player, World world) : PlayerEventArgs(player)
{
    public World World { get; } = world;
    public bool Cancelled { get; set; } = false;
}

public class PlayerRespawnEventArgs(PlayerEntity player, World world, bool reset, bool fromLogoff) : PlayerEventArgs(player)
{
    public bool Reset { get; } = reset;
    public bool FromLogoff { get; } = fromLogoff;
    public World World { get; } = world;
}

public class PlayerJumpEventArgs(PlayerEntity player, bool ignoreGrounded) : PlayerEventArgs(player)
{
    public bool IgnoreGrounded { get; } = ignoreGrounded;
    public bool Cancelled { get; set; } = false;
}

public class PlayerTickEventArgs(PlayerEntity player, Chunk chunk, World world) : PlayerEventArgs(player)
{
    public Chunk Chunk { get; } = chunk;
    public World World { get; } = world;
}

public class PlayerItemSwappedEventArgs(PlayerEntity player, Item? previous, Item? newItem, bool sendToServer) : PlayerEventArgs(player)
{
    public Item? PreviousItem { get; } = previous;
    public Item? NewItem { get; } = newItem;
    public bool SendToServer { get; } = sendToServer;
    public bool Cancelled { get; set; } = false;
}

public class PlayerUseItemEventArgs(PlayerEntity player, Item item, bool isLeftClick, World world) : PlayerEventArgs(player)
{
    public Item Item { get; } = item;
    public bool IsLeftClick { get; } = isLeftClick;
    public World World { get; } = world;
    public bool Cancelled { get; set; } = false;
}

public class PlayerAttackEntityEventArgs(PlayerEntity player, Entity target, World world) : PlayerEventArgs(player)
{
    public Entity Target { get; } = target; 
    public World World { get; } = world; 
    public bool Cancelled { get; set; } = false;
}

public class PlayerSweepAttackEventArgs(PlayerEntity player, World world) : PlayerEventArgs(player)
{
    public World World { get; } = world; 
    public bool Cancelled { get; set; } = false;
}

public class PlayerPlaceBlockEventArgs(PlayerEntity player, Vector3i position, Block block, World world) : PlayerEventArgs(player)
{
    public Vector3i Position { get; } = position; 
    public Block Block { get; } = block; 
    public World World { get; } = world; 
    public bool Cancelled { get; set; } = false;
}

public class PlayerBreakBlockEventArgs(PlayerEntity player, Vector3i position, Block block, World world) : PlayerEventArgs(player)
{
    public Vector3i Position { get; } = position; 
    public Block Block { get; } = block; 
    public World World { get; } = world; 
    public bool Cancelled { get; set; } = false;
}

public class PlayerInteractBlockEventArgs(PlayerEntity player, Vector3i position, Block block, World world) : PlayerEventArgs(player)
{
    public Vector3i Position { get; } = position; 
    public Block Block { get; } = block; 
    public World World { get; } = world; 
    public bool Cancelled { get; set; } = false;
}

public class PlayerDropItemEventArgs(PlayerEntity player, ItemStack stack, World world) : PlayerEventArgs(player)
{
    public ItemStack Stack { get; } = stack; 
    public World World { get; } = world; 
    public bool Cancelled { get; set; } = false;
}

public class PlayerSneakEventArgs(PlayerEntity player, bool sneaking) : PlayerEventArgs(player)
{
    public bool Sneaking { get; } = sneaking;
}

public class PlayerNoclipEventArgs(PlayerEntity player, bool noclip) : PlayerEventArgs(player)
{
    public bool Noclip { get; } = noclip;
}

public class PlayerGlideEventArgs(PlayerEntity player, bool gliding) : PlayerEventArgs(player)
{
    public bool Gliding { get; } = gliding;
}

public class PlayerFlyEventArgs(PlayerEntity player, bool flying) : PlayerEventArgs(player)
{
    public bool Flying { get; } = flying;
}

public class PlayerBlockingEventArgs(PlayerEntity player, bool blocking) : PlayerEventArgs(player)
{
    public bool Blocking { get; } = blocking;
}

public class PlayerStaminaEventArgs(PlayerEntity player, int oldStamina, int newStamina, int maxStamina) : PlayerEventArgs(player)
{
    public int OldStamina { get; } = oldStamina; 
    public int NewStamina { get; } = newStamina; 
    public int MaxStamina { get; } = maxStamina;
}
