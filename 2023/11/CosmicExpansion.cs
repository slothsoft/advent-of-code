using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/11">Day 11: Cosmic Expansion</a>: Analyze your OASIS report and extrapolate the next value for each history.
/// What is the sum of these extrapolated values?
/// </summary>
public class CosmicExpansion {
    public record GalaxyLocation(long X, long Y) {
        public long X { get; set; } = X;
        public long Y { get; set; } = Y;

        public long CalculateDistanceTo(GalaxyLocation location) {
            return Math.Abs(X - location.X) + Math.Abs(Y - location.Y);
        }
    }

    public readonly IList<GalaxyLocation> galaxyLocations;
    private readonly int _cosmosWidth;
    private readonly int _cosmosHeight;

    public CosmicExpansion(IEnumerable<string> input) {
        (galaxyLocations, _cosmosWidth, _cosmosHeight) = ParseGalaxyLocations(input);
    }

    private static (IList<GalaxyLocation>, int, int) ParseGalaxyLocations(IEnumerable<string> input) {
        var result = new List<GalaxyLocation>();
        var cosmos = input.ParseCharMatrix();

        for (var x = 0; x < cosmos.Length; x++) {
            for (var y = 0; y < cosmos[x].Length; y++) {
                if (cosmos[x][y] == '#') {
                    result.Add(new GalaxyLocation(x, y));
                }
            }
        }

        return (result, cosmos.Length, cosmos[0].Length);
    }

    public void ExpandUniverse(int expansion = 1) {
        // expand horizontally
        for (var x = _cosmosWidth - 1; x >= 0; x--) {
            if (galaxyLocations.All(g => g.X != x)) {
                // empty row => expand!
                foreach (var galaxyLocation in galaxyLocations) {
                    if (galaxyLocation.X > x) {
                        galaxyLocation.X += expansion;
                    }
                }
            }
        }

        // expand vertically
        for (var y = _cosmosHeight - 1; y >= 0; y--) {
            if (galaxyLocations.All(g => g.Y != y)) {
                // empty column => expand!
                foreach (var galaxyLocation in galaxyLocations) {
                    if (galaxyLocation.Y > y) {
                        galaxyLocation.Y += expansion;
                    }
                }
            }
        }
    }

    public long CalculateShortestDistanceSum() {
        long result = 0;
        foreach (var galaxyLocation in galaxyLocations) {
            foreach (var other in galaxyLocations) {
                result += galaxyLocation.CalculateDistanceTo(other);
            }
        }

        return result / 2;
    }
}