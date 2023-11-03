using System;
using System.Runtime.CompilerServices;

namespace CollectionLike;

public static class ReadOnlySpanExtensions
{
    [MethodImpl(256)] public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T[] arr) => (ReadOnlySpan<T>)arr;
    [MethodImpl(256)] public static ReadOnlySpan<T> AsReadOnly<T>(this Span<T> span) => (ReadOnlySpan<T>)span;
}
