using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltSkinEditor.Data
{
    public class CharacterData
    {
        public string Name;
        public string DisplayName;
        public string[] TextureNames;

        public CharacterData(string name, string displayName, string[] textureNames)
        {
            Name = name;
            DisplayName = displayName;
            TextureNames = textureNames;
        }
    }
}
