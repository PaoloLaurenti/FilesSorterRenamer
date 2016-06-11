namespace FilesSorterRenamer
{
    internal class FilesSorterRenamerCommandRequest
    {
        public string SourceFolderPath { get; set; }
        public string DestinationFolderPath { get; set; }
        public SortingStrategy SortingStrategy { get; set; }
    }
}