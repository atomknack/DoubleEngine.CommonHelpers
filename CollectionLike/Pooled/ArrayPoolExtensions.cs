using System;
using System.Buffers;

namespace CollectionLike.Pooled
{
    public static class ArrayPoolExtensions
    {
        public static T[] RentOrGetEmpty<T>(this ArrayPool<T> pool, int length)
        {
            if (length < 0)
                throw new Exception($"{nameof(length)} cannot be negative");
            if (length == 0)
                return Array.Empty<T>();
            return pool.Rent(length);
        }
        public static void ReleaseAndSetAsEmpty<T>(this ArrayPool<T> pool, ref T[] array)
        {
            if (array == null || array.Length == 0)
                return;
            pool.Return(array);
            array = Array.Empty<T>();
        }
    }
}
