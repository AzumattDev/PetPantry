using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;
using ServerSync;
using UnityEngine;

namespace PetPantry
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class PetPantryPlugin : BaseUnityPlugin
    {
        internal const string ModName = "PetPantry";
        internal const string ModVersion = "1.0.4";
        internal const string Author = "Azumatt";
        private const string ModGUID = $"{Author}.{ModName}";
        private static string ConfigFileName = $"{ModGUID}.cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;
        internal static string ConnectionError = "";
        private readonly Harmony _harmony = new(ModGUID);
        public static readonly ManualLogSource PetPantryLogger = BepInEx.Logging.Logger.CreateLogSource(ModName);
        private static readonly ConfigSync ConfigSync = new(ModGUID) { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

        public enum Toggle
        {
            On = 1,
            Off = 0
        }

        public void Awake()
        {
            _serverConfigLocked = config("1 - General", "Lock Configuration", Toggle.On, "If on, the configuration is locked and can be changed by server admins only.");
            _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

            ContainerRange = config("1 - General", "ContainerRange", 15f, "Defines the range in meters within which containers are checked for food items.");
            RequireOnlyFood = config("1 - General", "Require Only Food", Toggle.Off, "Ensures only containers with acceptable food items are considered for feeding.");
            FeedCheckCooldown = config("1 - General", "FeedCheckCooldown", 10f, "Cooldown in seconds between feed checks. Checks only occur when the pet is hungry.");
            DisableFeeding = config("2 - Feeding", "Disable Feeding", Toggle.Off, "Disable feeding when the pet is hungry. Global toggle, disables for everyone (essentially disables the mod features)");

            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
            SetupWatcher();
        }

        private void OnDestroy()
        {
            Config.Save();
        }

        private void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                PetPantryLogger.LogDebug("ReadConfigValues called");
                Config.Reload();
            }
            catch
            {
                PetPantryLogger.LogError($"There was an issue loading your {ConfigFileName}");
                PetPantryLogger.LogError("Please check your config entries for spelling and format!");
            }
        }


        #region ConfigOptions

        private static ConfigEntry<Toggle> _serverConfigLocked = null!;
        public static ConfigEntry<float> ContainerRange = null!;
        public static ConfigEntry<Toggle> RequireOnlyFood = null!;
        public static ConfigEntry<float> FeedCheckCooldown = null!;
        public static ConfigEntry<Toggle> DisableFeeding = null!;

        private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
        {
            ConfigDescription extendedDescription = new(description.Description + (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"), description.AcceptableValues, description.Tags);
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, extendedDescription);
            //var configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        private ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true)
        {
            return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        }

        private class ConfigurationManagerAttributes
        {
            [UsedImplicitly] public int? Order = null!;
            [UsedImplicitly] public bool? Browsable = null!;
            [UsedImplicitly] public string? Category = null!;
            [UsedImplicitly] public Action<ConfigEntryBase>? CustomDrawer = null!;
        }

        #endregion
    }

    public static class LoggerExtensions
    {
        public static void LogDebugIfDebug(this ManualLogSource logger, string message)
        {
#if DEBUG
            
            logger.LogDebug(message);
#endif
        }

        public static void LogErrorIfDebug(this ManualLogSource logger, string message)
        {
#if DEBUG
            logger.LogError(message);
#endif
        }
    }
    
    public static class ToggleExtentions
    {
        public static bool IsOn(this PetPantryPlugin.Toggle value)
        {
            return value == PetPantryPlugin.Toggle.On;
        }

        public static bool IsOff(this PetPantryPlugin.Toggle value)
        {
            return value == PetPantryPlugin.Toggle.Off;
        }
    }
}