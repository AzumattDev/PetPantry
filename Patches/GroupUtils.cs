/*using System.Collections.Generic;
using System.Linq;

namespace PetPantry.Patches;

public class GroupUtils
{
    // Get a list of all excluded groups for a container
    public static List<string> GetExcludedGroups(string container)
    {
        if (PetPantryPlugin.yamlData.TryGetValue(container, out Dictionary<string, List<string>> containerData))
        {
            if (containerData.TryGetValue("exclude", out List<string> excludeList))
            {
                return excludeList.Where(excludeItem => PetPantryPlugin.groups.ContainsKey(excludeItem)).ToList();
            }
        }

        return new List<string>();
    }

    public static bool IsGroupDefined(string groupName)
    {
        if (PetPantryPlugin.yamlData == null)
        {
            PetPantryPlugin.PetPantryLogger.LogError("yamlData is null. Make sure to call DeserializeYamlFile() before using IsGroupDefined.");
            return false;
        }

        bool groupInYaml = false;

        if (PetPantryPlugin.yamlData.TryGetValue("groups", out Dictionary<string, List<string>> groupsData))
        {
            groupInYaml = groupsData.ContainsKey(groupName);
        }

        // Check for the group in both yamlData and predefined groups
        return groupInYaml || PetPantryPlugin.groups.ContainsKey(groupName);
    }


    // Check if a group exists in the container data
    public static bool GroupExists(string groupName)
    {
        return PetPantryPlugin.groups.ContainsKey(groupName);
    }

    // Get a list of all groups in the container data
    public static List<string> GetAllGroups()
    {
        return PetPantryPlugin.groups.Keys.ToList();
    }

    // Get a list of all items in a group
    public static List<string> GetItemsInGroup(string groupName)
    {
        if (PetPantryPlugin.groups.TryGetValue(groupName, out HashSet<string> groupPrefabs))
        {
            return groupPrefabs.ToList();
        }

        return new List<string>();
    }
}*/