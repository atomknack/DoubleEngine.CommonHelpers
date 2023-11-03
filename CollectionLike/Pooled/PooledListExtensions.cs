using System;
using Collections.Pooled;
using CollectionLike;

namespace CollectionLike.Pooled;

public static partial class Expendables
{
    public static void AddRepeating<T>(this PooledList<T> list, T value, int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException("count");
        for(int i = 0; i < count; ++i)
            list.Add(value);
    }
    [Obsolete("need testing")]
    public static void RemoveAt_UseLastToFillHole<T>(this PooledList<T> list, int index)
    {
        var indexOfLastElement = list.IndexOfLast();
        list[index] = list[indexOfLastElement];
        list.RemoveAt(indexOfLastElement);
    }

    public static ReadOnlySpan<T> GetRelativeSpan<T>(this PooledList<T> list, RelativeMemory relative) =>
        list.Span.Slice(relative.start,relative.length).AsReadOnly();
    public static RelativeMemory AppendSpanAndReturnRelativeMemory<T>(this PooledList<T> list, ReadOnlySpan<T> items)
    {
        if (items.Length == 0)
            return RelativeMemory.Empty;
        int start = list.Count;
        int count = items.Length;
        for (int i = 0; i < count; ++i)
            list.Add(items[i]);
        return RelativeMemory.Create(start, count);
    }
    public static void AppendSpan<T>(this PooledList<T> list, ReadOnlySpan<T> items)
    {
        if (items.Length == 0)
            return;
        int count = items.Length;
        for (int i = 0; i < count; ++i)
            list.Add(items[i]);
    }

    public static Span<T> AsSpan<T>(this PooledList<T> arr) => arr.Span;
    public static ReadOnlySpan<T> AsReadOnlySpan<T>(this PooledList<T> arr) => (ReadOnlySpan<T>)arr.Span;

    public static int IndexOfLast<T>(this PooledList<T> items) => items.Count - 1;
    public static T Last<T>(this PooledList<T> items) => items[items.Count - 1];
    public static bool Contains<T>(this PooledList<T> list, T target, Func<T, T, bool> equalComparer) =>
        list.Span.Contains<T>(target, equalComparer);

}