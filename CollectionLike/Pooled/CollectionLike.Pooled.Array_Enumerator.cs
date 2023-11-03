using System;
using System.Collections;
using System.Collections.Generic;

namespace CollectionLike.Pooled;

public readonly partial struct PooledArrayStruct<T>: IEnumerable<T>, IEnumerable
{
    public Enumerator GetEnumerator()
        => new Enumerator(this);
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => new Enumerator(this);
    IEnumerator IEnumerable.GetEnumerator()
        => new Enumerator(this);

    public struct Enumerator : IEnumerator<T>, IEnumerator
    {
        private static void ThrowInvalidOperationException_EnumOperationShouldNeverHappen()
        {
            throw new InvalidOperationException("Invalid enumerator state which should never happen if enumerator used correctly");
        }

        private T[] _items;
        private int _size;
        private int _index;
        private T _current;

        internal Enumerator(PooledArrayStruct<T> arr)
        {
            _items = arr._items;
            _size = arr._size;
            _index = 0;
            _current = default;
        }

        public void Dispose()
        {
            //Enumerator not owns array, and should not Dispose of it
        }

        public bool MoveNext()
        {

            if (_index < _size)
            {
                _current = _items[_index];
                _index++;
                return true;
            }

            _index = _size + 1;
            _current = default;
            return false;
        }

        public T Current => _current;

        object IEnumerator.Current
        {
            get
            {
                if (_index == 0 || _index == _size + 1)
                    ThrowInvalidOperationException_EnumOperationShouldNeverHappen();
                return Current;
            }
        }

        void IEnumerator.Reset()
        {
            _index = 0;
            _current = default;
        }
    }
}
