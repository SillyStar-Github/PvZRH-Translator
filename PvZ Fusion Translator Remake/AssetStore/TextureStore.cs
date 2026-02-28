using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PvZ_Fusion_Translator_Remake.AssetStore
{
    public static class TextureStore
    {
        internal static Dictionary<string, string> textureDict = [];
		internal static Dictionary<string, string> spriteDict = [];

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
    }
}
