using AlluLoader.API.Events; 
using Allumeria.Blocks.Blocks;
using Allumeria.ChunkManagement;
using Allumeria.EntitySystem;
using Allumeria.EntitySystem.Components;
using Allumeria.EntitySystem.Entities;
using Allumeria.Items;
using HarmonyLib;
using OpenTK.Mathematics; 

namespace AlluLoader.API.Patches;

[HarmonyPatch(typeof(PlayerEntity))]
internal static class PlayerEntityPatches
{ 
    static PlayerEntityPatches()
    {
         
    } 

    // ========== HealthComponent.TakeDamage ==========
    [HarmonyPrefix]
    [HarmonyPatch(typeof(HealthComponent), nameof(HealthComponent.TakeDamage))]
    private static bool HealthTakeDamagePrefix(HealthComponent __instance, int amount, HealthComponent.DamageType damageType, bool stun, World world, bool ignoreIframes, ref bool __result)
    {
        if (__instance.parent is PlayerEntity player)
        {
            var args = new PlayerDamageEventArgs(player, amount, damageType, stun, world, ignoreIframes);
            PlayerEvents.InvokeBeforeDamage(args);
            if (args.WasCancelled)
            {
                __result = false;
                return false;  
            }
        }
        return true; 
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(HealthComponent), nameof(HealthComponent.TakeDamage))]
    private static void HealthTakeDamagePostfix(HealthComponent __instance, int amount, HealthComponent.DamageType damageType, bool stun, World world, bool ignoreIframes, bool __result)
    {
        if (__result && __instance.parent is PlayerEntity player)
        {
            var args = new PlayerDamageEventArgs(player, amount, damageType, stun, world, ignoreIframes);
            PlayerEvents.InvokeAfterDamage(args);
        }
    }

    // ========== Death ==========
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerEntity.Die))]
    private static void DiePostfix(PlayerEntity __instance, World world)
    {
        PlayerEvents.InvokeDeath(new PlayerDeathEventArgs(__instance, world));
    }

    // ========== Respawn ==========
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerEntity.Respawn), typeof(World))]
    private static void RespawnPostfix(PlayerEntity __instance, World world)
    {
        PlayerEvents.InvokeRespawn(new PlayerRespawnEventArgs(__instance, world, true, false));
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerEntity.Respawn), typeof(World), typeof(bool), typeof(bool))]
    private static void RespawnFullPostfix(PlayerEntity __instance, World world, bool reset, bool fromLogoff)
    {
        PlayerEvents.InvokeRespawn(new PlayerRespawnEventArgs(__instance, world, reset, fromLogoff));
    }

    // ========== Jump ==========
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerEntity.Jump))]
    private static void JumpPostfix(PlayerEntity __instance)
    {
        PlayerEvents.InvokeJump(new PlayerJumpEventArgs(__instance, false));
    } 

    // ========== Tick ==========
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerEntity.Tick))]
    private static void TickPrefix(PlayerEntity __instance, Chunk chunk, World world)
    {
        PlayerEvents.InvokePreTick(new PlayerTickEventArgs(__instance, chunk, world));
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerEntity.Tick))]
    private static void TickPostfix(PlayerEntity __instance, Chunk chunk, World world)
    {
        PlayerEvents.InvokeTick(new PlayerTickEventArgs(__instance, chunk, world));
    }

    // ========== Item Swapped ==========
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerEntity.OnHeldItemSwapped))]
    private static void OnHeldItemSwappedPostfix(PlayerEntity __instance, bool sendToServer)
    {
        // We need previous and new item. 
        var previous = __instance.previousHeldItem;
        var newItem = __instance.heldItem;
        PlayerEvents.InvokeItemSwapped(new PlayerItemSwappedEventArgs(__instance, previous, newItem, sendToServer));
    }

    // ========== Use Item (Left/Right Click) ==========
    // We intercept both left‑click use (heldItem.leftClickUse) and right‑click use (OnUse).
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerEntity.PlaceAndDestroy))]
    private static bool PlaceAndDestroyPrefix(PlayerEntity __instance, ChunkManager chunkManager, World world)
    {
         
        return true;  
    } 
    // 1. Right‑click use  
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Item), nameof(Item.OnUse))]
    private static bool ItemOnUsePrefix(Item __instance, PlayerEntity player, World world)
    {
        var args = new PlayerUseItemEventArgs(player, __instance, false, world);
        PlayerEvents.InvokeUseItem(args);
        if (args.Cancelled) return false; 
        return true;
    }

    // 2. Left‑click use
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Item), nameof(Item.OnLeftClickUse))]
    private static bool ItemOnLeftClickUsePrefix(Item __instance, PlayerEntity player, World world)
    {
        var args = new PlayerUseItemEventArgs(player, __instance, true, world);
        PlayerEvents.InvokeUseItem(args);
        if (args.Cancelled) return false;
        return true;
    }

    // ========== Attack Entity ==========
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerEntity.DamageEntity))]
    private static bool DamageEntityPrefix(PlayerEntity __instance, Entity punchedEntity, World world, ref int __result)
    {
        var args = new PlayerAttackEntityEventArgs(__instance, punchedEntity, world);
        PlayerEvents.InvokeAttackEntity(args);
        if (args.Cancelled)
        {
            __result = 70;  
            return false;  
        }
        return true;
    }

    // ========== Sweep Attack ==========
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerEntity.SweepAttack))]
    private static bool SweepAttackPrefix(PlayerEntity __instance, World world)
    {
        var args = new PlayerSweepAttackEventArgs(__instance, world);
        PlayerEvents.InvokeSweepAttack(args);
        if (args.Cancelled) return false;  
        return true;
    }

    // ========== Place Block ==========
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Block), nameof(Block.OnBreak))]
    private static bool BlockOnBreakPrefix(Block __instance, Entity entity, int x, int y, int z, World world, uint metadata)
    {
        if (entity is PlayerEntity player)
        {
            var args = new PlayerBreakBlockEventArgs(player, new Vector3i(x, y, z), __instance, world);
            PlayerEvents.InvokeBreakBlock(args);
            if (args.Cancelled) return false; 
        }
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Block), nameof(Block.OnPlace))]
    private static bool BlockOnPlacePrefix(Block __instance, Entity entity, int x, int y, int z, World world)
    {
        if (entity is PlayerEntity player)
        {
            var args = new PlayerPlaceBlockEventArgs(player, new Vector3i(x, y, z), __instance, world);
            PlayerEvents.InvokePlaceBlock(args);
            if (args.Cancelled) return false;  
        }
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Block), nameof(Block.OnRightClick))]
    private static bool BlockOnRightClickPrefix(Block __instance, PlayerEntity player, int x, int y, int z, World world, uint metadata)
    {
        var args = new PlayerInteractBlockEventArgs(player, new Vector3i(x, y, z), __instance, world);
        PlayerEvents.InvokeInteractBlock(args);
        if (args.Cancelled) return false;  
        return true;
    }

    // ========== Drop Item ==========
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerEntity.DropItem))]
    private static bool DropItemPrefix(PlayerEntity __instance, ItemStack stack, World world)
    {
        var args = new PlayerDropItemEventArgs(__instance, stack, world);
        PlayerEvents.InvokeDropItem(args);
        if (args.Cancelled) return false;  
        return true;
    }

     
}
