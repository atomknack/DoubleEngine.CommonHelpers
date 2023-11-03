using System;
using System.Buffers;
using Collections.Pooled;

namespace CollectionLike.Pooled;

public readonly partial struct PooledArrayStruct<T> : IDisposable, ISpanable<T>
{
    private static void ThrowIndexOutOfRangeException()
    {
        throw new IndexOutOfRangeException();
    }
    private static readonly T[] s_emptyArray = Array.Empty<T>();
    public static readonly PooledArrayStruct<T> Empty = new PooledArrayStruct<T>(0);
    private readonly T[] _items; // Do not rename (binary serialization)
    private readonly int _size; // Do not rename (binary serialization)
    private readonly ClearMode _clearMode;

    public T this[int index] 
    { 
        get {
            if ((uint)index >= (uint)_size)
            {
                ThrowIndexOutOfRangeException();
            }
            return _items[index]; } 
        set {
            if ((uint)index >= (uint)_size)
            {
                ThrowIndexOutOfRangeException();
            }
            _items[index] = value; } 
    }
    public int Count => _size;

    [Obsolete("need testing, especially for memory leaks when using with using")]
    public PooledArrayStruct<T> InvalidateIfNeededAndReturnResized(int newLength)
    {
        if (newLength < 0)
            throw new ArgumentOutOfRangeException(nameof(newLength));
        if (newLength <= _items.Length)
        {
            if (_clearMode == ClearMode.Always)
                Clear();
            return new PooledArrayStruct<T>(this, newLength, _clearMode);
        }
        this.Dispose();
        return new PooledArrayStruct<T>(newLength);
    }

    public static void ReSize(ref PooledArrayStruct<T> array, int newLength)
    {
        array = array.InvalidateIfNeededAndReturnResized(newLength);
    }

    public Span<T> AsSpan() => _items.AsSpan(0, _size);
    public ReadOnlySpan<T> AsReadOnlySpan() => new ReadOnlySpan<T>(_items, 0, _size);
    public void Dispose()
    {
        if ((_items is not null) && _items.Length != 0)
        {
            if (_clearMode == ClearMode.Always)
                ArrayPool<T>.Shared.Return(_items, clearArray: true);
            else
                ArrayPool<T>.Shared.Return(_items, clearArray: false);
        }
    }
    /*
    public PooledArrayStruct(int length, ClearMode clearMode): this(length)
    {

        if (clearMode != ClearMode.Never)
            throw new NotImplementedException("currently only available mode is ClearMode.Never");
    }*/
    public PooledArrayStruct() : this(0) { }
    public PooledArrayStruct(int length, ClearMode clearMode = ClearMode.Never)
    {
        if (clearMode == ClearMode.Auto)
            throw new NotImplementedException("auto is not available mode for PooledArrayStruct");
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));
        _size = length;
        _items = length == 0? s_emptyArray : ArrayPool<T>.Shared.Rent(length);
        _clearMode = clearMode;
    }

    private PooledArrayStruct(PooledArrayStruct<T> toResize, int size, ClearMode clearMode = ClearMode.Never)
    {
        _items = toResize._items;
        _size = size;
        _clearMode = clearMode;
    }
}
