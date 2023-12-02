using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC;

internal static class ParseExtensions {
    /// <summary>
    /// Parses an input of strings into a matrix of bools.
    /// </summary>
    /// <param name="input">the input strings</param>
    /// <param name="trueChar">char for true</param>
    /// <param name="falseChar">char for false</param>
    /// <returns>a  with X as the first coordinate and y as second</returns>
    public static bool[][] ParseBoolMatrix(this IEnumerable<string> input, char trueChar, char falseChar) {
        return input.ParseMatrix(c => {
            if (c == trueChar) {
                return true;
            }

            if (c == falseChar) {
                return false;
            }

            throw new ArgumentException("Cannot parse: " + c);
        });
    }

    /// <summary>
    /// Parses an input of strings into a matrix of values.
    /// </summary>
    /// <param name="input">the input strings</param>
    /// <param name="singleCellParser">parser for single cell</param>
    /// <returns>a  with X as the first coordinate and y as second</returns>
    public static TResult[][] ParseMatrix<TResult>(this IEnumerable<string> input, Func<char, TResult> singleCellParser) {
        List<TResult>[]? result = null;

        foreach (var line in input) {
            result ??= new List<TResult>[line.Length];
            for (var x = 0; x < line.Length; x++) {
                result[x] ??= new List<TResult>();
                result[x].Add(singleCellParser(line[x]));
            }
        }

        return result?.Select(r => r.ToArray()).ToArray() ?? Array.Empty<TResult[]>();
    }
}