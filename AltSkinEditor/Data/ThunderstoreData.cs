using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltSkinEditor.Data
{
    [System.Serializable]
    public class ThunderstoreData
    {
        public string name;
        public string version_number;
        public string website_url = "";
        public string description;
        public string[] dependencies = new string[] { "Bobbie-AltSkins-1.5.0" };
    }
}
