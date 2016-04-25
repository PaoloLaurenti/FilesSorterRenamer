using System;

namespace FilesSorterRenamer
{
    internal class ProgressTracker
    {
        private readonly int _allFilesCount;
        private readonly Action<int> _onPercentageProgressChanged;
        private int _fileProcessed;

        public ProgressTracker(int allFilesCount, Action<int> onPercentageProgressChanged)
        {
            _allFilesCount = allFilesCount;
            _onPercentageProgressChanged = onPercentageProgressChanged;
            _fileProcessed = 0;
        }

        internal void TrackFileProcessed()
        {
            _fileProcessed++;

            var percentage = Math.Round((double) (_fileProcessed*100)/_allFilesCount, 0);
            _onPercentageProgressChanged((int) percentage);
        }
    }
}