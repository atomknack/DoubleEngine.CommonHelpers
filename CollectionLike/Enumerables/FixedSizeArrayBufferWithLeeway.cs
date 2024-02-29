using System;
using CollectionLike;

namespace CollectionLike.Enumerables;

public partial class FixedSizeArrayBufferWithLeeway<T> : ISpanable<T>
{
    private static void ThrowIndexOutOfRangeException()
    {
        throw new IndexOutOfRangeException();
    }
    private const int ADDITIONALLEEWAY = 131;

    [Obsolete("Use only for testing")]
    public static int TESTING_GetADDITIONALLEEWAY() => ADDITIONALLEEWAY;

    private readonly int _requestedSize;
    private readonly int _maxAllowedIndex;
    private readonly T[] _items;
    private int _count;

    public int GetRealArrayFullLength() => _items.Length;

    public T this[int index]
    {
        get
        {
            if ((uint)index >= (uint)_count)
            {
                ThrowIndexOutOfRangeException();
            }
            return _items[index];
        }
        set
        {
            if ((uint)index >= (uint)_count)
            {
                ThrowIndexOutOfRangeException();
            }
            _items[index] = value;
        }
    }
    public int Count => _count;

    public bool CanAddMore() => _count < _requestedSize;
    public void Add(T item)
    {
        if (_count >= _maxAllowedIndex)
            ThrowIndexOutOfRangeException();
        _items[_count] = item;
        ++_count;
    }
    public void Clear()
    {
        _count = 0;
    }

    public Span<T> AsSpan() => _items.AsSpan(0, _count);
    public ReadOnlySpan<T> AsReadOnlySpan() => new ReadOnlySpan<T>(_items, 0, _count);


    public FixedSizeArrayBufferWithLeeway(int length)
    {
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));
        _requestedSize = length;
        _maxAllowedIndex = _requestedSize + ADDITIONALLEEWAY;
        _items = new T[_maxAllowedIndex + 1];
        _count = 0;
    }
}
