using CollectionLike.Comparers;
using System;
using System.Collections.Generic;

//#nullable enable
namespace CollectionLike.Enumerables;
public static partial class Enumerables_Extension
{
    [Obsolete("Not tested")]
    public static int Sum(this Span<int> span) => 
        Sum((ReadOnlySpan<int>)span);

    [Obsolete("Not tested")]
    public static int Sum(this ReadOnlySpan<int> span)
    {
        int result = 0;
        for (int i = 0; i < span.Length; ++i)
            result += span[i];
        return result;
    }

    public static int IndexPositiveCyclicRemapForLength(this int index, int length) => index % length;

    public static List<T> CreateNewListFromSpanElements<T>(this ReadOnlySpan<T> items) 
    {
        List<T> list = new List<T>(items.Length);
        for (int i = 0; i < items.Length; i++)
            list.Add(items[i]);
        return list;
    }
    [Obsolete("Not tested, need testing")]
    public static void AddRange<T>(this List<T> list, ReadOnlySpan<T> items) {
        //list.EnsureCapacity(list.Capacity + items.Length); //need net6 or higher
        for (int i = 0; i < items.Length; i++)
            list.Add(items[i]);
    }

    public static int IndexOf<TSource>(this Span<TSource> source, Func<TSource, bool> predicate, int startSearchFrom = 0) // not tested, need testing
    {

        if (predicate == null) 
            throw new ArgumentNullException("predicate is null");
        for (int i = startSearchFrom; i<source.Length;i++)
            if (predicate(source[i])) return i;
        return -1;
        //throw new Exception("No element found to return IndexOf");
    }

}
