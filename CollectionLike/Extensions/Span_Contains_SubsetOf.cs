
using System;
using System.Collections.Generic;
using System.Text;

namespace CollectionLike;

public static class Span_Contains_SubsetOf
{
    public static bool SubsetOf<T>(this ReadOnlySpan<T> items, ReadOnlySpan<T> superset, Func<T, T, bool> equalComparer)
    {
        for (int i = 0; i < items.Length; ++i)
            if (false == superset.Contains(items[i], equalComparer))
                return false;
        return true;
    }
    public static bool Contains<T>(this Span<T> items, T target, Func<T, T, bool> equalComparer) =>
    Contains<T>((ReadOnlySpan<T>)items, target, equalComparer);
    public static bool Contains<T>(this ReadOnlySpan<T> items, T target, Func<T, T, bool> equalComparer)
    {
        for (int i = 0; i < items.Length; ++i)
            if (equalComparer(items[i], target))
                return true;
        return false;
    }
    public static bool Contains<T>(this ReadOnlySpan<T> items, T target) where T : IEquatable<T>
    {
        for (int i = 0; i < items.Length; ++i)
            if (target.Equals(items[i]))
                return true;
        return false;
    }
}
