using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace FilesSorterRenamer
{
    public partial class MainWindow
    {
        private BackgroundWorker _worker;

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
            var dialog = new FolderBrowserDialog { ShowNewFolderButton = true };
            var result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
                onFolderSelected(dialog.SelectedPath);
        }

        private void Files_sorter__renamer_ContentRendered(object sender, EventArgs e)
        {
            _worker = new BackgroundWorker { WorkerReportsProgress = true };
            _worker.DoWork += worker_DoWork;
            _worker.ProgressChanged += worker_ProgressChanged;
            _worker.RunWorkerCompleted += WorkerOnRunWorkerCompleted;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (var i = 0; i < 101; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(100);
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PbStatus.Value = e.ProgressPercentage;            
        }

        private void WorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            EnableForm();
        }

        private void BtnExecute_Click(object sender, RoutedEventArgs e)
        {
            DisableForm();
            _worker.RunWorkerAsync();
        }

        private void EnableForm()
        {
            CanvasFoldersSelection.IsEnabled = true;
            BtnExecute.IsEnabled = true;
        }

        private void DisableForm()
        {
            CanvasFoldersSelection.IsEnabled = false;
            BtnExecute.IsEnabled = false;
        }
    }
}
