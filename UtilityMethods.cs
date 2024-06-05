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

    internal static void RegisterContainer(Container container)
    {
        if (!RegisteredContainers.Contains(container))
        {
            RegisteredContainers.Add(container);
        }
    }

    internal static void DeRegisterContainer(Container container)
    {
        if (RegisteredContainers.Contains(container))
        {
            RegisteredContainers.Remove(container);
        }
    }

    internal static void TryFeedAnimal(MonsterAI animalAI, Tameable tamable, Character character)
    {
        List<Container> nearbyContainers = GetNearbyContainers(character.transform.position, PetPantryPlugin.ContainerRange.Value);

        foreach (Container container in nearbyContainers)
        {
            if (PetPantryPlugin.RequireOnlyFood.Value == PetPantryPlugin.Toggle.On && container.GetInventory().GetAllItems().Any(item => !animalAI.m_consumeItems.Exists(ci => ci.m_itemData.m_shared.m_name == item.m_shared.m_name)))
                continue;

            foreach (ItemDrop.ItemData item in container.GetInventory().GetAllItems().Where(item => IsFoodForAnimal(item, animalAI)))
            {
                FeedAnimal(animalAI, tamable, character, container, item);
                return;
            }
        }
    }

    private static List<Container> GetNearbyContainers(Vector3 position, float range)
    {
        return RegisteredContainers.Where(c => Vector3.Distance(c.transform.position, position) <= range).ToList();
    }

    private static bool IsFoodForAnimal(ItemDrop.ItemData item, MonsterAI animalAI)
    {
        return animalAI.m_consumeItems.Exists(ci => ci.m_itemData.m_shared.m_name == item.m_shared.m_name);
    }

    private static void FeedAnimal(MonsterAI animalAI, Tameable tamable, Character character, Container container, ItemDrop.ItemData item)
    {
        animalAI.m_onConsumedItem?.Invoke(null);
        (character as Humanoid).m_consumeItemEffects.Create(character.transform.position, Quaternion.identity);
        animalAI.m_animator.SetTrigger("consume");
        container.GetInventory().RemoveItem(item.m_shared.m_name, 1);
        tamable.ResetFeedingTimer();
    }
}