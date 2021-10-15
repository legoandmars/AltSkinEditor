using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AltSkinEditor.Data;

namespace AltSkinEditor.Utilities
{
    class Constants
    {
        public static readonly List<CharacterData> Characters = new List<CharacterData>() {
            new CharacterData("char_apple", "Spongebob", CharacterTextureData.char_apple_textures),
            new CharacterData("char_star", "Patrick", CharacterTextureData.char_star_textures),
            new CharacterData("char_diver", "Sandy", CharacterTextureData.char_diver_textures),
            new CharacterData("char_kite", "Aang", CharacterTextureData.char_kite_textures),
            new CharacterData("char_athlete", "Korra", CharacterTextureData.char_athlete_textures),
            new CharacterData("char_clay", "Toph", CharacterTextureData.char_clay_textures),
            new CharacterData("char_rascal", "Lincoln Loud", CharacterTextureData.char_rascal_textures),
            new CharacterData("char_goth", "Lucy Loud", CharacterTextureData.char_goth_textures),
            new CharacterData("char_moon", "Leonardo", CharacterTextureData.char_moon_textures),
            new CharacterData("char_pizza", "Michelangelo", CharacterTextureData.char_pizza_textures),
            new CharacterData("char_reporter", "April O'Neil", CharacterTextureData.char_reporter_textures),
            new CharacterData("char_duo", "Ren & Stimpy", CharacterTextureData.char_duo_textures),
            new CharacterData("char_hero", "Powdered ToastMan", CharacterTextureData.char_hero_textures),
            new CharacterData("char_alien", "Zim", CharacterTextureData.char_alien_textures),
            new CharacterData("char_chimera", "CatDog", CharacterTextureData.char_chimera_textures),
            new CharacterData("char_mascot", "Reptar", CharacterTextureData.char_mascot_textures),
            new CharacterData("char_narrator", "Nigel Thornberry", CharacterTextureData.char_narrator_textures),
            new CharacterData("char_rival", "Helga", CharacterTextureData.char_rival_textures),
            new CharacterData("char_plasma", "Danny Phantom", CharacterTextureData.char_plasma_textures),
            new CharacterData("char_snake", "char_snake", CharacterTextureData.char_snake_textures),
        };

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
