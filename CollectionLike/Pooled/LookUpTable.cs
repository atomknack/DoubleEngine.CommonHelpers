using System;
using System.Buffers;
using System.Linq;
using Collections.Pooled;
using CollectionLike.Comparers;

using CollectionLike.Enumerables;


namespace CollectionLike.Pooled;

public class LookUpTable<T>: IDisposable
{
    private const int MaxArrayLength = 2_100_000_001;
    private const int HalfishArrayLength = 1_050_000_001;
    private const int DefaultCapacity = 16;
    private static readonly T[] s_emptyArray = Array.Empty<T>();
    private static readonly Func<T,T,bool> s_defaultEqualsComparer = StaticComparers<T>.TTEquals;
    private static readonly int s_manyDefragmentAtCount = 34;

    private Func<T, T, bool> _valueEqualityComparer;

    private ArrayPool<T> _poolT;
    private ArrayPool<RelativeMemory> _poolRelativeMemory;

    private T[] _store;
    private int _lastKeyInStore;
    private int _storeSize;
    private bool _clearStoreOnFree;
    private bool _disposed;

    private int _keysLength;
    private RelativeMemory[] _values;
    private PooledList<RelativeMemory> _oneItemSizedHoles;
    private PooledList<RelativeMemory> _twoItemSizedHoles;
    private PooledList<RelativeMemory> _manyItemSizedHoles;

    public int Debug_GetNumberOfAllKeys() => _keysLength;
    public int Debug_GetNumberOfUsedKeys()
    {
        int count = 0;
        for (int i = 0; i < _values.Length; ++i)
            if (_values[i].length>0) ++count;
        return count;
    } 
    public int Debug_GetCount()
    {
        int count = 0;
        for (int i = 0; i < _values.Length; ++i)
            count+=_values[i].length;
        return count;
    }
    public T[] Debug_GetInternalArray() => _store;
    public int Debug_GetInternalArrayLength() => _store.Length;
    public (int one, int two, int many) Debug_ElementsOcupiedMemory() => (_oneItemSizedHoles.Sum(x=>x.length), _twoItemSizedHoles.Sum(x => x.length), _manyItemSizedHoles.Sum(x => x.length));

    public LookUpTable(int keysLength, Func<T,T,bool> valueComparer = null, int startingCapacity = DefaultCapacity)
    {
        if (keysLength < 0)
            throw new ArgumentOutOfRangeException(nameof(keysLength));
        _keysLength = keysLength;

        _valueEqualityComparer = valueComparer==null ? s_defaultEqualsComparer : valueComparer;
        startingCapacity = startingCapacity < DefaultCapacity ? DefaultCapacity : startingCapacity;

        _poolT = ArrayPool<T>.Shared;
        _store = _poolT.Rent(startingCapacity);
        _poolRelativeMemory = ArrayPool<RelativeMemory>.Shared;
        _values = _poolRelativeMemory.Rent(_keysLength);
        _values.ClearArray();
        _lastKeyInStore = -1;

        _oneItemSizedHoles = Expendables.CreateList<RelativeMemory>(5);
        _twoItemSizedHoles= Expendables.CreateList<RelativeMemory>(10);
        _manyItemSizedHoles= Expendables.CreateList<RelativeMemory>(20);

        _clearStoreOnFree = false;
        _disposed = false;
    }
    public void Dispose()
    {
        if (_disposed)
            throw new ObjectDisposedException(GetType().FullName);
        _poolT.Return(_store,_clearStoreOnFree);
        _poolRelativeMemory.Return(_values, _clearStoreOnFree);
        _oneItemSizedHoles.Dispose();
        _twoItemSizedHoles.Dispose();
        _manyItemSizedHoles.Dispose();
        _disposed = true;
    }

    public void AddItem(int key, T value)
    {
        var currents = _values[key].GetReadOnlySpan(_store);
        foreach (var item in currents)
            if (_valueEqualityComparer(value, item))
                return;
        if(key == _lastKeyInStore)
        {
            AddToExistingLast(key, value);
            return;
        }
        if(currents.Length == 0)
        {
            AddOneSized(key, value);
            return;
        }
        if(currents.Length == 1)
        {
            AddTwoSized(key, value);
            return;
        }
        if (currents.Length > 1)
        {
            AddManySized(key, value);
            return;
        }
        throw new ArgumentException($"key: {key}, value: {value}, currents length: {currents.Length}");
    }
    private void AddManySized(int key, T value)
    {
        var current = _values[key];
        var currentSpan = current.GetReadOnlySpan(_store);
        int requiredSpanSize = current.length+1;
        for (int i = _manyItemSizedHoles.Count-1; i > -1; --i)
            if(requiredSpanSize == _manyItemSizedHoles[i].length)
            {
                var hole = _manyItemSizedHoles[i];
                var holeSpan = hole.GetSpan(_store);
                currentSpan.CopyTo(holeSpan);
                holeSpan[currentSpan.Length] = value;
                ReleaseRelativeMemory(key);
                _values[key] = hole;
                _manyItemSizedHoles.RemoveAt(i);
                return;
            }
        AddAsNewLast(key, value);
        DefragmentIfNeeded();
    }

