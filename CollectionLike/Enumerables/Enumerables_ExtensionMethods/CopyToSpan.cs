using System;
using System.Collections.Generic;

namespace CollectionLike.Enumerables;

public static partial class Enumerables_Extension
{
    [Obsolete("is it working? - unknown. Not tested, need testing")]
    public static void CopyTo<T>(this List<T> fromList, int fromIndex, Span<T> toSpan, int toIndex, int count)
    {
        for(int i = 0; i < count; i++)
            toSpan[toIndex+i] = fromList[fromIndex+i];
    }
}
