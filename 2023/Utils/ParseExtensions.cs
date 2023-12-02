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
}