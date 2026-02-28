using MelonLoader;
using MelonLoader.Utils;
using PvZ_Fusion_Translator_Remake.AssetStore;
using Unity.VisualScripting.FullSerializer;

[assembly: MelonInfo(typeof(PvZ_Fusion_Translator_Remake.Core), "PvZ Fusion Translator", "1.0.0", "cassidy", null)]
[assembly: MelonGame("LanPiaoPiao", "PlantsVsZombiesRH")]

namespace PvZ_Fusion_Translator_Remake
{
    public class Core : MelonMod
    {
        public static Core Instance { get; private set; }

        public string assetDirectory => Path.Combine(MelonEnvironment.ModsDirectory, "PvZ_Fusion_Translator");

        public override void OnInitializeMelon()
        {
            Instance = this;
            Config();
            StringStore.Init();
        }


        public MelonPreferences_Entry<bool> configCustomTextures;
		public MelonPreferences_Entry<bool> configCustomAudio;
		public MelonPreferences_Entry<string> configLanguage;

        private void Config()
		{
			var category = MelonPreferences.CreateCategory("PvZ_Fusion_Translator", "Fusion Translator Settings");

			configCustomTextures = category.CreateEntry("Custom Textures", true);
			configCustomAudio = category.CreateEntry("Custom Audio", true);
			configLanguage = category.CreateEntry("Language", "English");

			MelonPreferences.Save();
		}
    }
}