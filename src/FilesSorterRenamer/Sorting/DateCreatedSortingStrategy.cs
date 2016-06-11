using System;
using System.IO;

namespace FilesSorterRenamer.Sorting
{
    internal class DateCreatedSortingStrategy : ISortingStrategy
    {
        public DateTime GetDate(string fileFullPath)
        {
            return new DirectoryInfo(fileFullPath).CreationTime;
        }
    }
}