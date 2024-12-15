using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AoC.day12;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/12">Day 12: Garden Groups</a>
/// </summary>
public class GardenGroups {
    internal record Region(char plantType, IList<(int X, int Y)>  includedCoords) {
        public char PlantType { get; } = plantType;
        public IList<(int X, int Y)> IncludedCoords { get; } = includedCoords;

        public long CalculateArea() => IncludedCoords.Count;
        
        public long CalculatePerimeter() {
            var result = 0L;
            foreach (var includedCoord in IncludedCoords) {
                foreach (var direction in AllDirections) {
                    var neighboringPoint = direction.Modify(includedCoord.X, includedCoord.Y);
                    if (!IncludedCoords.Contains(neighboringPoint)) {
                        result++;
                    }
                }
            }

            return result;
        }
    }
    
    internal struct Direction(string name, Func<int, int> modifyX, Func<int, int> modifyY) {
        public (int X, int Y) Modify(int oldX, int oldY) => (
            modifyX(oldX), 
            modifyY(oldY)
        );
        public override string ToString() => name;
    }

    private static Direction[] AllDirections { get; } = [
            new Direction("North", x => x, y => y - 1),
            new Direction("South", x => x, y => y + 1),
            new Direction("West", x => x - 1, y => y),
            new Direction("East", x => x + 1, y => y),
        ];
    
    public GardenGroups(IEnumerable<string> input) {
        Input = input.ParseCharMatrix();
    }

    internal char[][] Input { get; }
    internal Region[]? Regions { get; private set; }

    public long CalculateFencingPrice() {
        Regions ??= FetchRegions(Input);
        return Regions.Sum(r => r.CalculateArea() * r.CalculatePerimeter());
    }
    
    internal static Region[] FetchRegions(char[][] map) {
        var result = new List<Region>();
        var alreadyChecked = new List<(int X, int Y)>();
        
        for (var x = 0; x < map.Length; x++) {
            for (var y = 0; y < map[x].Length; y++) {
                if (!alreadyChecked.Contains((x, y))) {
                    var regionPoints = new List<(int X, int Y)>();
                    CollectRegionPoints(map, x, y, regionPoints);
                    alreadyChecked.AddRange(regionPoints);
                    result.Add(new Region(map[x][y], regionPoints));
                }
            }
        }

        return result.ToArray();
    }
    
    private static void CollectRegionPoints(char[][] map, int startX, int startY, List<(int X, int Y)> regionPoints) {
        var collectedRegion = map[startX][startY];
        
        regionPoints.Add((startX, startY));
        foreach (var direction in AllDirections) {
            var newPoint = direction.Modify(startX, startY);
            if (map.IsPointOnMap(newPoint.X, newPoint.Y) && map[newPoint.X][newPoint.Y] == collectedRegion && !regionPoints.Contains(newPoint)) { 
                CollectRegionPoints(map, newPoint.X, newPoint.Y, regionPoints);
            }
        }
    }
    
    public long CalculateSidePrice() {
        // a) we could calculate the perimeter and join the parts next to each other?
        // b) the circumference is side-corner-side-corner-..., so maybe calculating the corner is easier?
        Regions ??= FetchRegions(Input);
        return Regions.Sum(r => r.CalculateArea() * r.CalculatePerimeter());
    }
}

public static class GardenGroupsExtensions {
    // TODO: these could be common
    public static bool IsPointOnMap<TTile>(this TTile[][] map, int x, int y) => IsCoordOnMap(x, map.Length) && IsCoordOnMap(y, map[0].Length);
    private static bool IsCoordOnMap(int coord, int length) => coord >= 0 && coord < length;
}