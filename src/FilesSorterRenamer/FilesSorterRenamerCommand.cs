using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FilesSorterRenamer.Sorting;

namespace FilesSorterRenamer
{
    internal class FilesSorterRenamerCommand
    {
        private readonly string _sourceFolderPath;
        private readonly string _destinationFolderPath;
        private readonly SortingStrategy _sortingStrategy;
        private static readonly string[] PhotoExtensions = {"jpg", "jpeg"};

        internal FilesSorterRenamerCommand(FilesSorterRenamerCommandRequest request)
        {
            _sourceFolderPath = request.SourceFolderPath;
            _destinationFolderPath = request.DestinationFolderPath;
            _sortingStrategy = request.SortingStrategy;
            OnPercentageProgressChanged = x => { };
        }

        internal Action<int> OnPercentageProgressChanged { get; set; }

        internal void Execute()
        {
            var allFilesCount = Directory.GetFiles(_sourceFolderPath, "*.*", SearchOption.AllDirectories).Length;
            var progressTracker = new ProgressTracker(allFilesCount, OnPercentageProgressChanged);
            var destinationFolderName = Path.GetFileName(_sourceFolderPath);
            var outputFolderPath = CreateOutputFolder(destinationFolderName);
            var sortingStrategy = SortingStrategyFactory.GetStrategy(_sortingStrategy);

            Process(_sourceFolderPath, outputFolderPath, sortingStrategy, progressTracker);
        }

        private string CreateOutputFolder(string destinationFolderName)
        {
            var existingDestinationFolders = Directory.GetDirectories(_destinationFolderPath, string.Format("{0}*", destinationFolderName), SearchOption.TopDirectoryOnly).Select(Path.GetFileName).Where(x => IsAnAlreadyExistingDestinationFolder(x, destinationFolderName)).ToArray();

            var outputFolderPath = Path.Combine(_destinationFolderPath, destinationFolderName);

            if (existingDestinationFolders.Any())
            {
                var counter = existingDestinationFolders.Length;
                outputFolderPath = string.Format("{0}_{1}", outputFolderPath, counter.ToString().PadLeft(4, '0'));
            }

            Directory.CreateDirectory(outputFolderPath);

            return outputFolderPath;
        }

        private static bool IsAnAlreadyExistingDestinationFolder(string folderName, string destinationFolderName)
        {
            return Regex.IsMatch(folderName, string.Format("^{0}(_[0-9]+)?$", destinationFolderName));
        }

        private static void Process(string sourceFolderPath, string destinationFolderPath, ISortingStrategy sortingStrategy, ProgressTracker progressTracker)
        {
            var index = 1;

            if (!Directory.Exists(destinationFolderPath))
                Directory.CreateDirectory(destinationFolderPath);

            Directory
                .GetFiles(sourceFolderPath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(x => PhotoExtensions.Any(pe =>
                {
                    return string.Equals(pe, Path.GetExtension(x).Substring(1), StringComparison.InvariantCultureIgnoreCase);
                }))
                .OrderBy(sortingStrategy.GetDate)
                .ToList()
                .ForEach(x =>
                            {
                                var destFileName = string.Format("{0}{1}", index.ToString().PadLeft(5, '0'), Path.GetExtension(x));
                                File.Copy(x, Path.Combine(destinationFolderPath, destFileName));
                                index++;
                                progressTracker.TrackFileProcessed();
                            });

            var subFolders = Directory.GetDirectories(sourceFolderPath, "*.*", SearchOption.TopDirectoryOnly);
            Array.ForEach(subFolders, x => Process(x, Path.Combine(destinationFolderPath, Path.GetFileName(x)), sortingStrategy, progressTracker));
        }
    }
}