using Collections.Pooled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CollectionLike.Enumerables;

public static partial class Enumerables_Extension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotEmptyOrNull(this string s) => ! String.IsNullOrEmpty(s);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsNullOrEmpty(this string s) =>
        String.IsNullOrEmpty(s);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrEmpty<T>(this PooledList<T> array) => 
        array == null || array.Count == 0;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsNullOrEmpty<T>(this T[] array) => 
        array == null || array.Length == 0;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsNullOrEmpty<T>(this List<T> list) =>
        list == null || list.Count == 0;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsNullOrEmpty<T>(this IList<T> list) =>
        list is null || list.Count == 0; // 2022.08.11 == replaced by is
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsNullOrEmpty<T>(this IEnumerable<T> items) => // need testing
        items is null || !items.Any(); // 2022.08.11 == replaced by is
}