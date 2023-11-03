///2020. Pavel Ivanov.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;


namespace CollectionLike.Enumerables;

public static partial class Enumerables_Extension
{
    public static T Last<T>(this ImmutableArray<T> items) => items[items.Length - 1];
    public static T Last<T>(this T[] items) => items[items.Length - 1];
    public static T Last<T>(this List<T> items) => items[items.Count - 1];
    public static T Last<T>(this IReadOnlyList<T> items) => items[items.Count - 1];
    public static T Last<T>(this IList<T> items) => items[items.Count - 1];
    public static T Last<T>(this Span<T> items) => items[items.Length - 1];
    public static T Last<T>(this ReadOnlySpan<T> items) => items[items.Length - 1];
    public static int IndexOfLast<T>(this ImmutableArray<T> items) => items.Length - 1;
    public static int IndexOfLast<T>(this T[] items) => items.Length - 1;
    public static int IndexOfLast<T>(this List<T> items) => items.Count - 1;
    public static int IndexOfLast<T>(this IReadOnlyList<T> items) => items.Count - 1;
    public static int IndexOfLast<T>(this IList<T> items) => items.Count - 1;
    public static int IndexOfLast<T>(this Span<T> items) => items.Length - 1;
    public static int IndexOfLast<T>(this ReadOnlySpan<T> items) => items.Length - 1;

}