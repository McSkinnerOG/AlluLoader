using Allumeria.Blocks.BlockEntities;
using Allumeria.EntitySystem.Entities;
using Allumeria.Items;
using Allumeria.Items.Crafting;
namespace AlluLoader.API.Events;

public class InventoryAddItemEventArgs(Inventory inventory, ItemStack stackToAdd, bool ignoreHotbar) : EventArgs
{
    public Inventory Inventory { get; } = inventory;
    public ItemStack StackToAdd { get; } = stackToAdd;
    public bool IgnoreHotbar { get; } = ignoreHotbar;
    public bool Cancelled { get; set; } = false;
    public bool Success { get; set; } = false;
    public ItemStack? Remainder { get; set; } = null;
}

public class InventoryTakeItemEventArgs(Inventory inventory, ItemStack stackToTake) : EventArgs
{
    public Inventory Inventory { get; } = inventory;
    public ItemStack StackToTake { get; } = stackToTake;
    public bool Cancelled { get; set; } = false;
    public bool Success { get; set; } = false;
    public ItemStack? TakenStack { get; set; } = null;
}

public class InventorySlotChangedEventArgs(Inventory inventory, int slotIndex, ItemStack? oldStack, ItemStack? newStack) : EventArgs
{
    public Inventory Inventory { get; } = inventory;
    public int SlotIndex { get; } = slotIndex;
    public ItemStack? OldStack { get; } = oldStack;
    public ItemStack? NewStack { get; } = newStack;
}

public class InventoryClearedEventArgs(Inventory inventory) : EventArgs
{
    public Inventory Inventory { get; } = inventory;
}

public class InventorySortedEventArgs(Inventory inventory) : EventArgs
{
    public Inventory Inventory { get; } = inventory;
}

public class PlayerInventoryOpenEventArgs(PlayerEntity player, Inventory inventory, List<CraftingStation>? stations, Inventory? chest, Catalogue? cat, BlockEntityChest? chestEntity) : EventArgs
{
    public PlayerEntity Player { get; } = player;
    public Inventory Inventory { get; } = inventory;
    public List<CraftingStation>? Stations { get; } = stations;
    public Inventory? Chest { get; } = chest;
    public Catalogue? Catalogue { get; } = cat;
    public BlockEntityChest? ChestEntity { get; } = chestEntity;
}

public class PlayerInventoryCloseEventArgs(PlayerEntity player, Inventory inventory) : EventArgs
{
    public PlayerEntity Player { get; } = player; public Inventory Inventory { get; } = inventory;
}