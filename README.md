# AltSkinEditor
A custom editor for [AltSkins](https://github.com/legoandmars/AltSkins), a dynamic mod for loading skins in Nickelodeon All-Star Brawl.

## Installation

You can install AltSkinEditor by downloading the [latest release on the releases tab](https://github.com/legoandmars/AltSkins/releases/latest).

## Usage
Once installed, launching AltSkinEditor will ask you if you want to automatically extract Textures from your game. It's highly recommended you do this, as it will give you more accurate previews and textures for you to edit.

> **NOTE #1:** You need a valid copy of NASB on steam for this to work.

> **NOTE #2:** If your game is modded to use skins from outdated methods (replacing the .sharedasset files directly), it will extract those modded textures. It's highly recommended to verify your game's content on steam before extracting your textures if you think this might be the case.

Automatic extraction will take a few minutes, and spit out `Textures` and `Portraits` folders with textures and portraits for you to edit. If you ever need to regenerate these folders, delete them and re-run AltSkinEditor.


Once you've got your edited textures and portraits, putting together a skin is very easy.

Simply select the character you want to create a skin for, individually select each texture you want to modify, click `Open`, and select an image.

> **NOTE #3:** It's ***highly*** recommended that you at the very least skin your character's Albedo texture and, if it exists, mirror Albedo texture. Some characters - such as Spongebob or Ren and Stimpy - have multiple albedo textures that will need to be changed.


After selecting all of your textures, you can name your skin in the `Skin Name` field.

While it is optional, clicking the `Portrait` tab and adding a custom portrait is ***HEAVILY*** recommended, as it will make your skin stand out significantly more. **If you add a custom portrait, make sure it's 2048x2048!**

## Exporting
Now that you've got your skin complete, press the `Save` button to save the skin to a `.nasbskin` file. Make sure to put it in your `BepInEx/Skins` folder so it appears in game - the export window should start in this folder by default.

After this skin has exported, you'll have a dialog option to export it as a thunderstore package. If you'd like to upload your skin to [Thunderstore](https://nasb.thunderstore.io/)/[Slime Mod Manager](https://github.com/legoandmars/SlimeModManager) for other people to download, it's highly recommended that you do this.

Exporting as a thunderstore package expects you to fill in two additional values - description and version.

Description will be displayed in the mod manager and on thunderstore, so make sure it accurately describes your skin.

Version is a semver string used to keep track of your skin's version. If it's the first upload, leave it at `1.0.0` . If in the future you want to update your skin, you can always change this to a greater value (eg - `1.0.1`) and reupload to thunderstore to update your existing skin.

If you've exported a thunderstore package, you can easily upload it by going to the [Thunderstore Upload Page](https://nasb.thunderstore.io/) and uploading the file there. Make sure to set the category as `Skins`!