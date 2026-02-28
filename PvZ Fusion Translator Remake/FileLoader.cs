using Il2Cpp;
using MelonLoader;
using PvZ_Fusion_Translator_Remake.AssetStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using UnityEngine;

namespace PvZ_Fusion_Translator_Remake
{
    internal class FileLoader
    {
        internal enum AssetType
        {
            Textures,
            Strings,
            Dumps,
            Almanac,
            Sprites
        }

        public static string GetAssetDir(AssetType assetType, Utils.LanguageEnum language = Utils.LanguageEnum.English)
        {
            string languagePath = (assetType != AssetType.Dumps) ? ("Localization//" + language.ToString()) : string.Empty;
            return Path.Combine(Core.Instance.assetDirectory, languagePath, assetType.ToString());
        }

        internal static void LoadStrings() => LoadStrings(Utils.Language);

        internal static void LoadStrings(Utils.LanguageEnum language)
        {
            string stringDir = GetAssetDir(AssetType.Strings, Utils.Language);

            if (!Directory.Exists(stringDir)) Directory.CreateDirectory(stringDir);

            try
            {
                foreach (string filePath in Directory.EnumerateFiles(stringDir, "*.json", SearchOption.AllDirectories))
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    string jsonString = File.ReadAllText(filePath);
                    Dictionary<string, string> json = [];

                    if(!fileName.EndsWith("travel_buffs"))
                    {
                        json = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
                    }

                    if (fileName.EndsWith("_strings"))
                    {
                        foreach(var pair in json)
                        {
                            StringStore.translationString[pair.Key] = pair.Value;
                        }
                    }
                    else if (fileName.EndsWith("_regexs"))
                    {
                        foreach(var pair in json)
                        {
                            StringStore.translationStringRegex[pair.Key] = pair.Value;
                        }
                    }
                    else if(fileName.EndsWith("travel_buffs"))
					{
						OdysseyStore.translatedTravelBuffs = JsonSerializer.Deserialize<Dictionary<string, SortedDictionary<int, string>>>(jsonString);
					}
                }
                LoadLevelTips(language);
                DumpJson();
            }
            catch (Exception e)
            {
                Logger.LogError($"{e.GetType()}: {e.Message}");
            }
            Logger.LogInfo("Strings loaded successfully!");
        }
    
        internal static void LoadLevelTips(Utils.LanguageEnum language)
        {
            string dumpDir = GetAssetDir(AssetType.Dumps);

            // dump iz tips
			var izLevelData = Resources.LoadAll<TextAsset>("izleveldata");
			Dictionary<string, string> izLevelDataDump = new Dictionary<string, string>();

            string izTranslatedPath = Path.Combine(GetAssetDir(AssetType.Strings, Utils.Language), "tips_iz.json");
            if (!File.Exists(izTranslatedPath))
            {
                File.WriteAllText(izTranslatedPath, JsonSerializer.Serialize(izLevelDataDump));
            }
            var izTranslatedTips = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(izTranslatedPath));

            foreach (var level in izLevelData) 
			{
				string data = level.text;
				LevelData levelData = JsonUtility.FromJson<LevelData>(data);
				if(levelData.tips != null)
				{
					izLevelDataDump.Add(level.name, levelData.tips);
					if(izTranslatedTips.ContainsKey(level.name) && !StringStore.translationString.ContainsKey(levelData.tips))
					{
                        StringStore.translationString.Add(levelData.tips, izTranslatedTips[level.name]);
                    }
                }
            }

            File.WriteAllText(Path.Combine(dumpDir, "tips_iz.json"), JsonSerializer.Serialize(izLevelDataDump, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));

            // dump fusion showcase tips
            var fusionShowcaseData = Resources.LoadAll<TextAsset>("leveldata/explore");
            Dictionary<string, string> fusionShowcaseDataDump = new Dictionary<string, string>();

