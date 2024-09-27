using dnlib.DotNet.Emit;
using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zylofuscatorgui.Protection.Renamer
{
    internal class RenamerPhase
    {
        public RenameMode selectedRenameMode = RenameMode.Chinese;
        public enum RenameMode
        {
            Ascii,
            Key,
            Normal,
            Chinese,
            Spam
        }

        private const string Ascii = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public const string ChineseCharacters = "站端竵窠竮童竤竦竩竳竲竡竭竵竰竷竣竫競竴竨竢竃竺竽竅竐竍竰";

        private static readonly Random Random = new Random();

        private static readonly string[] NormalNameStrings =
        {
            "HasPermission", "HasPermissions", "GetPermissions", "GetOpenWindows", "EnumWindows", "GetWindowText",
            "GetWindowTextLength", "IsWindowVisible", "GetShellWindow", "Awake", "FixedUpdate", "add_OnRockedInitialized",
            "remove_OnRockedInitialized", "Awake", "Initialize", "Translate", "Reload", "<Initialize>b__13_0", "Initialize",
            "FixedUpdate", "Start", "checkTimerRestart", "QueueOnMainThread", "QueueOnMainThread", "RunAsync", "RunAction",
            "Awake", "FixedUpdate", "IsUri", "GetTypes", "GetTypesFromParentClass", "GetTypesFromParentClass",
            "GetTypesFromInterface", "GetTypesFromInterface", "get_Timeout", "set_Timeout", "GetWebRequest",
            "get_SteamID64", "set_SteamID64", "get_SteamID", "set_SteamID", "get_OnlineState", "set_OnlineState",
            "get_StateMessage", "set_StateMessage", "get_PrivacyState", "set_PrivacyState", "get_VisibilityState",
            "set_VisibilityState", "get_AvatarIcon", "set_AvatarIcon", "get_AvatarMedium", "set_AvatarMedium",
            "get_AvatarFull", "set_AvatarFull", "get_IsVacBanned", "set_IsVacBanned", "get_TradeBanState",
            "set_TradeBanState", "get_IsLimitedAccount", "set_IsLimitedAccount", "get_CustomURL", "set_CustomURL",
            "get_MemberSince", "set_MemberSince", "get_HoursPlayedLastTwoWeeks", "set_HoursPlayedLastTwoWeeks",
            "get_Headline", "set_Headline", "get_Location", "set_Location", "get_RealName", "set_RealName", "get_Summary",
            "set_Summary", "get_MostPlayedGames", "set_MostPlayedGames", "get_Groups", "set_Groups", "Reload",
            "ParseString", "ParseDateTime", "ParseDouble", "ParseUInt16", "ParseUInt32", "ParseUInt64", "ParseBool",
            "ParseUri", "IsValidCSteamID", "LoadDefaults", "LoadDefaults", "get_Clients", "Awake", "handleConnection",
            "FixedUpdate", "Broadcast", "OnDestroy", "Read", "Send", "<Awake>b__8_0", "get_InstanceID", "set_InstanceID",
            "get_ConnectedTime", "set_ConnectedTime", "Send", "Read", "Close", "get_Address", "get_Instance",
            "set_Instance", "Save", "Load", "Unload", "Load", "Save", "Load", "get_Configuration", "LoadPlugin",
            "<.ctor>b__3_0", "<LoadPlugin>b__4_0", "add_OnPluginUnloading", "remove_OnPluginUnloading",
            "add_OnPluginLoading", "remove_OnPluginLoading", "get_Translations", "get_State", "get_Assembly",
            "set_Assembly", "get_Directory", "set_Directory", "get_Name", "set_Name", "get_DefaultTranslations",
            "IsDependencyLoaded", "ExecuteDependencyCode", "Translate", "ReloadPlugin", "LoadPlugin", "UnloadPlugin",
            "OnEnable", "OnDisable", "Load", "Unload", "TryAddComponent", "TryRemoveComponent", "add_OnPluginsLoaded",
            "remove_OnPluginsLoaded", "get_Plugins", "GetPlugins", "GetPlugin", "GetPlugin", "Awake", "Start",
            "GetMainTypeFromAssembly", "loadPlugins", "unloadPlugins", "Reload", "GetAssembliesFromDirectory",
            "LoadAssembliesFromDirectory", "<Awake>b__12_0", "GetGroupsByIds", "GetParentGroups", "HasPermission",
            "GetGroup", "RemovePlayerFromGroup", "AddPlayerToGroup", "DeleteGroup", "SaveGroup", "AddGroup", "GetGroups",
            "GetPermissions", "GetPermissions", "<GetGroups>b__11_3", "Start", "FixedUpdate", "Reload", "HasPermission",
            "GetGroups", "GetPermissions", "GetPermissions", "AddPlayerToGroup", "RemovePlayerFromGroup", "GetGroup",
            "SaveGroup", "AddGroup", "DeleteGroup", "DeleteGroup", "<FixedUpdate>b__4_0", "Enqueue", "_Logger_DoWork",
            "processLog", "Log", "Log", "var_dump", "LogWarning", "LogError", "LogError", "Log", "LogException",
            "ProcessInternalLog", "logRCON", "writeToConsole", "ProcessLog", "ExternalLog", "Invoke", "_invoke",
            "TryInvoke", "get_Aliases", "get_AllowedCaller", "get_Help", "get_Name", "get_Permissions", "get_Syntax",
            "Execute", "get_Aliases", "get_AllowedCaller", "get_Help", "get_Name", "get_Permissions", "get_Syntax",
            "Execute", "get_Aliases", "get_AllowedCaller", "get_Help", "get_Name", "get_Permissions", "get_Syntax",
            "Execute", "get_Name", "set_Name", "get_Name", "set_Name", "get_Name", "get_Help", "get_Syntax",
            "get_AllowedCaller", "get_Commands", "set_Commands", "add_OnExecuteCommand", "remove_OnExecuteCommand",
            "Reload", "Awake", "checkCommandMappings", "checkDuplicateCommandMappings", "Plugins_OnPluginsLoaded",
            "GetCommand", "GetCommand", "getCommandIdentity", "getCommandType", "Register", "Register", "Register",
            "DeregisterFromAssembly", "GetCooldown", "SetCooldown", "Execute", "RegisterFromAssembly"
        };

        private static readonly Dictionary<string, string> Names = new Dictionary<string, string>();

        public static string RandomString(int length, string chars)
        {
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        private static string GetRandomName()
        {
            return NormalNameStrings[Random.Next(NormalNameStrings.Length)];
        }

        public static string GenerateString(RenameMode mode)
        {
            switch (mode)
            {
                case RenameMode.Ascii:
                    return RandomString(Random.Next(3, 12), Ascii);
                case RenameMode.Key:
                    return RandomString(16, Ascii);
                case RenameMode.Normal:
                    return GetRandomName();
                case RenameMode.Chinese:
                    return RandomString(Random.Next(3, 12), ChineseCharacters);
                case RenameMode.Spam:
                    return "https://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSG" + RandomString(5, ChineseCharacters) + "https://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSGhttps://github.com/ZyloSG";

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static void ExecuteClassRenaming(ModuleDefMD module)
        {
            foreach (TypeDef type in module.GetTypes())
            {
                if (type.IsGlobalModuleType)
                {
                    continue;
                }

                if (type.Name == "GeneratedInternalTypeHelper" || type.Name == "Resources" || type.Name == "Settings")
                {
                    continue;
                }

                string nameValue;
                if (Names.TryGetValue(type.Name, out nameValue))
                {
                    type.Name = nameValue;
                }
                else
                {
                    RenamerPhase phase = new RenamerPhase();
                    string newName = GenerateString(phase.selectedRenameMode);
                    Names.Add(type.Name, newName);
                    type.Name = newName;
                }
            }

        }

        public static void ExecuteFieldRenaming(ModuleDefMD module)
        {
            foreach (TypeDef type in module.GetTypes())
            {
                if (type.IsGlobalModuleType)
                {
                    continue;
                }

                foreach (FieldDef field in type.Fields)
                {
                    string nameValue;
                    if (Names.TryGetValue(field.Name, out nameValue))
                    {
                        field.Name = nameValue;
                    }
                    else
                    {
                        RenamerPhase phase = new RenamerPhase();
                        string newName = GenerateString(phase.selectedRenameMode);
                        Names.Add(field.Name, newName);
                        field.Name = newName;
                    }
                }
            }
        }

        public static void ExecuteMethodRenaming(ModuleDefMD module)
        {
            foreach (TypeDef type in module.GetTypes())
            {   
                if (type.IsGlobalModuleType)
                {
                    continue;
                }

                if (type.Name == "Resources" || type.Name == "Settings")
                {
                    continue;
                }

                foreach (MethodDef method in type.Methods)
                {
                    if (method.IsConstructor)
                    {
                        continue;
                    }

                    string nameValue;
                    if (Names.TryGetValue(method.Name, out nameValue))
                    {
                        method.Name = nameValue;
                    }
                    else
                    {
                        RenamerPhase phase = new RenamerPhase();
                        string newName = GenerateString(phase.selectedRenameMode);
                        Names.Add(method.Name, newName);
                        method.Name = newName;
                    }
                }
            }
        }
    }
}
