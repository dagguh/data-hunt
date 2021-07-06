using System.Collections.Generic;

namespace model
{
    public static class CollectionExtensions
    {
        public static IList<T> Copy<T>(this IList<T> original)
        {
            return new List<T>(original);
        }
    }
}
