using System.Collections.Generic;
using System.Linq;

namespace NetrekClient
{
    static class Extension
    {
        public static IEnumerable<T> TakeAll<T>(this IEnumerable<T> arr, int count)
        {
            IEnumerable<T> taken = arr.Take(count);
            if(taken.Count() == count)
            {
                return taken;
            }
            T[] takenall = new T[count];
            taken.ToArray().CopyTo(takenall, 0);
            return takenall;
        }
    }
}
