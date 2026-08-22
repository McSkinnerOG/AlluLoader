using AlluLoader.Events;
using Allumeria.ChunkManagement;
using Allumeria.EntitySystem;
using Allumeria.EntitySystem.Components;
using HarmonyLib;

namespace AlluLoader.Patches;

[HarmonyPatch]
internal static class EntityPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Entity), nameof(Entity.OnRemove))]
    private static void OnRemovePrefix(Entity __instance, World world) =>
        EntityEvents.InvokeRemoving(new EntityWorldEventArgs(__instance, world));

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Entity), nameof(Entity.OnRemove))]
    private static void OnRemovePostfix(Entity __instance, World world) =>
        EntityEvents.InvokeRemoved(new EntityWorldEventArgs(__instance, world));

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Entity), nameof(Entity.AddComponent))]
    private static void AddComponentPrefix(Entity __instance, EntityComponent component) =>
        EntityEvents.InvokeComponentAdding(new EntityComponentEventArgs(__instance, component));

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Entity), nameof(Entity.AddComponent))]
    private static void AddComponentPostfix(Entity __instance, EntityComponent component) =>
        EntityEvents.InvokeComponentAdded(new EntityComponentEventArgs(__instance, component));
}