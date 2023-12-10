using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/9">Day 9: Mirage Maintenance</a>: Analyze your OASIS report and extrapolate the next value for each history.
/// What is the sum of these extrapolated values?
/// </summary>
public class MirageMaintenance {

    private readonly IList<long[]> _inputArrays;
    
    public MirageMaintenance(IEnumerable<string> input) {
        _inputArrays = input.Select(i => i.ParseLongArray()).ToList();
    }

    public long SumExtrapolatedValues() {
        return _inputArrays.Select(Extrapolate).Sum();
    }

    public static long Extrapolate(long[] input) {
        // we are in the last line of the parallelogram
        if (input.All(d => d == 0L)) {
            return 0;
        }
        
        var differences = new long[input.Length - 1];
        for (var i = 0; i < input.Length -1; i++) {
            differences[i] = input[i + 1] - input[i];
        }
        
        return input[^1] + Extrapolate(differences);
    }
    
    public long SumRevextrapolatedValues() {
        return _inputArrays.Select(Revextrapolate).Sum();
    }
    
    public static long Revextrapolate(long[] input) {
        // we are in the last line of the parallelogram
        if (input.All(d => d == 0L)) {
            return 0;
        }
        
        var differences = new long[input.Length - 1];
        for (var i = 0; i < input.Length -1; i++) {
            differences[i] = input[i + 1] - input[i];
        }
        
        return input[0] - Revextrapolate(differences);
    }
}