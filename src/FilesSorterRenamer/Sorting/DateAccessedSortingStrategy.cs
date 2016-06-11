using System;
using System.IO;

namespace FilesSorterRenamer.Sorting
{
    internal class DateAccessedSortingStrategy : ISortingStrategy
    {
        public DateTime GetDate(string fileFullPath)
        {
            return new DirectoryInfo(fileFullPath).LastAccessTime;
        }
    }
}