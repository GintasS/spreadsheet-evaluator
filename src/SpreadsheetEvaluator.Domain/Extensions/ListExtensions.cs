using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Domain.Extensions
{
    public static class ListExtensions
    {
        public static bool HasMismatchingElementTypes<T>(this List<T> list) where T : struct
        {
            if (list == null || list.Count <= 1)
            {
                return false;
            }

            return list.Distinct().Count() != 1;
        }
    }
}
