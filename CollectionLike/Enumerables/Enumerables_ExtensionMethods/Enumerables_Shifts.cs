using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace CollectionLike.Enumerables;

public static partial class Enumerables_Extension
{
    public static string ItemsToString<T>(this IEnumerable<T> items, string separator = ",", string emptyValue = "NullOrEmpty") => 
        items.HasElements() ? String.Join(separator, items.Select(p => p.ToString()).ToArray()) : emptyValue;
    public static IEnumerable<T> CyclicRightShift<T>(this IEnumerable<T> items) => items.HasElements() ? items.SkipLast(1).Prepend(items.Last()) : items;
    public static IEnumerable<T> CyclicLeftShift<T>(this IEnumerable<T> items) => items.HasElements() ? items.Skip(1).Append(items.First()) : items;

    //TODO rename - better name RotateLeft
    public static T[] RotateLeft<T>(this T[] items, int numberOfPositions)
    {
        if (items == null)
            throw new ArgumentNullException($"{nameof(items)} cannot be null");
        if (items.Length == 0)
            throw new ArgumentNullException($"items.Length cannot be 0");
        if (numberOfPositions <0)
            throw new ArgumentNullException($"numberOfPositions cannot be negative");
        Contract.EndContractBlock();

        if (numberOfPositions >= items.Length)
            numberOfPositions = numberOfPositions % items.Length;
        if (numberOfPositions == 0)
            return items;
        return items.Skip(numberOfPositions).Concat(items.Take(numberOfPositions)).ToArray();
    }

    public static void RotateLeftInplace(this List<int> items, int places)
    {
        // Handle cases where the number of places is greater than or equal to the number of items:
        places %= items.Count;
        if (places == 0) return;
        int[] temp = new int[places];
        items.CopyTo(0, temp, 0, places);
        items.RemoveRange(0, places);
        items.AddRange(temp);
    }
}
