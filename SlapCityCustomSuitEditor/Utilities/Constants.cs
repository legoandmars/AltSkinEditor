using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlapCityCustomSuitEditor.Utilities
{
    class Constants
    {
        public static readonly Dictionary<string, string[]> Skins = new Dictionary<string, string[]>()
        {
            { "Ittle Dew", new string[] { "Default", "Delinquent", "Tippsie Ittle" } },
            { "Jenny Fox", new string[] { "Default", "Cat", "Gardening Fox" } },
            { "Masked Ruby", new string[] { "Default", "Magic Crystal", "Rubibi" } },
            { "Ultra Fishbunjin 3000", new string[] { "Default", "Manbunjin", "Mechabun" } },
            { "Princess Remedy", new string[] { "Default", "Rave", "Amelia" } },
            { "Business Casual Man", new string[] { "Default", "Criminal", "Besiege" } },
            { "Goddess of Explosions", new string[] { "Default", "War Queen", "Goddess of Abs" } },
            { "Asha", new string[] { "Default", "Ansaksie", "Skeleton" } },
            { "Frallan", new string[] { "Default", "Nuna" } }
        };

        public static readonly string PACKAGE_FILE_NAME = "package.json";
        public static readonly string TEXTURE_FILE_NAME = "texture"; 
        public static readonly string PORTRAIT_FILE_NAME = "portrait"; 
    }
}
