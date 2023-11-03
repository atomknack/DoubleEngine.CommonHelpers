using System;

namespace CollectionLike;

public readonly struct RelativeMemory //need testing
{
    public static readonly RelativeMemory Empty = Create(0, 0);

    internal readonly int start;
    public readonly int length;

    public bool IsEmpty() => length == 0;
    public static RelativeMemory Create(int start, int length) => new RelativeMemory(start, length);
    public RelativeMemory Slice(int start) => new RelativeMemory(this, start);
    public RelativeMemory Slice(int start, int length) => new RelativeMemory(this, start, length);
    public Span<T> GetSpan<T>(T[] array) => new Span<T>(array, start, length);
    public Span<T> GetSpan<T>(Span<T> source) => source.Slice(start, length);
    //public Span<T> GetSpan<T>(PooledList<T> pooled) => pooled.Span.Slice(start, length);
    public ReadOnlySpan<T> GetReadOnlySpan<T>(T[] source) => new ReadOnlySpan<T>(source, start, length);
    public ReadOnlySpan<T> GetReadOnlySpan<T>(ReadOnlySpan<T> source) => source.Slice(start, length);
    //public ReadOnlySpan<T> GetReadOnlySpan<T>(PooledList<T> pooled) => pooled.Span.Slice(start, length);

    private RelativeMemory(int start, int length)
    {
        NonNegativeCheck(start, length);
        this.start = start;
        this.length = length;
    }
    private RelativeMemory(RelativeMemory toSlice, int start)
    {
        this.length = toSlice.length - start;
        NonNegativeCheck(start, length);
        this.start = toSlice.start + start;
    }
    private RelativeMemory(RelativeMemory toSlice, int start, int length)
    {
        NonNegativeCheck(start, length);
        TooLongCheck(toSlice.length, start, length);
        this.start = toSlice.start + start;
        this.length = length;
    }
    private static void TooLongCheck(int maxLength, int start, int length)
    {
        if (start + length > maxLength)
            throw new ArgumentOutOfRangeException($"start+length should not be more than maxLength: start:{start}, length:{length}, maxLength: {maxLength}");
    }
    private static void NonNegativeCheck(int start, int length)
    {
        if (start < 0)
            throw new ArgumentOutOfRangeException($"start: {start} should be >= 0");
        if (length < 0)
            throw new ArgumentOutOfRangeException($"start: {start} should be >= 0");
    }
}
