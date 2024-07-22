using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace PetPantry.Patches;

[HarmonyPatch(typeof(Container), nameof(Container.Awake))]
public static class Container_Awake_Patch
{
    private static void Postfix(Container __instance)
    {
        if (!UtilityMethods.VerifyValid(__instance))
            return;
        UtilityMethods.RegisterContainer(__instance);
    }
}

[HarmonyPatch(typeof(Container), nameof(Container.OnDestroyed))]
public static class Container_OnDestroy_Patch
{
    private static void Prefix(Container __instance)
    {
        if (!UtilityMethods.VerifyValid(__instance))
            return;
        UtilityMethods.DeRegisterContainer(__instance);
    }
}

[HarmonyPatch(typeof(WearNTear), nameof(WearNTear.OnDestroy))]
static class WearNTearOnDestroyPatch
{
    static void Prefix(WearNTear __instance)
    {
        Container[]? container = __instance.GetComponentsInChildren<Container>();
        Container[]? parentContainer = __instance.GetComponentsInParent<Container>();
        if (container.Length > 0)
        {
            foreach (Container c in container)
            {
                UtilityMethods.DeRegisterContainer(c);
            }
        }

        if (parentContainer.Length <= 0) return;
        {
            foreach (Container c in parentContainer)
            {
                UtilityMethods.DeRegisterContainer(c);
            }
        }
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
        
        UtilityMethods.LastFeedCheckTimes.Clear();
    }
}

[HarmonyPatch(typeof(Tameable), nameof(Tameable.IsHungry))]
public static class Tameable_IsHungry_Patch
{
    public static void Postfix(Tameable __instance, ref bool __result)
    {
        if (!__result || !__instance.m_nview.IsOwner()) // If not hungry, return early
            return;

        if (__instance.m_monsterAI == null || __instance.m_character == null) return;
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

[HarmonyPatch(typeof(Tameable), nameof(Tameable.OnDeath))]
public static class Tameable_OnDeath_Patch
{
    public static void Prefix(Tameable __instance)
    {
        if (__instance.m_character == null)
            return;
        UtilityMethods.SafeRemoveFeedCheckTime(__instance.m_character);
    }
}

[HarmonyPatch(typeof(Character), nameof(Character.OnDestroy))]
static class CharacterOnDestroyPatch
{
    static void Prefix(Character __instance)
    {
        UtilityMethods.SafeRemoveFeedCheckTime(__instance);
    }
}