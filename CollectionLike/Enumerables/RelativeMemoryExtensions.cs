using System;

namespace CollectionLike.Enumerables;

public static class RelativeMemoryExtensions
{
    [Obsolete("need testing")]
    public static void ClearArray(this RelativeMemory[] array)
    {
        for (int i = 0; i < array.Length; ++i)
            array[i] = RelativeMemory.Empty;
    }
    [Obsolete("need testing")]
    public static T GetItemRelative<T>(this T[] items, RelativeMemory memory, int relativeIndex)
    {
        if (relativeIndex < 0 || relativeIndex >= memory.length)
            throw new ArgumentOutOfRangeException(nameof(relativeIndex));
        return items[memory.start + relativeIndex];
    }
    [Obsolete("need testing")]
    public static T GetItemRelative<T>(this Span<T> items, RelativeMemory memory, int relativeIndex)
    {
        if (relativeIndex < 0 || relativeIndex >= memory.length)
            throw new ArgumentOutOfRangeException(nameof(relativeIndex));
        return items[memory.start + relativeIndex];
    }
    [Obsolete("need testing")]
    public static T GetItemRelative<T>(this ReadOnlySpan<T> items, RelativeMemory memory, int relativeIndex)
    {
        if (relativeIndex < 0 || relativeIndex >= memory.length)
            throw new ArgumentOutOfRangeException(nameof(relativeIndex));
        return items[memory.start + relativeIndex];
    }

    [Obsolete("need testing")]
    public static T GetFirst<T>(this T[] items, RelativeMemory memory)
    {
        if (memory.length < 1)
            throw new ArgumentOutOfRangeException(nameof(memory.length));
        return items[memory.start];
    }
    [Obsolete("need testing")]
    public static T GetFirst<T>(this Span<T> items, RelativeMemory memory)
    {
        if (memory.length < 1)
            throw new ArgumentOutOfRangeException(nameof(memory.length));
        return items[memory.start];
    }
    [Obsolete("need testing")]
    public static T GetFirst<T>(this ReadOnlySpan<T> items, RelativeMemory memory)
    {
        if (memory.length < 1)
            throw new ArgumentOutOfRangeException(nameof(memory.length));
        return items[memory.start];
    }
}
