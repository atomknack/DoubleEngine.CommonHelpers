using System;
using System.Collections.Generic;
using Collections.Pooled;

namespace CollectionLike.Pooled;

public static partial class Expendables
{
    public static bool ByteComparer (byte x, byte y) => s_byteComparer(x,y);
    public static bool IntComparer(int x, int y) => S_intComparer(x,y);
    private static bool s_byteComparer(byte x, byte y) => x == y;
    private static bool S_intComparer(int x, int y) => x == y;

    public static PooledArrayStruct<T> CreateArray<T>(int length) => new PooledArrayStruct<T>(length, ClearMode.Never);
    public static PooledArrayStruct<T> CreateArrayFromSpan<T>(ReadOnlySpan<T> span)
    {
        PooledArrayStruct<T> pooled = CreateArray<T>(span.Length);
        span.CopyTo(pooled.AsSpan());
        return pooled;
    }
    public static PooledArrayStruct<int> CreateIncreasingArray(int length, int start = 0)
    {
        var result = CreateArray<int>(length);
        var span = result.AsSpan().FillAsRange(0);
        return result;
    }
    public static PooledList<T> CreateList<T>(int length) => new PooledList<T>(length, ClearMode.Never);
    public static PooledList<T> CreateList<T>(int length, bool sizeToCapacity = false) => new PooledList<T>(length, ClearMode.Never, sizeToCapacity);
    public static PooledStack<T> CreateStack<T>(int length) => new PooledStack<T>(length, ClearMode.Never);
    public static PooledQueue<T> CreateQueue<T>(int length) => new PooledQueue<T>(length, ClearMode.Never);
    public static PooledQueue<T> CreateQueueFromSet<T>(PooledSet<T> set) => new PooledQueue<T>(set);
    public static PooledDictionary<TKey, TValue> CreateDictionary<TKey, TValue>(int length) => 
        new PooledDictionary<TKey, TValue>(length, ClearMode.Never);
    public static PooledSet<T> CreateSet<T>(int length) =>
        new PooledSet<T>(length, ClearMode.Never);

    public static LookUpTable<byte> CreateEmptyLookUpForMaterials(int verticeLength) =>
        new LookUpTable<byte>(verticeLength, s_byteComparer, verticeLength * 2 + 10);

    public static PooledList<T> Clone<T>(this PooledList<T> original)
    {
        var result = new PooledList<T>(original.Capacity, original.ClearMode);
        var resultSpan = result.AddSpan(original.Count);
        var originalSpan = original.Span;
        for (int i = 0; i < resultSpan.Length; ++i)
            resultSpan[i] = originalSpan[i];
        return result;
    }

    public static void AddAllWhere<T>(this PooledList<T> list, T[] from, Predicate<T> that) =>
        AddAllWhere(list, from.AsSpan(), that);
    public static void AddAllWhere<T>(this PooledList<T> list, PooledList<T> from, Predicate<T> that) =>
        AddAllWhere(list, from.Span, that);
    public static void AddAllWhere<T>(this PooledList<T> list, ReadOnlySpan<T> from, Predicate<T> that)
    {
        for (int i = 0; i < from.Length; ++i)
        {
            var item = from[i];
            if (that(item))
                list.Add(item);
        }
    }
    public static void AddAllWhere<T>(this PooledList<T> list, IReadOnlyCollection<T> from, Predicate<T> that)
    {
        foreach (var item in from)
            if (that(item))
                list.Add(item);
    }

}
