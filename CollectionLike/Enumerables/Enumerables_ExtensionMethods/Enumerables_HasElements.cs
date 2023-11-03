using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CollectionLike.Enumerables;

public static partial class Enumerables_Extension
{
    // all need testing
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool HasElements(this string s) => 
        !String.IsNullOrEmpty(s);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool HasElements<T>(this T[] array) => 
        array != null && array.Length != 0;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool HasElements<T>(this List<T> list) =>
        list != null && list.Count != 0;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool HasElements<T>(this IList<T> list) =>
        list != null && list.Count != 0;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool HasElements<T>(this IEnumerable<T> items) =>
        items != null && items.Any();
}