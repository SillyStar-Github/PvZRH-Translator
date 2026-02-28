using MelonLoader;
using MelonLoader.Utils;
using PvZ_Fusion_Translator_Remake.AssetStore;
using UnityEngine;

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
            TextureStore.Init();
            AudioStore.Init();
            FontStore.Init();
        }

        private static DateTime dtStart;
		private static DateTime? dtStartToast;
		private static string toastText;
        object replaceTexturesCoroutine = null;

        public override void OnLateInitializeMelon()
        {
            dtStart = DateTime.Now;
            replaceTexturesCoroutine = MelonCoroutines.Start(TextureStore.ReplaceTexturesCoroutine());
            OdysseyStore.DumpTravelBuffs();
        }

        public override void OnDeinitializeMelon()
		{
			MelonCoroutines.Stop(replaceTexturesCoroutine);
			FileLoader.SaveLanguage();
		}

        public static void ShowToast(string message)
        {
            toastText = message;
            dtStartToast = new DateTime?(DateTime.Now);
        }

        public override void OnGUI()
		{
			if (dtStartToast != null)
			{
				GUI.Button(new Rect(10f, 10f, 200f, 20f), "\n" + toastText + "\n");
				TimeSpan? timeSpan = DateTime.Now - dtStartToast;
				TimeSpan t = new(0, 0, 0, 2);
				if (timeSpan > t)
				{
					dtStartToast = null;
				}
			}
		}

        public override void OnLateUpdate()
        {
            #if DEBUG
			ModFeatures.OnLateUpdate();
			#endif

			if (Input.GetKeyDown(KeyCode.Insert))
			{
				Utils.OpenSaveDirectory();
			}

			if (Input.GetKeyDown(KeyCode.Delete))
			{
				Utils.OpenTrello();
			}
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