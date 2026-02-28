using System.Collections;
using UnityEngine;

namespace PvZ_Fusion_Translator_Remake.AssetStore
{
    public static class TextureStore
    {
        internal static Dictionary<string, byte[]> textureDict = [];
		internal static List<string> spriteList = [];

        internal static void Init() => FileLoader.LoadTextures();

		internal static void Reload()
		{
			textureDict.Clear();
			RestoreTextures();
			FileLoader.LoadTextures();
		}

		public static void RestoreTextures()
		{
			Texture2D[] textures = Resources.FindObjectsOfTypeAll<Texture2D>();
			foreach (Texture2D texture in textures)
			{
				if (texture != null)
				{
					texture.name = texture.name.Replace("replaced_", "");
				}
			}
		}

		public static IEnumerator ReplaceTexturesCoroutine()
		{
			while (true)
			{
				ReplaceTextures();
				yield return new WaitForSeconds(0.5f); // Texture replacement interval
			}
		}

		public static void ReplaceTextures()
		{
			Texture2D[] textures = Resources.FindObjectsOfTypeAll<Texture2D>();
			foreach (Texture2D texture in textures)
			{
				if (texture.name.StartsWith("replaced_"))
					continue;

				Utils.TryReplaceTexture2D(texture);
			}
		}
    }
}
