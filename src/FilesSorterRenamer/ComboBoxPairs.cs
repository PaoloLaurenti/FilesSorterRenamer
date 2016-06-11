namespace FilesSorterRenamer
{
    public class ComboBoxPairs
    {
        public string Key { get; set; }
        public SortingStrategy Value { get; set; }

        public ComboBoxPairs(string key, SortingStrategy value)
        {
            Key = key;
            Value = value;
        }
    }
}