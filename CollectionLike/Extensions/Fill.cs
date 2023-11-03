using Collections.Pooled;
using System;

namespace CollectionLike.Pooled;

public static partial class CollectionLike_Extensions //Fill
{
    public static PooledArrayStruct<T> Fill<T>(this PooledArrayStruct<T> toFill, T value)
    {
        toFill.AsSpan().Fill(value);
        return toFill;
    }
    public static T[] Fill<T>(this T[] toFill, T value)
    {
        toFill.AsSpan().Fill(value);
        return toFill;
    }
    /*
public static Span<T> Fill<T>(this Span<T> toFill, T value)
{
for (int i = 0; i < toFill.Length; ++i)
    toFill[i] = value;
return toFill;
}

public static void Fill<T>(this PooledArrayStruct<T> @this, T value)
{
for(int i = 0; i < @this.Count; ++i)
    @this[i] = value;
}*/

}
