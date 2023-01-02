using System.Collections.Generic;
using System.Linq;

namespace AoC._16;

public static class Permutation {
    public static IEnumerable<string> Permute(this string str) {
        return Permute(str.ToCharArray()).Select(c => new string(c));
    }

    public static IEnumerable<TElement[]> Permute<TElement>(this TElement[] elements) {
        return Permute(elements, 0, elements.Length - 1);
    }

    private static IEnumerable<TElement[]> Permute<TElement>(this TElement[] elements, int left, int right) {
        if (left == right)
            yield return elements;
        else {
            for (var i = left; i <= right; i++) {
                elements = SwapElements(elements, left, i);
                foreach (var s in Permute(elements, left + 1, right)) {
                    yield return s;
                }
                elements = SwapElements(elements, left, i);
            }
        }
    }

    private static TElement[] SwapElements<TElement>(TElement[] elements,
        int index1, int index2) {
        var temp = elements[index1];
        var copy = (TElement[]) elements.Clone();
        copy[index1] = copy[index2];
        copy[index2] = temp;
        return copy;
    }
}