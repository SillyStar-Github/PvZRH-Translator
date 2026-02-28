using HarmonyLib;
using UnityEngine;

namespace PvZ_Fusion_Translator_Remake.AssetStore
{
    internal class AudioStore
    {
        public static Dictionary<string, AudioClip> audioClips = [];

        public static string customAudioPath = Path.Combine(Core.Instance.assetDirectory, "[Custom Audios]");
        public static string[] audioExts = [".wav", ".mp3", ".ogg"];

        public static void Init() => LoadAudios();

        public static void LoadAudios()
        {
            if (!Directory.Exists(customAudioPath))
            {
                Directory.CreateDirectory(customAudioPath);
            }

            string[] audioFilePaths = Directory.GetFiles(customAudioPath, "*", SearchOption.AllDirectories);
            foreach (string filePath in audioFilePaths)
            {
                if (!audioExts.Contains(Path.GetExtension(filePath))) continue;

                AudioClip clip = AudioImportLib.API.LoadAudioClip(filePath);
                audioClips.Add(clip.name, clip);
                Logger.LogInfo($"Loaded custom audio clip: {clip.name}");
            }
        }

        [HarmonyPatch(typeof(AudioSource))]
        public class AudioStore_Patch
        {
            [HarmonyPatch(nameof(AudioSource.Play), argumentTypes: [])]
            [HarmonyPrefix]
            public static void Prefix(AudioSource __instance)
            {
                if (!Utils.customAudio) return;
                if (__instance.clip == null) return;

                if (audioClips.TryGetValue(__instance.clip.name, out AudioClip replaceClip))
                {
                    __instance.pitch = 1;
                    __instance.clip = replaceClip;
                }
            }
        }
    }
}
