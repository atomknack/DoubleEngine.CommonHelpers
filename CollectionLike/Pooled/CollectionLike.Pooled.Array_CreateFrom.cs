
using System;
using Collections.Pooled;

namespace CollectionLike.Pooled;

public readonly partial struct PooledArrayStruct<T>
{
    public static PooledArrayStruct<TItem> CreateAsCopyFromSpan<TItem>(ReadOnlySpan<TItem> span)
    {
        PooledArrayStruct<TItem> result = Expendables.CreateArray<TItem>(span.Length);
        span.CopyTo(result.AsSpan());
        return result;
    }
    public static PooledArrayStruct<TKey> CreateFromDictionaryKeys<TKey, TValue>(PooledDictionary<TKey,TValue> dict)
    {
        PooledArrayStruct<TKey> result = Expendables.CreateArray<TKey>(dict.Count);
        dict.Keys.CopyTo(result._items,0);
        return result;
    }
    public static PooledArrayStruct<TValue> CreateFromDictionaryValues<TKey, TValue>(PooledDictionary<TKey, TValue> dict)
    {
        PooledArrayStruct<TValue> result = Expendables.CreateArray<TValue>(dict.Count);
        dict.Values.CopyTo(result._items, 0);
        return result;
    }
    public static PooledArrayStruct<TItem> CreateFromSet<TItem>(PooledSet<TItem> set)
    {
        PooledArrayStruct<TItem> result = Expendables.CreateArray<TItem>(set.Count);
        set.CopyTo(result.AsSpan());
        return result;
    }
    public static PooledArrayStruct<TItem> CreateFromList<TItem>(PooledList<TItem> list)
    {
        PooledArrayStruct<TItem> result = Expendables.CreateArray<TItem>(list.Count);
        list.CopyTo(result.AsSpan());
        return result;
    }
}
