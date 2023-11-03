using System;

namespace CollectionLike.Comparers;

public static partial class StaticComparers
{
    public static readonly Func<long, long, bool> LongLongDefault = (x, y) => x == y;
}
