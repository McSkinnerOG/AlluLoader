using Allumeria.Blocks.BlockEntities;
using Allumeria.EntitySystem.Entities;
using Allumeria.Items;
using Allumeria.Items.Crafting;

namespace AlluLoader.API.Events;

public static class InventoryEvents
{
    // Fired before an item is added to an inventory; can cancel by setting Cancelled = true.
    public static event EventHandler<InventoryAddItemEventArgs>? OnBeforeAddItem;
    // Fired after an item is added (or attempted) to an inventory.
    public static event EventHandler<InventoryAddItemEventArgs>? OnAfterAddItem;
    // Fired before an item is taken from an inventory; can cancel.
    public static event EventHandler<InventoryTakeItemEventArgs>? OnBeforeTakeItem;
    public static event EventHandler<InventoryTakeItemEventArgs>? OnAfterTakeItem;
    // Fired when a slot's contents change (item set, taken, etc.)
    public static event EventHandler<InventorySlotChangedEventArgs>? OnSlotChanged;
    // Fired when inventory is cleared.
    public static event EventHandler<InventoryClearedEventArgs>? OnCleared;
    // Fired when inventory is sorted.
    public static event EventHandler<InventorySortedEventArgs>? OnSorted;
    // Player-specific: inventory opened/closed.
    public static event EventHandler<PlayerInventoryOpenEventArgs>? OnPlayerInventoryOpened;
    public static event EventHandler<PlayerInventoryCloseEventArgs>? OnPlayerInventoryClosed;
    // When a player drops an item (already have PlayerDropItemEventArgs, but we can add another)

    // Internal invokers
    internal static void InvokeBeforeAddItem(InventoryAddItemEventArgs e) => OnBeforeAddItem?.Invoke(null, e);
    internal static void InvokeAfterAddItem(InventoryAddItemEventArgs e) => OnAfterAddItem?.Invoke(null, e);
    internal static void InvokeBeforeTakeItem(InventoryTakeItemEventArgs e) => OnBeforeTakeItem?.Invoke(null, e);
    internal static void InvokeAfterTakeItem(InventoryTakeItemEventArgs e) => OnAfterTakeItem?.Invoke(null, e);
    internal static void InvokeSlotChanged(InventorySlotChangedEventArgs e) => OnSlotChanged?.Invoke(null, e);
    internal static void InvokeCleared(InventoryClearedEventArgs e) => OnCleared?.Invoke(null, e);
    internal static void InvokeSorted(InventorySortedEventArgs e) => OnSorted?.Invoke(null, e);
    internal static void InvokePlayerInventoryOpened(PlayerInventoryOpenEventArgs e) => OnPlayerInventoryOpened?.Invoke(null, e);
    internal static void InvokePlayerInventoryClosed(PlayerInventoryCloseEventArgs e) => OnPlayerInventoryClosed?.Invoke(null, e);
}