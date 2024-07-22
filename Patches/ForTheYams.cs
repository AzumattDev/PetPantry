/*using System.Collections.Generic;
using System.IO;

namespace PetPantry.Patches;

public static class YamlUtils
{
    internal static void ReadYaml(string yamlInput)
    {
        IDeserializer deserializer = new DeserializerBuilder().Build();
        PetPantryPlugin.yamlData = deserializer.Deserialize<Dictionary<string, Dictionary<string, List<string>>>>(yamlInput);
        PetPantryPlugin.PetPantryLogger.LogDebug($"yamlData:\n{yamlInput}");
    }

    internal static void ParseGroups()
    {
        if (PetPantryPlugin.yamlData.TryGetValue("groups", out Dictionary<string, List<string>> groupData))
        {
            foreach (KeyValuePair<string, List<string>> group in groupData)
            {
                PetPantryPlugin.groups[group.Key] = new HashSet<string>(group.Value);
            }
        }
    }
    public static void WriteYaml(string filePath)
    {
        ISerializer serializer = new SerializerBuilder().Build();
        using StreamWriter output = new(filePath);
        serializer.Serialize(output, PetPantryPlugin.yamlData);

        // Serialize the data again to YAML format
        string serializedData = serializer.Serialize(PetPantryPlugin.yamlData);

        // Append the serialized YAML data to the file
        File.AppendAllText(filePath, serializedData);
    }
}*/