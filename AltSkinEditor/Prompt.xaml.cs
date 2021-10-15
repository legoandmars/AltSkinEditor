using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AltSkinEditor
{
    /// <summary>
    /// Interaction logic for Prompt.xaml
    /// </summary>
    public partial class Prompt : Window
    {
		public Prompt(string defaultDescription = "", string defaultVersion = "1.0.0")
		{
			InitializeComponent();
			txtSkinDesc.Text = defaultDescription;
			txtSkinVersion.Text = defaultVersion;
		}

		private void btnDialogOk_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

		private void Window_ContentRendered(object sender, EventArgs e)
		{
			//txtAnswer.Focus();
		}

		public string Description
		{
			get { return txtSkinDesc.Text; }
		}
		public string Version
		{
			get { return txtSkinVersion.Text; }
		}
	}
}
