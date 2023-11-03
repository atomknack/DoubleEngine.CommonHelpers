
using System;
using Collections.Pooled;

namespace CollectionLike.Pooled;

public static class Pooled_ExtensionMethods
{
    public static PooledArrayStruct<TKey> CreatePooledArrayFromKeys<TKey, TValue>(this PooledDictionary<TKey,TValue> dict) =>
        PooledArrayStruct<TKey>.CreateFromDictionaryKeys(dict);

    public static PooledArrayStruct<TValue> CreatePooledArrayFromValues<TKey, TValue>(this PooledDictionary<TKey, TValue> dict) =>
        PooledArrayStruct<TValue>.CreateFromDictionaryValues(dict);

    public static PooledArrayStruct<TItem> CreatePooledArray<TItem>(this PooledSet<TItem> set) => 
        PooledArrayStruct<TItem>.CreateFromSet(set);

    [Obsolete("not tested")]
    public static void Clear<T>(this PooledList<T> list, int startFrom)
    {
        if (startFrom < 0)
            throw new ArgumentOutOfRangeException(nameof(startFrom));
        if (startFrom >= list.Count)
            return;
        list.RemoveRange(startFrom, list.Count - startFrom);
    }

}
