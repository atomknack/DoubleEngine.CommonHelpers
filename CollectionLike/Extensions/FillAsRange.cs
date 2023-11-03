using System;
using CollectionLike.Pooled;
using Collections.Pooled;

//#nullable enable
namespace CollectionLike;

public static partial class CollectionLike_Extensions
{
    public static PooledArrayStruct<int> FillAsRange(this PooledArrayStruct<int> toFill, int startValue = 0)
    {
        toFill.AsSpan().FillAsRange(startValue);
        return toFill;
    }
    public static int[] FillAsRange(this int[] toFill, int startValue = 0)
    {
        toFill.AsSpan().FillAsRange(startValue);
        return toFill;
    }

    public static Span<int> FillAsRange(this Span<int> toFill, int startValue = 0)
    {
        for (int i = 0; i < toFill.Length; i++)
            toFill[i] = i + startValue;
        return toFill;
    }
}