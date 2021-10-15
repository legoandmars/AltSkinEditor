using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetsTools.NET.Extra;
using AssetsTools.NET;
using System.Drawing;
//using System.Drawing.Common;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using AltSkinEditor.Data;
using AltSkinEditor.Utilities;
using System.Reflection;
using System.Windows;

namespace AltSkinEditor.Assets
{
    public class AssetHandler
    {

        public struct TextureSearchData {
            public string PathToExport;
            public string TextureName;
            public TextureSearchData(string path, string texture)
            {
                PathToExport = path;
                TextureName = texture;
            }
        }

        public void SaveFile(TextureSearchData textureToSearch, byte[] texDat, int width, int height)
        {
            if (texDat != null && texDat.Length > 0)
            {
                var canvas = new Bitmap(width, height, width * 4, PixelFormat.Format32bppArgb,
                    Marshal.UnsafeAddrOfPinnedArrayElement(texDat, 0));
                canvas.RotateFlip(RotateFlipType.RotateNoneFlipY);
                if (!Directory.Exists(Path.GetDirectoryName(textureToSearch.PathToExport))) Directory.CreateDirectory(Path.GetDirectoryName(textureToSearch.PathToExport));
                canvas.Save(textureToSearch.PathToExport);
            }
        }

        public void SearchAssetFile(AssetsManager am, string path, ref List<TextureSearchData> searchData)
        {
            if (File.Exists(path))
            {
                Console.WriteLine(path);

                var inst = am.LoadAssetsFile(path, true);

                am.LoadClassPackage("classdata.tpk");
                am.LoadClassDatabaseFromPackage(inst.file.typeTree.unityVersion);

                var allTextures = inst.table.GetAssetsOfType((int)AssetClassID.Texture2D);

                for (int i = 0; i < allTextures.Count(); i++)
                {
                    var inf = allTextures[i];
                    var baseField = am.GetTypeInstance(inst, inf).GetBaseField();
                    var name = baseField.Get("m_Name").GetValue().AsString();
                    foreach(TextureSearchData textureToSearch in searchData)
                    {
                        if (name == textureToSearch.TextureName)
                        {
                            // export
                            var tf = TextureFile.ReadTextureFile(baseField);
                            var texDat = tf.GetTextureData(inst);

                            SaveFile(textureToSearch, texDat, tf.m_Width, tf.m_Height);
                            //searchData.Remove(textureToSearch);
                            //probably re-enable that and hope it dont break shit lmao
                            /*
                            Console.WriteLine("FOUND!");

                            var tf = TextureFile.ReadTextureFile(baseField);
                            var texDat = tf.GetTextureData(inst);
                            if (texDat != null && texDat.Length > 0)
                            {
                                var canvas = new Bitmap(tf.m_Width, tf.m_Height, tf.m_Width * 4, PixelFormat.Format32bppArgb,
                                    Marshal.UnsafeAddrOfPinnedArrayElement(texDat, 0));
                                canvas.RotateFlip(RotateFlipType.RotateNoneFlipY);

                                canvas.Save($"{textureToSearch}.png");
                            }*/
                        }
                    }
                }

                am.UnloadAllAssetsFiles();
            }
        }

        public void StartLoading()
        {
            var am = new AssetsManager();
            List<TextureSearchData> searchData = new List<TextureSearchData>();

            //reflection moment
            //public System.Reflection.FieldInfo[] GetFields();

            FieldInfo[] fields = typeof(CharacterTextureData).GetFields();

            foreach(var field in fields)
            {
                string[] value = field.GetValue(null) as string[];
                var characterName = field.Name.Substring(0, field.Name.IndexOf("_textures"));
                var rawCharacterName = characterName.Split('_')[1];

                searchData.Add(new TextureSearchData($"./Portraits/{rawCharacterName}.png", rawCharacterName+"_large"));

                foreach (string characterTexture in value)
                {
                    if (characterTexture == "CharactersCustomesHatsMtlSG_Albedo") continue; // no reason for hats
                    searchData.Add(new TextureSearchData($"./Textures/{characterName}/{characterTexture}.png", characterTexture));
                }
            }

            /*foreach (string characterTexture in CharacterTextureData.char_apple_textures)
            {
                if (characterTexture == "CharactersCustomesHatsMtlSG_Albedo") continue; // no reason for hats
                searchData.Add(new TextureSearchData($"./Textures/char_apple/{characterTexture}.png", characterTexture));
            }*/

            var steamLoc = GameUtils.GetSteamLocation();

            var resourcesSearchLocation = Path.Combine(steamLoc, "Nickelodeon All-Star Brawl_Data", "resources.assets");
            SearchAssetFile(am, resourcesSearchLocation, ref searchData);

            for (int i = 0; i < 68; i++)
            {
                //var result = MessageBox.Show("Would you like to automatically extract all skin textures from your game?", "Automatic Extraction", MessageBoxButton, MessageBoxImage.Question);
                var searchLocation = Path.Combine(steamLoc, "Nickelodeon All-Star Brawl_Data", $"sharedassets{i}.assets");
                SearchAssetFile(am, searchLocation, ref searchData);
                //SearchAssetFile(am, $"C:\\Program Files (x86)\\Steam\\steamapps\\common\\Nickelodeon All-Star Brawl\\Nickelodeon All-Star Brawl_Data\\sharedassets{i}.assets", "Plasma_Albedo");
            }
            /*Console.WriteLine("gaming");
            var inst = am.LoadAssetsFile("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Nickelodeon All-Star Brawl\\Nickelodeon All-Star Brawl_Data\\sharedassets0.assets", true);

            am.LoadClassPackage("classdata.tpk");
            am.LoadClassDatabaseFromPackage(inst.file.typeTree.unityVersion);
            var allTextures = inst.table.GetAssetsOfType((int)AssetClassID.Texture2D);
            //Console.WriteLine((inst.table.GetAssetsOfType((int)AssetClassID.Texture2D).Count()));
            var textureToSearch = "Plasma_Albedo";
            Console.WriteLine("Searching for texture by name...");
            Console.WriteLine(textureToSearch);
            for(int i = 0; i < allTextures.Count(); i++)
            {
                var inf = allTextures[i];

                var baseField = am.GetTypeInstance(inst, inf).GetBaseField();

                var name = baseField.Get("m_Name").GetValue().AsString();
                Console.WriteLine(name);
                if(name.ToLower().Trim() == textureToSearch.ToLower().Trim())
                {
                    Console.WriteLine("FOUND!");

                    var tf = TextureFile.ReadTextureFile(baseField);
                    var texDat = tf.GetTextureData(inst);
                    if (texDat != null && texDat.Length > 0)
                    {
                        var canvas = new Bitmap(tf.m_Width, tf.m_Height, tf.m_Width * 4, PixelFormat.Format32bppArgb,
                            Marshal.UnsafeAddrOfPinnedArrayElement(texDat, 0));
                        canvas.RotateFlip(RotateFlipType.RotateNoneFlipY);

                        canvas.Save($"{textureToSearch}.png");
                    }
                }
            }*/
            /*foreach (var inf in inst.table.GetAssetsOfType((int)AssetClassID.Texture2D))
            {
                var baseField = am.GetTypeInstance(inst, inf).GetBaseField();

                var name = baseField.Get("m_Name").GetValue().AsString();
                //Console.WriteLine(name);
            }*/

            //am.UnloadAllAssetsFiles();

        }
    }
}
