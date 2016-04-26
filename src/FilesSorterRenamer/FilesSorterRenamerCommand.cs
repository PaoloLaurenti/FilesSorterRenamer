using System;
using System.IO;
using System.Linq;

namespace FilesSorterRenamer
{
    internal class FilesSorterRenamerCommand
    {
        private const string OutputFolderNamePrefix = "Output";
        private readonly string _sourceFolderPath;
        private readonly string _destinationFolderPath;

        internal FilesSorterRenamerCommand(FilesSorterRenamerCommandRequest request)
        {
            _sourceFolderPath = request.SourceFolderPath;
            _destinationFolderPath = request.DestinationFolderPath;
            OnPercentageProgressChanged = x => { };
        }

        public Action<int> OnPercentageProgressChanged { get; set; }

        internal void Execute()
        {
            var allFilesCount = Directory.GetFiles(_sourceFolderPath, "*.*", SearchOption.AllDirectories).Length;
            var progressTracker = new ProgressTracker(allFilesCount, OnPercentageProgressChanged);
            var outputFolderPath = CreateOutputFolder();

            var firstLevelFolders = Directory.GetDirectories(_sourceFolderPath, "*.*", SearchOption.TopDirectoryOnly);

            Array.ForEach(firstLevelFolders, x =>
            {
                var destinationFolderName = Path.GetFileName(x);
                var destinationFolderPath = Path.Combine(outputFolderPath, destinationFolderName);
                Process(x, destinationFolderPath, progressTracker);
            });
        }

        private string CreateOutputFolder()
        {
            var counter = 1;
            var existingOutputFolders = Directory
                                        .GetDirectories(_destinationFolderPath, string.Format("{0}*", OutputFolderNamePrefix), SearchOption.TopDirectoryOnly)
                                        .Select(Path.GetFileName)
                                        .Select(x => x.Substring(OutputFolderNamePrefix.Length))
                                        .ToArray();

            if (existingOutputFolders.Any())
            {
                var existingMaxOutputFolderCounter = existingOutputFolders.Max(x => int.Parse(x));
                counter = existingMaxOutputFolderCounter + 1;
            }

            var outputFolderPath = Path.Combine(_destinationFolderPath, string.Format("{0}{1}", OutputFolderNamePrefix, counter.ToString().PadLeft(4, '0')));

            Directory.CreateDirectory(outputFolderPath);

            return outputFolderPath;
        }

        private static void Process(string sourceFolderPath, string destinationFolderPath, ProgressTracker progressTracker)
        {
            var index = 1;

            if (!Directory.Exists(destinationFolderPath))
                Directory.CreateDirectory(destinationFolderPath);

            Directory
                .GetFiles(sourceFolderPath, "*.*", SearchOption.TopDirectoryOnly)
                .OrderBy(x => new DirectoryInfo(x).CreationTimeUtc)
                .ToList()
                .ForEach(x =>
                {
                    var destFileName = string.Format("{0}{1}", index.ToString().PadLeft(5, '0'), Path.GetExtension(x));
                    File.Copy(x, Path.Combine(destinationFolderPath, destFileName));
                    index++;
                    progressTracker.TrackFileProcessed();
                });

            var subFolders = Directory.GetDirectories(sourceFolderPath, "*.*", SearchOption.TopDirectoryOnly);
            Array.ForEach(subFolders, x => Process(x, Path.Combine(destinationFolderPath, Path.GetFileName(x)), progressTracker));
        }
    }
}