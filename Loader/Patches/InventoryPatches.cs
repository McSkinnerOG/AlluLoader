using AlluLoader.API.Events;
using Allumeria.Blocks.BlockEntities;
using Allumeria.EntitySystem.Entities;
using Allumeria.Items;
using Allumeria.Items.Crafting;
using HarmonyLib;

namespace AlluLoader.API.Patches;

[HarmonyPatch]
internal static class InventoryPatches
{
    // ---- TryAddItem ----
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.TryAddItem))]
    private static bool TryAddItemPrefix(Inventory __instance, ItemStack stackToAdd,
        ref ItemStack newStack, bool ignoreHotbar, ref bool __result,
        out InventoryAddItemEventArgs __state)
    {
        __state = new InventoryAddItemEventArgs(__instance, stackToAdd, ignoreHotbar);
        InventoryEvents.InvokeBeforeAddItem(__state);
        if (__state.Cancelled)
        {
            newStack = stackToAdd;
            __result = false;
            return false;
        }
        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.TryAddItem))]
    private static void TryAddItemPostfix(ref ItemStack newStack, bool __result,
        InventoryAddItemEventArgs __state)
    {
        __state.Success = __result;
        __state.Remainder = newStack;
        InventoryEvents.InvokeAfterAddItem(__state);
    }

    // ---- TryTakeItem ----
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.TryTakeItem))]
    private static bool TryTakeItemPrefix(Inventory __instance, ItemStack stackToTake,
        ref ItemStack takenStack, ref bool __result, out InventoryTakeItemEventArgs __state)
    {
        __state = new InventoryTakeItemEventArgs(__instance, stackToTake);
        InventoryEvents.InvokeBeforeTakeItem(__state);
        if (__state.Cancelled)
        {
            takenStack = new ItemStack(stackToTake.GetItem(), 0);
            __result = false;
            return false;
        }
        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.TryTakeItem))]
    private static void TryTakeItemPostfix(ref ItemStack takenStack, bool __result,
        InventoryTakeItemEventArgs __state)
    {
        __state.Success = __result;
        __state.TakenStack = takenStack;
        InventoryEvents.InvokeAfterTakeItem(__state);
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