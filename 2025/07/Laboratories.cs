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
        CalculateBeamsOnManifold(_ => beamSplitting++);
        return beamSplitting;
    }
    
    public long CalculateTimelines() {
        return CalculateBeamsOnManifold().Sum(xCount => xCount.Value);
    }
    
    private IDictionary<int, long> CalculateBeamsOnManifold(Action<int>? onBeamSplitting = null) {
        onBeamSplitting ??= _ => { };
        var beamXAndCounts = new Dictionary<int, long> { {StartIndex, 1} };

        for (var y = 1; y < Manifold[0].Length; y++) {
            foreach (var (x, count) in beamXAndCounts.ToArray()) {
                switch (Manifold[x][y]) {
                    case '^': {
                        // split the beam
                        onBeamSplitting(x);
                        beamXAndCounts.Remove(x);
                        beamXAndCounts.AddBeam(x - 1, count);
                        beamXAndCounts.AddBeam(x + 1, count);
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
        
        return beamXAndCounts;
    }
}

public static class LaboratoriesExtensions {
    public static void AddBeam(this IDictionary<int, long> value, int x, long count) {
        if (!value.TryAdd(x, count)) {
            value[x] += count;
        }
    }
}
