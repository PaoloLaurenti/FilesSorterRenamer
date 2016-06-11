using System;

namespace FilesSorterRenamer.Sorting
{
    internal class SortingStrategyFactory
    {
        internal static ISortingStrategy GetStrategy(SortingStrategy sortingStrategy)
        {
            switch (sortingStrategy)
            {
                case SortingStrategy.DateTaken:
                    return new DateTakenSortingStrategy();
                case SortingStrategy.DateCreated:
                    return new DateCreatedSortingStrategy();
                case SortingStrategy.DateModified:
                    return new DateModifiedSortingStrategy();
                case SortingStrategy.DateAccessed:
                    return new DateAccessedSortingStrategy();
            }

            throw new ArgumentOutOfRangeException("sortingStrategy", sortingStrategy, null);
        }
    }
}