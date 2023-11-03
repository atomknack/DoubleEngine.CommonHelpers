using System;

namespace CollectionLike;

public interface ISpanable<T> : IReadOnlySpanable<T>
{
    public Span<T> AsSpan();
}
