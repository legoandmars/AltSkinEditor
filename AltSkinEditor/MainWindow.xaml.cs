using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using AltSkinEditor.Utilities;
using System.Windows.Media;
using AltSkinEditor.Data;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace AltSkinEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string textureExtension;
        byte[] texture;
        string portraitExtension;
        byte[] portrait;

        public MainWindow()
        {
            if (!Directory.Exists("./Portraits") || !Directory.Exists("./Textures"))
            {
                var result = MessageBox.Show("Would you like to automatically extract all skin textures from your game?", "Automatic Extraction", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result is MessageBoxResult.Yes)
                {
                    var assetHandler = new Assets.AssetHandler();
                    assetHandler.StartLoading();
                }
            }
            InitializeComponent();
        }

        List<EditorSkinData> editorSkinData = new List<EditorSkinData>();
        EditorSkinData selectedSkinData;
        private void cmbCharacter_Initialized(object sender, EventArgs e)
        {
            var characterSelect = sender as ComboBox;

            characterSelect.Items?.Clear();

            foreach (var character in Constants.Characters)
            {
                cmbCharacter.Items.Add($"{character.Name} ({character.DisplayName})");
            }
            selectedSkinData = new EditorSkinData();
            selectedSkinData.character = Constants.Characters[0];

            editorSkinData.Add(selectedSkinData);

            characterSelect.SelectedIndex = 0;
        }

        private void cmbCharacter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbskin == null) return;
            cmbskin.Items.Clear();

            foreach (var character in Constants.Characters)
            {
                if((string)cmbCharacter.SelectedItem == $"{character.Name} ({character.DisplayName})")
                {
                    foreach (var skin in character.TextureNames) cmbskin.Items.Add(skin);
                    if(editorSkinData.Any(a => a.character.Name == character.Name))
                    {
                        selectedSkinData = editorSkinData.Where(a => a.character.Name == character.Name).First();
                    }
                    else
                    {
                        Console.WriteLine("me makey new");
                        selectedSkinData = new EditorSkinData();
                        selectedSkinData.character = character;

                        editorSkinData.Add(selectedSkinData);

                        selectedSkinData.selectedSkin = 0;

                        string path = $"./Portraits/{character.Name.Split('_')[1]}.png";
                        imgPortrait.Source = LoadBitmapAtPath(path, "portrait");
                    }
                }
            }

            cmbskin.SelectedIndex = selectedSkinData.selectedSkin;
            txtSuitName.Text = selectedSkinData.skinName;
            imgPortrait.Source = selectedSkinData.images["portrait"];

        }

        private void cmbSkin_Initialized(object sender, EventArgs e)
        {
            cmbskin.Items?.Clear();

            foreach (var skin in Constants.Characters[0].TextureNames) cmbskin.Items.Add(skin);

            cmbskin.SelectedIndex = 0;
        }

        private void imgTexture_Initialized(object sender, EventArgs e)
        {
            imgTexture.Source = LoadImageForSelected();
        }

        private void imgPortrait_Initialized(object sender, EventArgs e)
        {
            string path = $"./Portraits/{Constants.Characters[0].Name.Split('_')[1]}.png";
            imgPortrait.Source = LoadBitmapAtPath(path, "portrait");
        }

        private void cmbskin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(selectedSkinData != null && cmbskin.SelectedIndex >= 0) selectedSkinData.selectedSkin = cmbskin.SelectedIndex;

            if(imgTexture != null)
            {
                imgTexture.Source = LoadImageForSelected();
            }
        }

        private BitmapImage LoadBitmapAtPath(string path, string key)
        {
            if(key == null) return null;
            BitmapImage bm;
            if (File.Exists(path)) bm = new BitmapImage(new Uri(Path.GetFullPath(path)));
            else bm = BitmapSource.Create(1, 1, 1, 1, PixelFormats.BlackWhite, null, new byte[] { 0 }, 1) as BitmapImage;

            selectedSkinData.defaultImages.Add(key, bm);
            selectedSkinData.images.Add(key, bm);

            return bm;
        }

            private BitmapImage LoadImageForSelected()
        {
            if (cmbskin.SelectedItem != null && selectedSkinData.images.ContainsKey((string)cmbskin.SelectedItem))
            {
                Console.WriteLine("WE EXIST");
                return selectedSkinData.images[(string)cmbskin.SelectedItem];
            }
            else
            {
                foreach (var character in Constants.Characters)
                {
                    if ((string)cmbCharacter.SelectedItem == $"{character.Name} ({character.DisplayName})")
                    {
                        // proper character, do stuff
                        // also make support generic stuff
                        //try loading (string)cmbskin.SelectedItem
                        Console.WriteLine(cmbskin.SelectedItem);
                        string path = $"./Textures/{character.Name}/{cmbskin.SelectedItem}.png";

                        return LoadBitmapAtPath(path, (string)cmbskin.SelectedItem);
                        // foreach (var skin in Constants.Characters[0].TextureNames) cmbskin.Items.Add(skin);

                    }
                }
            }
            return BitmapSource.Create(1, 1, 1, 1, PixelFormats.BlackWhite, null, new byte[] { 0 }, 1) as BitmapImage;
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                if(tabImages.SelectedIndex == 0)
                {
                    try
                    {
                        var bm = new BitmapImage(new Uri(op.FileName));
                        if (selectedSkinData.images.ContainsKey((string)cmbskin.SelectedItem))
                        {
                            // will have to be exapnded for custom textures eventually
                            selectedSkinData.images[(string)cmbskin.SelectedItem] = bm;
                        }

                        imgTexture.Source = bm;
                        LoadImageForSelected();
                    }
                    catch
                    {
                        MessageBox.Show("Could not load texture.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if(tabImages.SelectedIndex == 1)
                {
                    try
                    {
                        var bm = new BitmapImage(new Uri(op.FileName));
                        if (selectedSkinData.images.ContainsKey("portrait"))
                        {
                            // will have to be exapnded for custom textures eventually
                            selectedSkinData.images["portrait"] = bm;
                        }

                        imgPortrait.Source = bm;
                        //LoadImageForSelected();
                    }
                    catch
                    {
                        MessageBox.Show("Could not load portrait.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            switch (tabImages.SelectedIndex)
            {
                case 0:
                    imgTexture.Source = null;
                    texture = null;
                    break;
                case 1:
                    imgPortrait.Source = null;
                    portrait = null;
                    break;
            }
        }

        protected void txtSuitName_TextChanged(object sender, EventArgs e)
        {
            if(selectedSkinData != null) selectedSkinData.skinName = txtSuitName.Text;
        }

        public static void SaveImage(BitmapImage image, string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }

        private string saveToZip()
        {
            if (!Validate()) return "";

            var path = GameUtils.GetSteamLocation();
            if (path != null)
                path = Path.Combine(path, "BepInEx\\Skins");
            else
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = path,
                FileName = txtSuitName.Text + ".nasbskin",
                Filter = "NASB Alt Skin|*.nasbskin",
                Title = "Save Skin"
            };

            if ((bool)saveFileDialog.ShowDialog() && saveFileDialog.FileName != "")
            {
                if (File.Exists(saveFileDialog.FileName))
                {
                    var result = MessageBox.Show("A skin already exists there.\nAre you SURE you want to overwrite it?", "File already exists", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (!(result is MessageBoxResult.Yes)) return "";
                    else File.Delete(saveFileDialog.FileName);
                }
                //var texturePath = Utilities.Constants.TEXTURE_FILE_NAME + textureExtension;
                //var portraitPath = Utilities.Constants.PORTRAIT_FILE_NAME + portraitExtension;

                Dictionary<string, string> textureData = new Dictionary<string, string>();
                Dictionary<string, string> portraitData = new Dictionary<string, string>();
                List<string> imagesToZip = new List<string>();

                foreach (var imageData in selectedSkinData.images)
                {
                    foreach (var defaultImageData in selectedSkinData.defaultImages)
                    {
                        if (imageData.Key == defaultImageData.Key && imageData.Value != defaultImageData.Value)
                        {
                            var imagePath = Path.Combine(Path.GetTempPath(), imageData.Key + ".png");
                            SaveImage(imageData.Value, imagePath);
                            imagesToZip.Add(imagePath);
                            if (imageData.Key == "portrait")
                            {
                                portraitData.Add(imageData.Key, Path.GetFileName(imagePath));

                                // what the fuck lol
                                var medPath = Path.Combine(Path.GetTempPath(), "portrait_medium.png");
                                ResizeImage(System.Drawing.Image.FromFile(imagePath), 1080, 1080).Save(medPath);

                                imagesToZip.Add(medPath);
                                portraitData.Add("portrait_medium", "portrait_medium.png");

                                var smallPath = Path.Combine(Path.GetTempPath(), "portrait_small.png");
                                ResizeImage(System.Drawing.Image.FromFile(imagePath), 720, 720).Save(smallPath);

                                imagesToZip.Add(smallPath);
                                portraitData.Add("portrait_small", "portrait_small.png");
                            }
                            else textureData.Add(imageData.Key, Path.GetFileName(imagePath));
                        }
                    }
                }

                var jsonInfo = new SkinFormat()
                {
                    skinName = selectedSkinData.skinName,
                    characterName = selectedSkinData.character.Name,
                    forceReplaceAll = false,
                    textureReplacements = textureData,
                    portraitReplacements = portraitData
                };

                var json = JsonConvert.SerializeObject(jsonInfo, Formatting.Indented);

                File.WriteAllText(Path.Combine(Path.GetTempPath(), Utilities.Constants.PACKAGE_FILE_NAME), json);

                List<string> files = new List<string>
                {
                    Path.Combine(Path.GetTempPath(), Utilities.Constants.PACKAGE_FILE_NAME),
                };

                files.AddRange(imagesToZip);

                CreateZipFile(saveFileDialog.FileName, files);

                return saveFileDialog.FileName;
                // foreach (var file in files) File.Delete(file);
            }
            return "";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var savedZip = saveToZip();
            if (!String.IsNullOrEmpty(savedZip))
            {
                MessageBoxResult portraitMessageBox = MessageBox.Show("Would you like to export a thunderstore package for your skin?\n\nA thunderstore package would allow you to easily upload to Thunderstore/Slime Mod Manager.", "Thunderstore Package",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (portraitMessageBox == MessageBoxResult.No) return;

                var path = GameUtils.GetSteamLocation();
                if (path != null)
                    path = Path.Combine(path, "BepInEx\\Skins");
                else
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                var saveFileDialog = new SaveFileDialog
                {
                    InitialDirectory = path,
                    FileName = selectedSkinData.skinName + "_thunderstore_package.zip",
                    Filter = "ZIP|*.zip",
                    Title = "Save Thunderstore Package"
                };
                if ((bool)saveFileDialog.ShowDialog() && saveFileDialog.FileName != "")
                {
                    Prompt prompt = new Prompt();

                    if (prompt.ShowDialog() == true)
                    {
                        Console.WriteLine(prompt.Description);
                        Console.WriteLine(prompt.Version);
                        var portrait = selectedSkinData.images.Where(a => a.Key == "portrait").First();

                        var imagePath = Path.Combine(Path.GetTempPath(), "portrait.png");
                        var iconPath = Path.Combine(Path.GetTempPath(), "icon.png");
                        ResizeImage(System.Drawing.Image.FromFile(imagePath), 256, 256).Save(iconPath);

                        var thunderstoreInfo = new ThunderstoreData()
                        {
                            name = selectedSkinData.skinName.Replace(' ', '_'),
                            version_number = prompt.Version,
                            description = prompt.Description
                        };

                        var json = JsonConvert.SerializeObject(thunderstoreInfo, Formatting.Indented);

                        File.WriteAllText(Path.Combine(Path.GetTempPath(), "manifest.json"), json);

                        File.WriteAllText(Path.Combine(Path.GetTempPath(), "README.md"), $"# {selectedSkinData.skinName}\n{prompt.Description}");

                        List<string> files = new List<string>
                    {
                        Path.Combine(Path.GetTempPath(), "manifest.json"),
                        Path.Combine(Path.GetTempPath(), "README.md"),
                        Path.Combine(Path.GetTempPath(), "icon.png"),
                        Path.GetFullPath(savedZip)
                    };


                        CreateZipFile(saveFileDialog.FileName, files);
                    }
                }
            }
        }

        public static void CreateZipFile(string fileName, IEnumerable<string> files)
        {
            // Create and open a new ZIP file
            var zip = ZipFile.Open(fileName, ZipArchiveMode.Create);
            foreach (var file in files)
            {
                // Add the entry for each file
                zip.CreateEntryFromFile(file, Path.GetFileName(file), System.IO.Compression.CompressionLevel.Optimal);
            }
            // Dispose of the object when we are done
            zip.Dispose();
        }

        private bool Validate()
        {
            if (txtSuitName.Text == "")
            {
                MessageBox.Show("Please enter a name for your skin.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            bool texturesChanged = false;
            bool portraitChanged = false;

            foreach (var imageData in selectedSkinData.images)
            {
                foreach(var defaultImageData in selectedSkinData.defaultImages)
                {
                    var individualTextureChanged = false;
                    if(imageData.Key == defaultImageData.Key && imageData.Value != defaultImageData.Value)
                    {
                        if (imageData.Key == "portrait") portraitChanged = true;
                        else individualTextureChanged = true;
                    }
                    if (individualTextureChanged) texturesChanged = true;
                }
            }

            if (!texturesChanged)
            {
                MessageBox.Show("Please change at least one texture.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!portraitChanged)
            {
                MessageBoxResult portraitMessageBox = MessageBox.Show("You did not select a portrait image. Continue anyway?", "Warning",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (portraitMessageBox == MessageBoxResult.No) return false;

            }
            return true;
        }

        private void imgTexture_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            for (int i = 0; i < files.Length; i++)
            {
                
            }

        }
    }
}
