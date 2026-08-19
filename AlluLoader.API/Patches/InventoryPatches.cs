using AlluLoader.API.Events;
using AlluLoader.API.Logging;
using Allumeria.Blocks.BlockEntities;
using Allumeria.EntitySystem.Entities;
using Allumeria.Items;
using Allumeria.Items.Crafting;
using HarmonyLib;
using System.Collections.Concurrent;

namespace AlluLoader.API.Patches;

[HarmonyPatch]
internal static class InventoryPatches
{
    static InventoryPatches()
    {
        try
        {
            var harmony = new Harmony("alluloader.api.inventory");
            harmony.PatchAll();
            Log.Write("Inventory API patches applied successfully.");
        }
        catch (Exception ex)
        {
            Log.Write("Failed to apply Inventory API patches", ex);
        }
    }

    // ---- Storage for pending results ----
    private static readonly ConcurrentDictionary<Inventory, object> _pendingAdd = new();
    private static readonly ConcurrentDictionary<Inventory, object> _pendingTake = new();

    // ---- TryAddItem ----
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.TryAddItem))]
    private static bool TryAddItemPrefix(Inventory __instance, ItemStack stackToAdd, bool ignoreHotbar, ref bool __result)
    {
        var args = new InventoryAddItemEventArgs(__instance, stackToAdd, ignoreHotbar);
        InventoryEvents.InvokeBeforeAddItem(args);
        if (args.Cancelled)
        {
            __result = false;
            return false;
        }
        _pendingAdd[__instance] = null;
        return true;
    }

    // ---- TryTakeItem ----
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.TryTakeItem))]
    private static bool TryTakeItemPrefix(Inventory __instance, ItemStack stackToTake, out ItemStack takenStack, ref bool __result)
    {
        takenStack = null!;
        var args = new InventoryTakeItemEventArgs(__instance, stackToTake);
        InventoryEvents.InvokeBeforeTakeItem(args);
        if (args.Cancelled)
        {
            __result = false;
            return false;
        }
        _pendingTake[__instance] = null;
        return true;
    }

    // ---- ClearAll ----
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.ClearAll))]
    private static void ClearAllPostfix(Inventory __instance)
    {
        InventoryEvents.InvokeCleared(new InventoryClearedEventArgs(__instance));
    }

    // ---- Sort ----
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.Sort))]
    private static void SortPostfix(Inventory __instance)
    {
        InventoryEvents.InvokeSorted(new InventorySortedEventArgs(__instance));
    }

    // ---- Player Open/Close ----
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.OpenInventory))]
    private static void OpenInventoryPostfix(PlayerEntity __instance,
        List<CraftingStation>? stations, Inventory? chest, Catalogue? cat, BlockEntityChest? chestEntity)
    {
        var args = new PlayerInventoryOpenEventArgs(__instance, __instance.inventory.inventory,
            stations, chest, cat, chestEntity);
        InventoryEvents.InvokePlayerInventoryOpened(args);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.CloseInventory))]
    private static void CloseInventoryPostfix(PlayerEntity __instance)
    {
        var args = new PlayerInventoryCloseEventArgs(__instance, __instance.inventory.inventory);
        InventoryEvents.InvokePlayerInventoryClosed(args);
    }
}