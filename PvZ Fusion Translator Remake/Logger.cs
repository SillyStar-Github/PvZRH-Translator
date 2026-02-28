using MelonLoader;
using MelonLoader.Logging;

namespace PvZ_Fusion_Translator_Remake
{
    public class Logger
    {
        public static void LogDebug(object txt) => MelonLogger.Msg(ColorARGB.Gray, txt);
        public static void LogInfo(object txt) => MelonLogger.Msg(ColorARGB.Blue, txt);
        public static void LogWarning(object txt) => MelonLogger.Warning(txt);
        public static void LogError(object txt) => MelonLogger.Error(txt);
    }
}
