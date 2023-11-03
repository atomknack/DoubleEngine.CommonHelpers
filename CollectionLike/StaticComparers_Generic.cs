using System;

namespace CollectionLike.Comparers;

public static partial class StaticComparers<T>
{
    public static readonly Func<T, T, bool> TTEquals = (x, y) => x.Equals(y);
}
