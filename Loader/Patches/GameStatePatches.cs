using AlluLoader.Events;
using Allumeria.Networking;
using HarmonyLib;

namespace AlluLoader.Patches;

[HarmonyPatch]
internal static class GameStatePatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(InternalGameState), nameof(InternalGameState.Setup))]
    private static void InternalSetupPostfix(InternalGameState __instance) =>
        GameStateEvents.InvokeInternalStateSetup(new InternalGameStateEventArgs(__instance));

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InternalGameState), nameof(InternalGameState.Shutdown))]
    private static void InternalShutdownPrefix(InternalGameState __instance) =>
        GameStateEvents.InvokeInternalStateShuttingDown(new InternalGameStateEventArgs(__instance));

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ClientGameState), nameof(ClientGameState.Setup))]
    private static void ClientSetupPostfix(ClientGameState __instance) =>
        GameStateEvents.InvokeClientStateSetup(new ClientGameStateEventArgs(__instance));

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ClientGameState), nameof(ClientGameState.Shutdown))]
    private static void ClientShutdownPrefix(ClientGameState __instance) =>
        GameStateEvents.InvokeClientStateShuttingDown(new ClientGameStateEventArgs(__instance));
}