using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltSkinEditor.Data
{
    [System.Serializable]
    public class SkinFormat
    {
        public string characterName;
        public string skinName;
        public bool forceReplaceAll = false;
        public Dictionary<string, string> textureReplacements;
        public Dictionary<string, string> portraitReplacements;
    }
}
