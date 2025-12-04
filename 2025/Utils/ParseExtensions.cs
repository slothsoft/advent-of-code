using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// <returns>result, e.g. 37</returns>
    
    public static long ExtractDigitsAsLong(this string input) {
        return long.Parse(input.ExtractDigitsAsString());
    }
    
    /// <summary>
    /// Ignores everything else in the input string except for digits 0 to 9. Returns these as an int.
    /// </summary>
    /// <param name="input">string, e.g. "tr3buche7"</param>
    /// <returns>result, e.g. "37"</returns>
    
    public static string ExtractDigitsAsString(this string input) {
        return Regex.Replace(input, "[^-0-9]+", "");
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
    /// Parses an input of strings into a matrix of ints.
    /// </summary>
    /// <param name="input">the input strings</param>
    /// <returns>a matrix with X as the first coordinate and y as second</returns>
    public static int[][] ParseIntMatrix(this IEnumerable<string> input) {
        return input.ParseMatrix(c => int.Parse(c.ToString()));
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
        return input.ParseMatrix((x, y, c) => singleCellParser(c));
    }
    
    /// <summary>
    /// Parses an input of strings into a matrix of values.
    /// </summary>
    /// <param name="input">the input strings</param>
    /// <param name="singleCellParser">parser for single cell</param>
    /// <returns>a  with X as the first coordinate and y as second</returns>
    public static TResult[][] ParseMatrix<TResult>(this IEnumerable<string> input, Func<int, int, char, TResult> singleCellParser) {
        List<TResult>?[]? result = null;
        var y = 0;
        
        foreach (var line in input) {
            result ??= new List<TResult>[line.Length];
            for (var x = 0; x < line.Length; x++) {
                result[x] ??= [];
                result[x]!.Add(singleCellParser(x, y, line[x]));
            }
            y++;
        }

        return result?.Select(r => r!.ToArray()).ToArray() ?? [];
    }

    public static string StringifyMatrix<TElement>(this TElement[][] matrix) => matrix.StringifyMatrix(e => e?.ToString()?[0] ?? '❓');

    public static string StringifyMatrix<TElement>(this TElement[][] matrix, Func<TElement, char> toString) {
        return matrix.StringifyMatrix((x, y, e) => toString(e));
    }
    
    public static string StringifyMatrix<TElement>(this TElement[][] matrix, Func<int, int, TElement, char> toString) {
        var result = new StringBuilder();
        for (var y = 0; y < matrix[0].Length; y++) {
            for (var x = 0; x < matrix.Length; x++) {
                result.Append(toString(x, y, matrix[x][y]));
            }
            result.Append(Environment.NewLine);
        }
        return result.ToString().TrimEnd();
    }

    /// <summary>
    /// Parses an input of strings into a matrix of values.
    /// </summary>
    /// <param name="input">the input string</param>
    /// <param name="separator">what separates the ints</param>
    /// <returns>a  with X as the first coordinate and y as second</returns>
    public static int[] ParseIntArray(this string input, char separator = ' ') {
        if (input.Length == 0) {
            return [];
        }
        
        return input.Trim().Split(separator).Select(ExtractDigitsAsInt).ToArray();
    }
    
    /// <summary>
    /// Parses an input of strings into a matrix of values.
    /// </summary>
    /// <param name="input">the input string</param>
    /// <param name="separator">what separates the ints</param>
    /// <returns>a  with X as the first coordinate and y as second</returns>
    public static long[] ParseLongArray(this string input, char separator = ' ') {
        if (input.Length == 0) {
            return [];
        }
        
        return input.Trim().Split(separator).Select(ExtractDigitsAsLong).ToArray();
    }
}