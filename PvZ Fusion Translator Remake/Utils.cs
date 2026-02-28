using MelonLoader;
using PvZ_Fusion_Translator_Remake.AssetStore;
using System.Diagnostics;
using UnityEngine;

namespace PvZ_Fusion_Translator_Remake
{
    public class Utils
    {
		// Language Settings

		public static LanguageEnum OldLanguage;

		public static LanguageEnum Language;

        public enum LanguageEnum
        {
            // first column
			English,
			French,
			Italian,
			German,
			Spanish,
			Portuguese,

			// second column
			Javanese, //Filipino,
			Vietnamese,
			Indonesian,
			Russian, //NOTE: legacy language
			Japanese,
			Korean,

			// third column
			 Ukrainian,
			// Slovak,
			//Polish,
			Turkish,
            //Arabic,
            Romanian,

            LANG_END
        }
		
		internal static void ChangeLanguage(string language)
		{
			OldLanguage = Language;

			if (Enum.TryParse(language, out LanguageEnum lang))
			{
				Language = lang;
			}
			else
			{
				// Handle invalid language string
				Logger.LogError($"Invalid language string: {language}");
			}
			
			//WarningStore.isWarningMessageLoaded = false;
			FontStore.Reload();
			StringStore.Reload();
			TextureStore.Reload();
			FileLoader.SaveLanguage();
			//RegisterPlantIndices();
			//Zombie_Patch.LoadHPStrings();
        }

		// Custom Asset Settings

		public static bool customTextures => MelonPreferences.GetEntry<bool>("PvZ_Fusion_Translator", "Custom Textures").Value;
		public static bool customAudio => MelonPreferences.GetEntry<bool>("PvZ_Fusion_Translator", "Custom Audio").Value;

		// Loading Functions

		internal static byte[] LoadImage(string path)
		{
			if (!File.Exists(path))
			{
				throw new FileNotFoundException($"The image file at path '{path}' does not exist.");
			}

			byte[] array = File.ReadAllBytes(path);
			return array;
		}

		internal static bool TryReplaceTexture2D(Texture2D ogTexture)
		{
			if (ogTexture != null)
			{
				if (TextureStore.textureDict.TryGetValue(ogTexture.name, out byte[] textureData))
				{
					try
					{
						ImageConversion.LoadImage(ogTexture, textureData);
						Logger.LogDebug($"Loaded texture {ogTexture.name}!");
						ogTexture.name = "replaced_" + ogTexture.name;
						return true;
					}
					catch (Exception e)
					{
						Logger.LogError($"Failed to replace texture: {ogTexture.name}");
						Logger.LogError($"{e.GetType()}: {e.Message}");
					}
				}
			}
			return false;
		}
	
		// Text Manipulation Functions

		// Almanac Data Functions

		// Misc. Utilities

		public static void OpenSaveDirectory()
		{
			string saveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "LocalLow", "LanPiaoPiao", "PlantsVsZombiesRH");
			Process.Start("explorer.exe", saveDirectory);
		}

		public static void OpenTrello()
		{
			string website = "https://trello.com/b/DcdT1kUp";
			Process.Start(new ProcessStartInfo(website) { UseShellExecute = true });
		}
	}
}
