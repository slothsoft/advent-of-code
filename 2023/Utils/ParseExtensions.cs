using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC;

public static class ParseExtensions {
    /// <summary>
    /// Ignores everything else in the input string except for digits 0 to 9. Returns these as an int.
    /// </summary>
    /// <param name="input">string, e.g. "tr3buche7"</param>
    /// <returns>result, e.g. 37</returns>
    
    public static int ExtractDigitsAsInt(this string input) {
        return int.Parse(input.ExtractDigitsAsString());
    }
    
    /// <summary>
    /// Ignores everything else in the input string except for digits 0 to 9. Returns these as an int.
    /// </summary>
    /// <param name="input">string, e.g. "tr3buche7"</param>
    /// <returns>result, e.g. "37"</returns>
    
    public static string ExtractDigitsAsString(this string input) {
        return Regex.Replace(input, "[^0-9]+", "");
    }
    
    /// <summary>
    /// Parses an input of strings into a matrix of bools.
    /// </summary>
    /// <param name="input">the input strings</param>
    /// <param name="trueChar">char for true</param>
    /// <param name="falseChar">char for false</param>
    /// <returns>a matrix with X as the first coordinate and y as second</returns>
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
    /// Parses an input of strings into a matrix of chars.
    /// </summary>
    /// <param name="input">the input strings</param>
    /// <returns>a matrix with X as the first coordinate and y as second</returns>
    public static char[][] ParseCharMatrix(this IEnumerable<string> input) {
        return input.ParseMatrix(c => c);
    }

    /// <summary>
    /// Parses an input of strings into a matrix of values.
    /// </summary>
    /// <param name="input">the input strings</param>
    /// <param name="singleCellParser">parser for single cell</param>
    /// <returns>a  with X as the first coordinate and y as second</returns>
    public static TResult[][] ParseMatrix<TResult>(this IEnumerable<string> input, Func<char, TResult> singleCellParser) {
        List<TResult>?[]? result = null;

        foreach (var line in input) {
            result ??= new List<TResult>[line.Length];
            for (var x = 0; x < line.Length; x++) {
                result[x] ??= new List<TResult>();
                result[x]!.Add(singleCellParser(line[x]));
            }
        }

        return result?.Select(r => r!.ToArray()).ToArray() ?? Array.Empty<TResult[]>();
    }
}