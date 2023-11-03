using System;
using System.Collections;
using System.Collections.Generic;

namespace CollectionLike.Pooled;

public readonly partial struct PooledArrayStruct<T> : IList<T>, IList
{
    object IList.this[int index] { get => this[index]; set => this[index] = (T)value; }

    public bool IsReadOnly => false;

    bool IList.IsFixedSize => true;

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => 
        throw new NotImplementedException();

    public void Clear()
    {
        //throw new NotImplementedException();
        //for(int i = 0; i < _size; ++i)
        //    _items[i] = default(T);
        Array.Clear(_items, 0, _size);
    }
    public bool Contains(T item)
    {
        if (IndexOf(item) != -1)
            return true;
        return false;
    }
    bool IList.Contains(object value) => 
        Contains((T)value);

    public void CopyTo(T[] array, int arrayIndex)
    {
        for(int i = 0; i < _size; ++i)
            array[i+arrayIndex] = _items[i];

    }
    void ICollection.CopyTo(Array array, int index)
    {
        CopyTo((T[])array, index);
    }

    public int IndexOf(T item)
    {
        for (int i = 0; i < _size; ++i)
            if (_items[i].Equals(item))
                return i;
        return -1;
    }
    int IList.IndexOf(object value) =>
        IndexOf((T)value);

    void ICollection<T>.Add(T item)
    {
        throw new NotImplementedException();
    }
    int IList.Add(object value)
    {
        throw new NotImplementedException();
    }

    void IList<T>.Insert(int index, T item)
    {
        throw new NotImplementedException();
    }

    void IList.Insert(int index, object value)
    {
        throw new NotImplementedException();
    }

    bool ICollection<T>.Remove(T item)
    {
        throw new NotImplementedException();
    }

    void IList.Remove(object value)
    {
        throw new NotImplementedException();
    }

    void IList<T>.RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    void IList.RemoveAt(int index)
    {
        throw new NotImplementedException();
    }
}
