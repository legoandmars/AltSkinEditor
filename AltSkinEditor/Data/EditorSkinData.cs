using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace AltSkinEditor.Data
{
    public class EditorSkinData
    {
        public CharacterData character;
        public string skinName = "";
        public int selectedSkin = 0;
        public Dictionary<string, BitmapImage> images = new Dictionary<string, BitmapImage>();
        public Dictionary<string, BitmapImage> defaultImages = new Dictionary<string, BitmapImage>();
    }
}
