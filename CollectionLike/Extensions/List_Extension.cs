using System;
using System.Collections.Generic;

namespace CollectionLike;

public static partial class CollectionLike_Extension
{
    
    public static int RemoveAll<T>(this List<T> list, T value) where T : IEquatable<T>
        => list.RemoveAll(x=>x.Equals(value));

    public static void RemoveAllElementsStartingFromIndex<T>(this List<T> list, int startIndex)
    {
        if (startIndex < 0)
            throw new ArgumentOutOfRangeException("Non-negative number required. (Parameter 'startIndex')");
        int count = list.Count;
        if (count <= startIndex)
            return;
        list.RemoveRange(startIndex, count-startIndex);
    }
    public static void InsertAtStart<T>(this List<T> list, T value) 
        => list.Insert(0, value);
}
