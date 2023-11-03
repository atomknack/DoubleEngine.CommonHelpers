///2020. Pavel Ivanov.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CollectionLike.Enumerables;
public static partial class Enumerables_Extension
{
    [Obsolete("need testing, inefficient way, probably need rewrite")]
    public static List<int> InvertedIndexesSelection(this List<int> selected, int allIndexesLength)
    {
        HashSet<int> selectedSet = new HashSet<int>(selected);
        List<int> result = new List<int>();
        for(int i = 0; i < allIndexesLength; ++i)
            if( ! selectedSet.Contains(i) )
                result.Add(i);
        return result;
    }

    [Obsolete("Directly use version with ReadOnlySpan instead")]
    public static bool Found<T>(this T[] items, Predicate<T> match, out T found, out int indexOfFound, int startSearchFromIndex = 0) where T : new()
        => Found(new ReadOnlySpan<T>(items), match, out found, out indexOfFound, startSearchFromIndex);
    [Obsolete("Directly use version with ReadOnlySpan instead")]
    public static bool Found<T>(this ImmutableArray<T> items, Predicate<T> match, out T found, out int indexOfFound, int startSearchFromIndex = 0) where T : new()
        => Found(items.AsSpan(), match, out found, out indexOfFound, startSearchFromIndex);
    [Obsolete("need testing")]
    public static bool Found<T>(this ReadOnlySpan<T> items, Predicate<T> match, out T found, out int indexOfFound, int startSearchFromIndex = 0) where T : new()
    {
        for(int i = startSearchFromIndex; i < items.Length; i++)
            if (match(items[i]))
            {
                found = items[i];
                indexOfFound = i;
                return true;
            }
        found = new();
        indexOfFound = 0;
        return false;
    }
    public static IEnumerable<(int, T)> EnumerateReverse<T>(this T[] items)
    {
        //Debug.Log("EnumerateReverse Array extension");
        for (int i = items.Length - 1; i >= 0; i--)
            yield return (i, items[i]);
    }
    public static IEnumerable<(int, T)> EnumerateReverse<T>(this IList<T> items)
    {
        //Debug.Log("EnumerateReverse IList extension");
        for (int i = items.Count - 1; i >= 0; i--)
            yield return (i, items[i]);
    }
    public static IEnumerable<T> Reversed<T>(this T[] items)
    {
        //Debug.Log("FastReverse Array extension");
        for (int i = items.Length - 1; i >= 0; i--)
            yield return items[i];
    }
    public static IEnumerable<T> Reversed<T>(this IList<T> items)
    {
        //Debug.Log("FastReverse IList extension");
        for (int i = items.Count - 1; i >= 0; i--)
            yield return items[i];
    }

    public static IEnumerable<(int, T)> Enumerate<T>(this T[] items)
    {
        //Debug.Log("Enumerate Array extension");
        for (int i = 0; i < items.Length; i++)
            yield return (i, items[i]);
    }

    /// <summary>
    /// Better use ReverseEnumerate for list
    /// </summary>
    public static IEnumerable<(int, T)> Enumerate<T>(this IList<T> items)
    {
        // Debug.Log("Enumerate IList extension, Do Not Use if you don't have to");
        /// Can possible lead to Out of bounds exception if element was deleted in time of iteration
        /// So we will convert it to array first
        T[] arr = items.ToArray();
        return arr.Enumerate();
    }

}

