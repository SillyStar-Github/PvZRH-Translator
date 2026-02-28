using Il2CppTMPro;

namespace PvZ_Fusion_Translator_Remake.AssetStore
{
    internal class FontStore
    {
        internal static Dictionary<string, TMP_FontAsset> fontAssetDict = [];
        internal static Dictionary<string, TMP_FontAsset> fontAssetDictSecondary = [];

        internal static void Init()
        {
            string fontsDir = Path.Combine(Core.Instance.assetDirectory, "[Custom Fonts]");

            foreach(string filePath in Directory.GetFiles(fontsDir))
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string fileExtension = Path.GetExtension(filePath);

                if(fileExtension == ".ttf" || fileExtension == ".otf")
                {
                    if(!fileName.EndsWith("_Almanac") && !fileName.EndsWith("_Fallback"))
                    {
                        TMP_FontAsset fontAsset = FontHandler.LoadTMPFont(filePath, true);
                        fontAsset.name = fileName;
                        fontAssetDict.Add(fileName, fontAsset);

                        Logger.LogInfo($"Font for language {fileName} loaded!");
                    }

                    if (fileName.EndsWith("_Almanac") || fileName.EndsWith("_Fallback"))
                    {
                        TMP_FontAsset fontAsset = FontHandler.LoadTMPFont(filePath, true);
                        fontAsset.name = fileName;
                        string fileNameLanguage = fileName.Replace("_Fallback", "").Replace("_Almanac", "");

                        if (fontAssetDictSecondary.ContainsKey(fileNameLanguage))
                        {
                            if (fileName.EndsWith("_Fallback"))
                            {
                                if (fileName.EndsWith("_Fallback"))
							    {
								    fontAssetDictSecondary.Add(fileNameLanguage + "_Almanac", fontAssetDictSecondary[fileNameLanguage]);
								    fontAssetDictSecondary.Remove(fileNameLanguage);
							    }
							    else
							    {
								    fontAssetDictSecondary.Add(fileNameLanguage + "_Fallback", fontAssetDictSecondary[fileNameLanguage]);
								    fontAssetDictSecondary.Remove(fileNameLanguage);
							    }
                                fontAssetDictSecondary.Remove(fileNameLanguage);
                            }
                            else
                            {
                                fontAssetDictSecondary.Add(fileNameLanguage, fontAsset);
                            }
                        }

                        Logger.LogInfo($"Fallback font for language {fileNameLanguage} loaded!");

                        AddFallback();
                    }
                }
            }
        }
  
        internal static void AddFallback()
        {
            foreach (var lang in fontAssetDict.Keys)
			{
				if (fontAssetDict[lang].fallbackFontAssetTable == null)
				{
					fontAssetDict[lang].fallbackFontAssetTable = new Il2CppSystem.Collections.Generic.List<TMP_FontAsset>();
				}

				if (fontAssetDictSecondary.ContainsKey(lang))
				{
					fontAssetDict[lang].fallbackFontAssetTable.Add(fontAssetDictSecondary[lang]);
					Logger.LogInfo("Fallback font for language '" + lang + "' added");
					continue;
				}
				if (fontAssetDictSecondary.ContainsKey(lang + "_Almanac"))
				{
					fontAssetDict[lang].fallbackFontAssetTable.Add(fontAssetDictSecondary[lang + "_Almanac"]);
					Logger.LogInfo("Fallback font for language '" + lang + "' added");
					continue;
				}
				if (fontAssetDictSecondary.ContainsKey(lang + "_Fallback"))
				{
					fontAssetDict[lang].fallbackFontAssetTable.Add(fontAssetDictSecondary[lang + "_Fallback"]);
					Logger.LogInfo("Fallback font for language '" + lang + "' added");
					continue;
				}
			}
        }

		public static void Reload()
		{
			_ = LoadTMPFont(Utils.Language);
		}

		public static TMP_FontAsset LoadTMPFont() => LoadTMPFont(Utils.Language);

		public static TMP_FontAsset LoadTMPFont(Utils.LanguageEnum language)
		{
            string languageString = language.ToString();
			if (fontAssetDict.TryGetValue(languageString, out TMP_FontAsset font))
			{
				TMP_FontAsset fontAsset = font;
				if (fontAsset.fallbackFontAssetTable != null)
				{
					// Log.LogInfo("Fallback font for language '" + language + "' loaded. The name of the FB Font is" + fontAsset.fallbackFontAssetTable[0].name);
				}
				return fontAsset;
			}
			return fontAssetDict.GetValueOrDefault("English");
		}

		public static TMP_FontAsset LoadTMPFontAlmanac() => LoadTMPFontAlmanac(Utils.Language);

        public static TMP_FontAsset LoadTMPFontAlmanac(Utils.LanguageEnum language)
		{
			string languageString = language.ToString();
			if (fontAssetDictSecondary.ContainsKey(languageString))
			{
				if (fontAssetDictSecondary.TryGetValue(languageString, out TMP_FontAsset almanacAsset))
				{
					return almanacAsset;
				}
			}
			if (fontAssetDictSecondary.ContainsKey(languageString + "_Almanac"))
			{
				if (fontAssetDictSecondary.TryGetValue(languageString + "_Almanac", out TMP_FontAsset almanacAsset))
				{
					return almanacAsset;
				}
			}

			return fontAssetDict.GetValueOrDefault("English");
		}
    }
}
