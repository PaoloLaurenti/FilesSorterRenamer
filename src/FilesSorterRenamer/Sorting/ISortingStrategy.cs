using System;

namespace FilesSorterRenamer.Sorting
{
    internal interface ISortingStrategy
    {
        DateTime GetDate(string fileFullPath);
    }
}