            string fsTranslatedPath = Path.Combine(GetAssetDir(AssetType.Strings, Utils.Language), "tips_fs.json");
			if(!File.Exists(fsTranslatedPath))
			{
				File.WriteAllText(fsTranslatedPath, JsonSerializer.Serialize(fusionShowcaseDataDump));
			}
            var fsTranslatedTips = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(fsTranslatedPath));

            foreach (var level in fusionShowcaseData)
            {
                string data = level.text;
                LevelData levelData = JsonUtility.FromJson<LevelData>(data);
                if (levelData.tips != null)
                {
                    fusionShowcaseDataDump.Add(level.name, levelData.tips);
                    if (fsTranslatedTips.ContainsKey(level.name) && !StringStore.translationString.ContainsKey(levelData.tips))
                    {
                        StringStore.translationString.Add(levelData.tips, fsTranslatedTips[level.name]);
                    }
                }
            }

            File.WriteAllText(Path.Combine(dumpDir, "tips_fs.json"), JsonSerializer.Serialize(fusionShowcaseDataDump, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));
        }

        internal static void DumpJson()
        {
            string dumpDir = GetAssetDir(AssetType.Dumps);

			if (!Directory.Exists(dumpDir))
			{
				Directory.CreateDirectory(dumpDir);
			}
			string LawnStrings = Resources.Load<TextAsset>("LawnStrings").text;
			string ZombieStrings = Resources.Load<TextAsset>("ZombieStrings").text;
			string AbyssBuffData = Resources.Load<TextAsset>("AbyssBuffData").text;
			File.WriteAllText(Path.Combine(dumpDir, "LawnStrings.json"), LawnStrings);
			File.WriteAllText(Path.Combine(dumpDir, "ZombieStrings.json"), ZombieStrings);
			File.WriteAllText(Path.Combine(dumpDir, "AbyssBuffData.json"), AbyssBuffData);

            /*
                Dictionary<Achievement, AchievementObject> achievementsList = new Dictionary<Achievement, AchievementObject>();
                foreach (Il2CppSystem.Collections.Generic.KeyValuePair<Achievement, Il2CppSystem.Tuple<string, string>> entry in AchievementClip.achievementsText)
                {
                    achievementsList.Add(entry.Key, new AchievementObject(entry.Key, entry.Value.Item1, entry.Value.Item2));
                }
                File.WriteAllText(Path.Combine(dumpDir, "AchievementsText.json"), JsonSerializer.Serialize(achievementsList, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }));
            */
        }

        public static void DumpUntranslatedStrings(string text)
		{
			string dumpDir = GetAssetDir(AssetType.Dumps);
			string jsonFile = Path.Combine(dumpDir, "UntranslatedStrings.json");

			if (!Directory.Exists(dumpDir))
			{
				Directory.CreateDirectory(dumpDir);
			}

			if (!File.Exists(jsonFile))
			{
				File.WriteAllText(jsonFile, "{}"); // Initialize empty JSON object
			}

			string json = File.ReadAllText(jsonFile);
			var untranslatedStrings = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

			if (!untranslatedStrings.ContainsKey(text))
			{
				untranslatedStrings[text] = text;
				var options = new JsonSerializerOptions 
				{
					Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
				};
				File.WriteAllText(jsonFile, JsonSerializer.Serialize(untranslatedStrings, options));
			}
		}
		internal static void LoadLanguage()
		{
			try
			{
				// Load the language preference as a string and parse it into the enum
				string languageName = MelonPreferences.GetEntryValue<string>("PvZ_Fusion_Translator", "Language");
				if (Enum.TryParse(languageName, out Utils.LanguageEnum loadedLanguage))
				{
					Utils.Language = loadedLanguage;
				}
				else
				{
					Logger.LogWarning($"Invalid language '{languageName}' found in preferences. Falling back to English.");
					Utils.Language = Utils.LanguageEnum.English; // Default fallback
				}
				Logger.LogWarning($"Loaded language {languageName}");
			}
			catch (Exception e)
			{
				Logger.LogError("Error loading language setting.");
				Logger.LogError($"{e.GetType()} {e.Message}");
			}
			Logger.LogInfo($"Language has been loaded: {Utils.Language}");
		}

		internal static void SaveLanguage()
		{
			try
			{
				// Save the current language as a string
				MelonPreferences.SetEntryValue("PvZ_Fusion_Translator", "Language", Utils.Language.ToString());

				// Ensure changes are written to the config file
				MelonPreferences.Save();
            }
			catch (Exception e)
			{
				Logger.LogError("Error saving language setting.");
				Logger.LogError($"{e.GetType()} {e.Message}");
			}
			 Logger.LogInfo($"Language has been saved: {Utils.Language}");
        }
    }
}
