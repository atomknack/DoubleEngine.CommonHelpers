using System;

namespace CollectionLike;

public interface IReadOnlySpanable<T>
{
    public ReadOnlySpan<T> AsReadOnlySpan();
}
