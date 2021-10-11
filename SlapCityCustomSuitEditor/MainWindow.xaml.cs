using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SlapCityCustomSuitEditor
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
            InitializeComponent();
        }
        private void cmbCharacter_Initialized(object sender, EventArgs e)
        {
            var characterSelect = sender as ComboBox;

            characterSelect.Items?.Clear();

            foreach (var character in Utilities.Constants.Skins.Keys)
            {
                characterSelect.Items.Add(character);
            }
            characterSelect.SelectedIndex = 0;
        }

        private void cmbCharacter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbskin == null) return;
            cmbskin.Items.Clear();
            foreach (var skin in Utilities.Constants.Skins[(string)cmbCharacter.SelectedItem])
            {
                cmbskin.Items.Add(skin);
            }
            cmbskin.SelectedIndex = 0;
        }

        private void cmbskin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtIndex != null) txtIndex.Text = "0";
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
                switch(tabImages.SelectedIndex)
                {
                    case 0:
                        imgTexture.Source = new BitmapImage(new Uri(op.FileName));
                        textureExtension = Path.GetExtension(op.FileName);
                        texture = File.ReadAllBytes(op.FileName);
                        break;
                    case 1:
                        imgPortrait.Source = new BitmapImage(new Uri(op.FileName));
                        portraitExtension = Path.GetExtension(op.FileName);
                        portrait = File.ReadAllBytes(op.FileName);
                        break;
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (!Validate()) return;

            var path = Utilities.GameUtils.GetSteamLocation();
            if (path != null)
                path = Path.Combine(path, "BepInEx\\CustomPalettes");
            else
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = path,
                FileName = txtSuitName.Text + ".suit",
                Filter = "Custom Suit|*.suit",
                Title = "Save Custom Suit"
            };

            if ((bool)saveFileDialog.ShowDialog() && saveFileDialog.FileName != "")
            {
                var texturePath = Utilities.Constants.TEXTURE_FILE_NAME + textureExtension;
                var portraitPath = Utilities.Constants.PORTRAIT_FILE_NAME + portraitExtension;

                var package = new Data.PackageJSON()
                {
                    Name = txtSuitName.Text,
                    characterID = cmbCharacter.SelectedItem as string ?? "Ittle Dew",
                    skinID = cmbskin.SelectedItem as string ?? "Default",
                    suitIndex = int.Parse(txtIndex.Text),
                    texturePath = texturePath,
                    portraitPath = portraitPath
                };

                var json = JsonConvert.SerializeObject(package);

                File.WriteAllText(Path.Combine(Path.GetTempPath(), Utilities.Constants.PACKAGE_FILE_NAME), json);
                if (texture != null) File.WriteAllBytes(Path.Combine(Path.GetTempPath(), texturePath), texture);
                if (portrait != null) File.WriteAllBytes(Path.Combine(Path.GetTempPath(), portraitPath), portrait);

                List<string> files = new List<string>
                {
                    Path.Combine(Path.GetTempPath(), Utilities.Constants.PACKAGE_FILE_NAME),
                    Path.Combine(Path.GetTempPath(), texturePath),
                    Path.Combine(Path.GetTempPath(), portraitPath)
                };

                CreateZipFile(saveFileDialog.FileName, files);
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
                MessageBox.Show("Please enter a name for your custom suit", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (texture == null)
            {
                MessageBox.Show("Please select an image texture.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            MessageBoxResult portraitMessageBox = MessageBox.Show("You did not select a portrait image. Continue anyway?", "Warning",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (portraitMessageBox == MessageBoxResult.No) return false;

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
