using FilesValidator.VM;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FilesValidator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainModel Model { get; set; }

        public MainWindow()
        {
            Model = new MainModel(this.Dispatcher);
            InitializeComponent();
        }

        private void BrowseChecklistFileButtonClick(object sender, RoutedEventArgs e)
        {
            SelectChecklistFile();
            e.Handled = true;
        }

        private void ChecklistFileTextBox_MouseDoubleClick(
            object sender, MouseButtonEventArgs e)
        {
            SelectChecklistFile();
            e.Handled = true;
        }

        private void SelectChecklistFile()
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();
            var dialogResult = ofd.ShowDialog();
            if (!dialogResult.HasValue)
                return;
            if (!dialogResult.Value)
                return;
            Model.ChecklistFile = ofd.FileName;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DevExpress.Xpf.Core.ThemeManager.ApplicationThemeName = "Office2013";
            e.Handled = true;
        }
        private void BrowseCheckFolderButtonClick(object sender, RoutedEventArgs e)
        {
            SelectCheckFolder();
            e.Handled = true;
        }

        private void CheckFolderTextBox_MouseDoubleClick(
            object sender, MouseButtonEventArgs e)
        {
            SelectCheckFolder();
            e.Handled = true;
        }

        private void SelectCheckFolder()
        {
            var ofd = new FolderBrowserDialog();
            var dialogResult = ofd.ShowDialog();
            if (dialogResult != System.Windows.Forms.DialogResult.OK)
                return;
            Model.CheckFolder = ofd.SelectedPath;
        }
    }
}
