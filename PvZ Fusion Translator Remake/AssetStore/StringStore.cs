using Il2Cpp;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PvZ_Fusion_Translator_Remake.AssetStore
{
    public static class StringStore
    {
        public static Dictionary<string, string> translationString = [];
        public static Dictionary<string, string> translationStringRegex = [];

        internal static void Init()
        {
            FileLoader.LoadStrings();
        }

        internal static void Reload()
        {
            translationString.Clear();
            translationStringRegex.Clear();
            FileLoader.LoadStrings();
        }
    }

    public static class OdysseyStore
    {
        public static Dictionary<string, SortedDictionary<int, string>> dumpedTravelBuffs = new()
        {
            { "advancedBuffs", new SortedDictionary<int, string>() },
            { "ultimateBuffs", new SortedDictionary<int, string>() },
            { "debuffs", new SortedDictionary<int, string>() },
            { "unlocks", new SortedDictionary<int, string>() },
            { "investmentBuffs", new SortedDictionary<int, string>() }
        };

        public static Dictionary<string, SortedDictionary<int, string>> translatedTravelBuffs = new()
        {
            { "advancedBuffs", new SortedDictionary<int, string>() },
            { "ultimateBuffs", new SortedDictionary<int, string>() },
            { "debuffs", new SortedDictionary<int, string>() },
            { "unlocks", new SortedDictionary<int, string>() },
            { "investmentBuffs", new SortedDictionary<int, string>() }
        };

        public static Dictionary<BuffType, string> buffLinks = new()
        {
            { BuffType.AdvancedBuff, "advancedBuffs" },
            { BuffType.UltimateBuff, "ultimateBuffs" },
            { BuffType.Debuff, "debuffs" },
            { BuffType.UnlockPlant, "unlocks" },
            { BuffType.InvestmentBuff, "investmentBuffs" }
        };

        public static void DumpTravelBuffs()
        {
            TravelMgr.Instance.GetPlantBuffUnlockCount(PlantType.DoomGatling);

            foreach (var pair in TravelDictionary.advancedBuffsText)
            {
                dumpedTravelBuffs["advancedBuffs"].Add((int)pair.Key, pair.Value);
            }

            foreach (var pair in TravelDictionary.ultimateBuffsText)
            {
                dumpedTravelBuffs["ultimateBuffs"].Add((int)pair.Key, pair.Value);
            }

            foreach (var pair in TravelDictionary.debuffData)
            {
                dumpedTravelBuffs["debuffs"].Add((int)pair.Key, pair.Value.Item1);
            }

            foreach (var pair in TravelDictionary.unlocksText)
            {
                dumpedTravelBuffs["unlocks"].Add((int)pair.Key, pair.Value);
            }

            foreach (var pair in TravelMgr.InvestBuffsData)
            {
                dumpedTravelBuffs["investmentBuffs"].Add((int)pair.Key, pair.Value.GetDescription());
            }

            File.WriteAllText(Path.Combine(FileLoader.GetAssetDir(FileLoader.AssetType.Dumps), "travel_buffs.json"), JsonSerializer.Serialize(dumpedTravelBuffs, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));

            string translatedTravelBuffsPath = Path.Combine(FileLoader.GetAssetDir(FileLoader.AssetType.Strings, Utils.Language), "travel_buffs.json");

            if (!File.Exists(translatedTravelBuffsPath))
            {
                File.WriteAllText(translatedTravelBuffsPath, JsonSerializer.Serialize(dumpedTravelBuffs, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }));
                translatedTravelBuffs = dumpedTravelBuffs;
            }
        }

        public static string MatchTravelBuff(string originalText)
        {
            string res = "";

            foreach (var i in dumpedTravelBuffs)
            {
                foreach (var j in dumpedTravelBuffs[i.Key])
                {
                    if (j.Value == originalText || j.Value == RemoveBuffName(originalText))
                    {
                        res = translatedTravelBuffs[i.Key][j.Key];
                        break;
                    }
                }

                if (res != "") break;
            }

            return res;
        }

        public static string RemoveBuffName(string buffText)
        {
            string res = buffText;
            int firstColon = res.IndexOf("：");
            if(firstColon > 0)
            {
                res = res.Substring(firstColon + 1);
            }
            return res;
        }
    }
}
