using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace FilesSorterRenamer
{
    public partial class MainWindow
    {
        private BackgroundWorker _worker;
        private readonly List<ComboBoxPairs> _sortingStrategies;

        public MainWindow()
        {
            _sortingStrategies = Enum
                                    .GetValues(typeof(SortingStrategy))
                                    .Cast<SortingStrategy>()
                                    .Select(x => new ComboBoxPairs(Properties.Resources.ResourceManager.GetString(x.ToString()), x))
                                    .ToList();
            InitializeComponent();
        }

        private void Photos_sorter__renamer_Loaded(object sender, RoutedEventArgs e)
        {
            CmbSortingStrategy.DisplayMemberPath = "Key";
            CmbSortingStrategy.SelectedValuePath = "Value";
            CmbSortingStrategy.ItemsSource = _sortingStrategies;
            CmbSortingStrategy.SelectedIndex = 0;
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
            var sourceFolderPath = "";
            var destFolderPath = "";
            var sortingStrategy = default(SortingStrategy);

            TxtSourceFolder.Dispatcher.Invoke(new Action(delegate {
                sourceFolderPath = TxtSourceFolder.Text;
            }));

            TxtDestinationFolder.Dispatcher.Invoke(new Action(delegate {
                destFolderPath = TxtDestinationFolder.Text;
            }));

            TxtDestinationFolder.Dispatcher.Invoke(new Action(delegate
            {
                sortingStrategy = (SortingStrategy) CmbSortingStrategy.SelectedValue;
            }));

            var filesSorterRenamerCommandRequest = new FilesSorterRenamerCommandRequest
            {
                SourceFolderPath = sourceFolderPath,
                DestinationFolderPath = destFolderPath,
                SortingStrategy = sortingStrategy
            };
            new FilesSorterRenamerCommand(filesSorterRenamerCommandRequest)
            {
                OnPercentageProgressChanged = x => ((BackgroundWorker) sender).ReportProgress(x)
            }.Execute();
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PbStatus.Value = e.ProgressPercentage;            
        }

        private void WorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            MessageBox.Show(Properties.Resources.OperationCompletedMessage, Properties.Resources.OperationCompletedDialogTitle, MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            TxtSourceFolder.Text = "";
            TxtDestinationFolder.Text = "";
            PbStatus.Value = 0;
        }
    }
}
