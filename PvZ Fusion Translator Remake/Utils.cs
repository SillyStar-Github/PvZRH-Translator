using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvZ_Fusion_Translator_Remake
{
    public class Utils
    {
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
    }
}
