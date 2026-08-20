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
    [HarmonyPrefix]
    [HarmonyPatch(typeof(HealthComponent), nameof(HealthComponent.TakeDamage))]
    private static bool HealthTakeDamagePrefix(HealthComponent __instance, int amount,
        HealthComponent.DamageType damageType, bool stun, World world, bool ignoreIframes,
        ref bool __result, out PlayerDamageEventArgs? __state)
    {
        __state = null;
        if (__instance.parent is not PlayerEntity player)
            return true;

        __state = new PlayerDamageEventArgs(player, amount, damageType, stun, world, ignoreIframes);
        PlayerEvents.InvokeBeforeDamage(__state);
        if (!__state.WasCancelled)
            return true;

        __result = false;
        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(HealthComponent), nameof(HealthComponent.TakeDamage))]
    private static void HealthTakeDamagePostfix(PlayerDamageEventArgs? __state, bool __result)
    {
        if (__state is not null && !__state.WasCancelled && __result)
            PlayerEvents.InvokeAfterDamage(__state);
    }

    [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.Die))]
    private static class PlayerDiePatch
    {
        [HarmonyPrefix]
        private static bool Prefix(PlayerEntity __instance, World world, out PlayerDeathEventArgs __state)
        {
            __state = new PlayerDeathEventArgs(__instance, world);
            PlayerEvents.InvokeBeforeDeath(__state);
            return !__state.Cancelled;
        }

        [HarmonyPostfix]
        private static void Postfix(PlayerDeathEventArgs __state)
        {
            if (!__state.Cancelled)
                PlayerEvents.InvokeAfterDeath(__state);
        }
    }

    [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.Respawn),
        new[] { typeof(World), typeof(bool), typeof(bool) })]
    private static class PlayerRespawnPatch
    {
        [HarmonyPrefix]
        private static bool Prefix(PlayerEntity __instance, World world, ref bool reset,
            ref bool fromLogoff, out PlayerRespawnEventArgs __state)
        {
            __state = new PlayerRespawnEventArgs(__instance, world, reset, fromLogoff);
            PlayerEvents.InvokeBeforeRespawn(__state);
            reset = __state.Reset;
            fromLogoff = __state.FromLogoff;
            return !__state.Cancelled;
        }

        [HarmonyPostfix]
        private static void Postfix(PlayerRespawnEventArgs __state)
        {
            if (!__state.Cancelled)
                PlayerEvents.InvokeAfterRespawn(__state);
        }
    }

    [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.Teleport),
        new[] { typeof(float), typeof(float), typeof(float) })]
    private static class PlayerTeleportCoordinatesPatch
    {
        [HarmonyPrefix]
        private static bool Prefix(PlayerEntity __instance, ref float x, ref float y, ref float z,
            out PlayerTeleportEventArgs __state)
        {
            __state = new PlayerTeleportEventArgs(__instance, __instance.position, new Vector3(x, y, z));
            PlayerEvents.InvokeBeforeTeleport(__state);
            x = __state.Destination.X;
            y = __state.Destination.Y;
            z = __state.Destination.Z;
            return !__state.Cancelled;
        }

        [HarmonyPostfix]
        private static void Postfix(PlayerTeleportEventArgs __state)
        {
            if (!__state.Cancelled)
                PlayerEvents.InvokeAfterTeleport(__state);
        }
    }

    [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.Teleport), new[] { typeof(Vector3) })]
    private static class PlayerTeleportVectorPatch
    {
        [HarmonyPrefix]
        private static bool Prefix(PlayerEntity __instance, ref Vector3 pos,
            out PlayerTeleportEventArgs __state)
        {
            __state = new PlayerTeleportEventArgs(__instance, __instance.position, pos);
            PlayerEvents.InvokeBeforeTeleport(__state);
            pos = __state.Destination;
            return !__state.Cancelled;
        }

        [HarmonyPostfix]
        private static void Postfix(PlayerTeleportEventArgs __state)
        {
            if (!__state.Cancelled)
                PlayerEvents.InvokeAfterTeleport(__state);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerEntity.Jump))]
    private static bool JumpPrefix(PlayerEntity __instance)
    {
        var args = new PlayerJumpEventArgs(__instance, false);
        PlayerEvents.InvokeJump(args);
        return !args.Cancelled;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerEntity.Tick))]
    private static void TickPrefix(PlayerEntity __instance, Chunk chunk, World world) =>
        PlayerEvents.InvokePreTick(new PlayerTickEventArgs(__instance, chunk, world));

    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerEntity.Tick))]
    private static void TickPostfix(PlayerEntity __instance, Chunk chunk, World world) =>
        PlayerEvents.InvokeTick(new PlayerTickEventArgs(__instance, chunk, world));

    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerEntity.OnHeldItemSwapped))]
    private static bool OnHeldItemSwappedPrefix(PlayerEntity __instance, bool sendToServer)
    {
        var args = new PlayerItemSwappedEventArgs(
            __instance, __instance.previousHeldItem, __instance.heldItem, sendToServer);
        PlayerEvents.InvokeItemSwapped(args);
        return !args.Cancelled;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Item), nameof(Item.OnUse))]
    private static bool ItemOnUsePrefix(Item __instance, PlayerEntity player, World world)
    {
        var args = new PlayerUseItemEventArgs(player, __instance, false, world);
        PlayerEvents.InvokeUseItem(args);
        return !args.Cancelled;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Item), nameof(Item.OnLeftClickUse))]
    private static bool ItemOnLeftClickUsePrefix(Item __instance, PlayerEntity player, World world)
    {
        var args = new PlayerUseItemEventArgs(player, __instance, true, world);
        PlayerEvents.InvokeUseItem(args);
        return !args.Cancelled;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerEntity.DamageEntity))]
    private static bool DamageEntityPrefix(PlayerEntity __instance, Entity punchedEntity, World world,
        ref int __result, out PlayerAttackEntityEventArgs __state)
    {
        __state = new PlayerAttackEntityEventArgs(__instance, punchedEntity, world);
        PlayerEvents.InvokeAttackEntity(__state);
        if (!__state.Cancelled)
            return true;

        __result = 70;
        __state.AttackDelay = __result;
        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerEntity.DamageEntity))]
    private static void DamageEntityPostfix(PlayerAttackEntityEventArgs __state, int __result)
    {
        if (__state.Cancelled)
            return;
        __state.AttackDelay = __result;
        PlayerEvents.InvokeAfterAttackEntity(__state);
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerEntity.SweepAttack))]
    private static bool SweepAttackPrefix(PlayerEntity __instance, World world)
    {
        var args = new PlayerSweepAttackEventArgs(__instance, world);
        PlayerEvents.InvokeSweepAttack(args);
        return !args.Cancelled;
    }

    // BreakBlockAt. Block.OnBreak runs after the block is removed.
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerEntity.BreakBlockAt))]
    private static bool BreakBlockPrefix(PlayerEntity __instance, Vector3i blockBreakPosition, World world)
    {
        Block block = world.chunkManager.GetBlock(
            blockBreakPosition.X, blockBreakPosition.Y, blockBreakPosition.Z);
        var args = new PlayerBreakBlockEventArgs(__instance, blockBreakPosition, block, world);
        PlayerEvents.InvokeBreakBlock(args);
        return !args.Cancelled;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Block), nameof(Block.OnPlace))]
    private static bool BlockOnPlacePrefix(Block __instance, PlayerEntity player,
        int x, int y, int z, World world)
    {
        var args = new PlayerPlaceBlockEventArgs(player, new Vector3i(x, y, z), __instance, world);
        PlayerEvents.InvokePlaceBlock(args);
        return !args.Cancelled;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Block), nameof(Block.OnRightClick))]
    private static bool BlockOnRightClickPrefix(Block __instance, PlayerEntity player,
        int x, int y, int z, World world, uint metadata)
    {
        var args = new PlayerInteractBlockEventArgs(player, new Vector3i(x, y, z), __instance, world);
        PlayerEvents.InvokeInteractBlock(args);
        return !args.Cancelled;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerEntity.DropItem))]
    private static bool DropItemPrefix(PlayerEntity __instance, ItemStack stack, World world,
        out PlayerDropItemEventArgs __state)
    {
        __state = new PlayerDropItemEventArgs(__instance, stack, world);
        PlayerEvents.InvokeDropItem(__state);
        return !__state.Cancelled;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerEntity.DropItem))]
    private static void DropItemPostfix(PlayerDropItemEventArgs __state)
    {
        if (!__state.Cancelled)
            PlayerEvents.InvokeAfterDropItem(__state);
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerEntity.TryConsumeStamina))]
    private static bool TryConsumeStaminaPrefix(PlayerEntity __instance, ref int amount,
        ref bool __result, out PlayerStaminaConsumeEventArgs __state)
    {
        __state = new PlayerStaminaConsumeEventArgs(
            __instance, amount, __instance.stamina, __instance.maxStamina);
        PlayerEvents.InvokeBeforeConsumeStamina(__state);
        amount = __state.Amount;
        if (!__state.Cancelled)
            return true;

        __result = false;
        __state.Success = false;
        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerEntity.TryConsumeStamina))]
    private static void TryConsumeStaminaPostfix(PlayerEntity __instance,
        PlayerStaminaConsumeEventArgs __state, bool __result)
    {
        if (__state.Cancelled)
            return;
        __state.Success = __result;
        __state.NewStamina = __instance.stamina;
        PlayerEvents.InvokeAfterConsumeStamina(__state);
        if (__state.OldStamina != __state.NewStamina)
            PlayerEvents.InvokeStaminaChanged(new PlayerStaminaEventArgs(
                __instance, __state.OldStamina, __state.NewStamina, __instance.maxStamina));
    }
}