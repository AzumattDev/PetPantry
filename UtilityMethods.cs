using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace PetPantry;

public class UtilityMethods
{
    internal static readonly List<Container> RegisteredContainers = [];
    internal static readonly Dictionary<Character, float> LastFeedCheckTimes = new();
    private const string NameFilter = "Treasure";
    private const string ConsumeTrigger = "consume";

    internal static void RegisterContainer(Container container)
    {
        if (container != null && !RegisteredContainers.Contains(container))
        {
            RegisteredContainers.Add(container);
        }
    }

    internal static void DeRegisterContainer(Container container)
    {
        if (container != null && RegisteredContainers.Contains(container))
        {
            RegisteredContainers.Remove(container);
        }
    }

    internal static bool VerifyContainer(Container container)
    {
        return container != null && !container.name.StartsWith(NameFilter) && container.GetInventory() != null && container.m_nview.IsValid() && container.m_nview.GetZDO().GetLong(ZDOVars.s_creator) != 0L;
    }

    internal static void TryFeedAnimal(MonsterAI animalAI, Tameable tamable, Character character)
    {
        List<Container> nearbyContainers = GetNearbyContainers(character.transform.position, PetPantryPlugin.ContainerRange.Value);

        foreach (Container container in nearbyContainers)
        {
            if (container.GetInventory() == null)
                continue;
            if (PetPantryPlugin.RequireOnlyFood.Value == PetPantryPlugin.Toggle.On && container.GetInventory().GetAllItems().Any(item => !animalAI.m_consumeItems.Exists(ci => ci.m_itemData.m_shared.m_name == item.m_shared.m_name)))
                continue;

            foreach (ItemDrop.ItemData? item in container.GetInventory().GetAllItems().Where(item => IsFoodForAnimal(item, animalAI)))
            {
                FeedAnimal(animalAI, tamable, character, container, item);
                return;
            }
        }
    }

    private static List<Container> GetNearbyContainers(Vector3 position, float range)
    {
        return RegisteredContainers.Where(c => c != null && Vector3.Distance(c.transform.position, position) <= range).ToList();
    }

    private static bool IsFoodForAnimal(ItemDrop.ItemData? item, MonsterAI animalAI)
    {
        if (animalAI.m_consumeItems == null || animalAI.m_consumeItems.Count == 0)
        {
            return false;
        }

        return animalAI != null && item != null && animalAI.m_consumeItems.Exists(ci => ci.m_itemData.m_shared.m_name == item.m_shared.m_name);
    }

    private static void FeedAnimal(MonsterAI animalAI, Tameable tamable, Character character, Container container, ItemDrop.ItemData? item)
    {
        if (animalAI == null || tamable == null || character == null || container == null || item == null) return;
        animalAI.m_onConsumedItem?.Invoke(null);
        Humanoid? humanoid = character as Humanoid;
        humanoid?.m_consumeItemEffects.Create(character.transform.position, Quaternion.identity);
        animalAI.m_animator.SetTrigger(ConsumeTrigger);
        container.GetInventory().RemoveItem(item.m_shared.m_name, 1);
        container.Save();
        tamable.ResetFeedingTimer();
    }

    internal static void SafeRemoveFeedCheckTime(Character character)
    {
        if (character == null)
            return;
        if (LastFeedCheckTimes.ContainsKey(character))
            LastFeedCheckTimes.Remove(character);
    }
}