using System;
using System.IO;

namespace FilesSorterRenamer.Sorting
{
    internal class DateModifiedSortingStrategy : ISortingStrategy
    {
        public DateTime GetDate(string fileFullPath)
        {
            return new DirectoryInfo(fileFullPath).LastWriteTime;
        }
    }
}