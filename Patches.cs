using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace PetPantry;

[HarmonyPatch(typeof(Container), nameof(Container.Awake))]
public static class Container_Awake_Patch
{
    private static void Postfix(Container __instance)
    {
        if (__instance.name.StartsWith("Treasure") || __instance.GetInventory() == null || !__instance.m_nview.IsValid() || __instance.m_nview.GetZDO().GetLong("creator".GetStableHashCode()) == 0L)
            return;
        UtilityMethods.RegisterContainer(__instance);
    }
}

[HarmonyPatch(typeof(Container), nameof(Container.OnDestroyed))]
public static class Container_OnDestroy_Patch
{
    private static void Prefix(Container __instance)
    {
        if (__instance.name.StartsWith("Treasure") || __instance.GetInventory() == null || !__instance.m_nview.IsValid() || __instance.m_nview.GetZDO().GetLong("creator".GetStableHashCode()) == 0L)
            return;
        UtilityMethods.DeRegisterContainer(__instance);
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.UpdateTeleport))]
public static class PlayerUpdateTeleportPatchCleanupContainers
{
    public static void Prefix(float dt)
    {
        if (Player.m_localPlayer == null || !Player.m_localPlayer.m_teleporting)
            return;
        foreach (Container container in UtilityMethods.RegisteredContainers.ToList().Where(container => (!(container != null) || !(container.transform != null) ? 0 : (container.GetInventory() != null ? 1 : 0)) == 0).Where(container => container != null))
        {
            UtilityMethods.DeRegisterContainer(container);
        }
    }
}

[HarmonyPatch(typeof(Tameable), nameof(Tameable.IsHungry))]
public static class Tameable_IsHungry_Patch
{
    public static void Postfix(Tameable __instance, ref bool __result)
    {
        if (!__result) // If not hungry, return early
            return;

        if (__instance.m_monsterAI == null) return;
        float currentTime = Time.time;
        if (UtilityMethods.LastFeedCheckTimes.TryGetValue(__instance.m_character, out float lastCheckTime))
        {
            if (currentTime - lastCheckTime < PetPantryPlugin.FeedCheckCooldown.Value)
                return;
        }

        UtilityMethods.LastFeedCheckTimes[__instance.m_character] = currentTime;
        UtilityMethods.TryFeedAnimal(__instance.m_monsterAI, __instance, __instance.m_character);
    }
}