    private void DefragmentIfNeeded()
    {
        if(_manyItemSizedHoles.Count> s_manyDefragmentAtCount)
        {
            var lastRelativeMemory = _values[_lastKeyInStore];
            T[] newArray = _poolT.Rent(lastRelativeMemory.start + lastRelativeMemory.length + 10);
            int newLastKeyInStore = -1;
            int startOfNewPlaceholder = 0;
            for (int i = 0; i < _keysLength; i++)
            {
                var oldRelative = _values[i];
                if (oldRelative.length == 0)
                    continue;
                var oldRelativeSpan = oldRelative.GetReadOnlySpan(_store);
                var newRelative = RelativeMemory.Create(startOfNewPlaceholder, oldRelative.length);
                var newRelativeSpan = newRelative.GetSpan(newArray);
                oldRelativeSpan.CopyTo(newRelativeSpan);
                _values[i] = newRelative;
                startOfNewPlaceholder = startOfNewPlaceholder + newRelative.length;
                newLastKeyInStore = i;
            }
            ReturnArray();
            _store = newArray;
            SetAsLastElement(newLastKeyInStore);
            _oneItemSizedHoles.Clear();
            _twoItemSizedHoles.Clear();
            _manyItemSizedHoles.Clear();

        }
    }

    private void AddTwoSized(int key, T value)
    {
        if (_twoItemSizedHoles.Count > 0)
        {
            var hole = _twoItemSizedHoles.PopLast();
            _store[hole.start] = _store.GetFirst(_values[key]);//currents[0];
            _store[hole.start + 1] = value;
            ReleaseRelativeMemory(key);
            _values[key] = hole;
            return;
        }
        AddAsNewLast(key, value);
    }

        private void AddOneSized(int key, T value)
    {
        if (_oneItemSizedHoles.Count > 0)
        {
            var hole = _oneItemSizedHoles.PopLast();
            _store[hole.start] = value;
            _values[key] = hole;
            return;
        }
        AddAsNewLast(key, value);
    }

    private void AddToExistingLast(int key, T value)
    {
        var memory = _values[key];
        EnsureCapacity(_storeSize + 1);
        _store[memory.start + memory.length] = value;
        _values[key] = RelativeMemory.Create(memory.start, memory.length + 1);
        SetAsLastElement(key);
    }

    private void AddAsNewLast(int key, T value)
    {
        var span = GetValues(key);
        EnsureCapacity(_storeSize + 1 + span.Length);
        int startOfNewPlaceholder = _storeSize;
        int countNewPlaceholder = 0;
        for(int i = 0; i < span.Length; ++i)
        {
            _store[startOfNewPlaceholder+countNewPlaceholder] =span[i];
            ++countNewPlaceholder;
        }
        _store[startOfNewPlaceholder + countNewPlaceholder] = value;
        ++countNewPlaceholder;
        ReleaseRelativeMemory(key);
        _values[key] = RelativeMemory.Create(startOfNewPlaceholder, countNewPlaceholder);
        SetAsLastElement(key);
        //EnsureCapacity()
    }

    private void SetAsLastElement(int key)
    {
        _storeSize = _values[key].start + _values[key].length;
        _lastKeyInStore = key;
    }

    void ReleaseRelativeMemory(int key)
    {
        var relativeMemory = _values[key];
        if (relativeMemory.length == 0)
            return;
        if (relativeMemory.length == 1)
        {
            _oneItemSizedHoles.Add(relativeMemory);
            return;
        }
        if (relativeMemory.length == 2)
        {
            _twoItemSizedHoles.Add(relativeMemory);
            return;
        }

        if (relativeMemory.length < 0)
            throw new ArgumentException();

        _manyItemSizedHoles.Add(relativeMemory);
        //throw new NotImplementedException();
    }

    public ReadOnlySpan<T> GetValues(int key) => _values[key].GetReadOnlySpan<T>(_store);
    [Obsolete("need testing")]
    public int GetValuesLength(int key) => _values[key].length;

    private void EnsureCapacity(int min)
    {
        if (_store.Length < min)
        {
            int newCapacity = _store.Length>HalfishArrayLength ? MaxArrayLength : _store.Length * 2;
            if (newCapacity < min) newCapacity = min;
            if (newCapacity > MaxArrayLength) newCapacity = MaxArrayLength;
            IncreaseCapacity(newCapacity);
        }
    }
    private void IncreaseCapacity(int value)
    {
        if (value <= _store.Length)
            throw new ArgumentOutOfRangeException(nameof(value));

        var newItems = _poolT.Rent(value);
        Array.Copy(_store, newItems, _storeSize);
        ReturnArray();
        _store = newItems;
    }

    private void ReturnArray()
    {
        if (_store.Length == 0)
            return;
        try
        {
            _poolT.Return(_store, clearArray: _clearStoreOnFree); //_clearIfNeed to reclaim refereces
        }
        catch (ArgumentException)
        {
            //pool didn't like our array
        }
        _store = s_emptyArray;
    }
}