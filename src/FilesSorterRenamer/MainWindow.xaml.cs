using System;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace FilesSorterRenamer
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSelectSourceFolder_Click(object sender, RoutedEventArgs e)
        {
            SelectFolderByDialog(sp => TxtSourceFolder.Text = sp);
        }

        private void BtnSelectDestinationFolder_Click(object sender, RoutedEventArgs e)
        {
            SelectFolderByDialog(sp => TxtDestinationFolder.Text = sp);
        }

        private static void SelectFolderByDialog(Action<string> onFolderSelected)
        {
            var dialog = new FolderBrowserDialog {ShowNewFolderButton = true};
            var result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
                onFolderSelected(dialog.SelectedPath);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CanvasFoldersSelection.IsEnabled = false;

            Thread.Sleep(7000);

            CanvasFoldersSelection.IsEnabled = true;
        }
    }
}
