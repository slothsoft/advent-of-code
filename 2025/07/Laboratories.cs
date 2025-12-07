using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day7;

/// <summary>
/// <a href="https://adventofcode.com/2025/day/7">Day 7: Laboratories</a>
/// </summary>
public class Laboratories {
    public Laboratories(IEnumerable<string> input) {
        var inputAsArray = input.ToArray();
        Manifold = inputAsArray.ParseCharMatrix();
        StartIndex = inputAsArray[0].IndexOf('S');
    }

    internal char[][] Manifold { get; }
    internal int StartIndex { get; }

    public long CalculateBeamSplitting() {
        var beamSplitting = 0L;
        var beamXs = new HashSet<int> { StartIndex };

        for (var y = 1; y < Manifold[0].Length; y++) {
            foreach (var x in beamXs.ToArray()) {
                switch (Manifold[x][y]) {
                    case '^': {
                        // split the beam
                        beamXs.Remove(x);
                        beamXs.Add(x - 1);
                        beamXs.Add(x + 1);
                        beamSplitting++;
                        break;
                    }
                    case '.': {
                        // nothing happens to the beam, x stays the same
                        break;
                    }
                        default: throw new Exception($"Do not know how to handle field {Manifold[x][y]} ({x} | {y})");
                    
                }
            }
        }
        
        return beamSplitting;
    }
}
