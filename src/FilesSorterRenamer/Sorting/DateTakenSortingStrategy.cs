using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FilesSorterRenamer.Sorting
{
    internal class DateTakenSortingStrategy : ISortingStrategy
    {
        private static readonly Regex R = new Regex(":");

        public DateTime GetDate(string fileFullPath)
        {
            using (var fs = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read))
            using (var myImage = Image.FromStream(fs, false, false))
            {
                var propItem = myImage.GetPropertyItem(36867);
                var dateTaken = R.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }
    }
}