using System;
using System.Collections.Generic;
using System.Linq;
using AoC.day25;

namespace AoC.day22;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/22">Day 22: Sand Slabs</a>
/// </summary>
public class SandSlabs {
    private static int NextSlabId;

    public record Slab(Point StartPoint, Point EndPoint) {
        public int Id { get; set; } = NextSlabId++;
        public Point StartPoint { get; set; } = StartPoint;
        public Point EndPoint { get; set; } = EndPoint;

        public bool Intersects(Slab otherSlab) {
            return Intersection(otherSlab) != null;
        }

        public Slab? Intersection(Slab other) {
            var start = new int[coordinates.Length];
            var end = new int[coordinates.Length];
            for (var i = 0; i < start.Length; i++) {
                start[i] = Math.Max(Math.Min(StartPoint.values[i], EndPoint.values[i]), Math.Min(other.StartPoint.values[i], other.EndPoint.values[i]));
                end[i] = Math.Min(Math.Max(StartPoint.values[i], EndPoint.values[i]), Math.Max(other.StartPoint.values[i], other.EndPoint.values[i]));
                if (start[i] > end[i]) {
                    return null;
                }
            }

            return new Slab(new Point(start), new Point(end));
        }

        public bool EqualsPoints(Slab other) => StartPoint.Equals(other.StartPoint) && EndPoint.Equals(other.EndPoint);

        public override string ToString() => $"Slab({StartPoint} -> {EndPoint})";

        public Slab Copy() {
            return new Slab(StartPoint.Copy(), EndPoint.Copy()) {Id = Id,};
        }
    }

    public readonly struct Point {
        public Point() {
            values = new int[coordinates.Length];
        }

        public Point(int[] newValues) {
            values = newValues;
        }

        public Point(int x, int y, int z) {
            values = new[] {x, y, z};
        }

        public readonly int[] values;
        public int X { get => values[SandSlabs.X]; set => values[SandSlabs.X] = value; }
        public int Y { get => values[SandSlabs.Y]; set => values[SandSlabs.Y] = value; }
        public int Z { get => values[SandSlabs.Z]; set => values[SandSlabs.Z] = value; }

        public Point With(int coordinate, int value) {
            return new Point(X, Y, Z) {values = {[coordinate] = value}};
        }

        public override string ToString() => $"{X}|{Y}|{Z}";

        public bool Equals(Point other) => values.SequenceEqual(other.values);

        public override bool Equals(object? obj) => obj is Point other && Equals(other);

        public override int GetHashCode() => values.GetHashCode();

        public Point Copy() {
            return new Point(X, Y, Z);
        }
    }

    private const int X = 0;
    private const int Y = 1;
    private const int Z = 2;
    internal static readonly int[] coordinates = {X, Y, Z};

    public SandSlabs(IEnumerable<string> input) {
        Slabs = input.Select(ParseSlab).ToArray();
    }

    private SandSlabs(IEnumerable<Slab> input) {
        Slabs = input.ToArray();
    }

    internal Slab[] Slabs { get; }

    private static Slab ParseSlab(string input) {
        var split = input.Split('~');
        return new Slab(ParsePoint(split[0]), ParsePoint(split[1]));
    }

    private static Point ParsePoint(string input) {
        var split = input.Split(',');
        return new Point(split[0].ExtractDigitsAsInt(), split[1].ExtractDigitsAsInt(), split[2].ExtractDigitsAsInt());
    }

    public ISet<int> FallDown() {
        var result = new HashSet<int>();
        bool somethingFellDown;
        do {
            var fallenSlabIds =  TryFallDown();
            somethingFellDown = fallenSlabIds.Any();
            result.AddRange(fallenSlabIds);
        } while (somethingFellDown);

        return result;
    }

    private ISet<int> TryFallDown() {
        var result = new HashSet<int>();
        foreach (var slab in Slabs.OrderBy(s => s.StartPoint.values[Z])) {
            var fallenSlab = CreateFallenSlab(slab);

            if (fallenSlab.StartPoint.Z <= 0) {
                // now the slab is in the floor, so don't allow it
                continue;
            }

            if (!FetchIntersectingSlabs(fallenSlab).Any()) {
                // so there is space below the slab, so move it (I like to move it move it)
                slab.StartPoint = slab.StartPoint with {Z = slab.StartPoint.Z - 1,};
                slab.EndPoint = slab.EndPoint with {Z = slab.EndPoint.Z - 1,};
                result.Add(slab.Id);
            }
        }

        return result;
    }

    private static Slab CreateFallenSlab(Slab slab) {
        var minZ = Math.Min(slab.StartPoint.Z, slab.EndPoint.Z) - 1;
        return new Slab(slab.StartPoint.With(Z, minZ), slab.EndPoint.With(Z, minZ)) {Id = slab.Id,};
    }

    private IEnumerable<Slab> FetchIntersectingSlabs(Slab fallenSlab) {
        return Slabs.Where(fallenSlab.Intersects);
    }

    public IEnumerable<Slab> CalculateDisintegrateableSlabs() {
        FallDown();
        return Slabs.Where(IsDisintegrateable);
    }

    private bool IsDisintegrateable(Slab slab) {
        // these slabs are stacked on the incoming one
        var stackedSlabs = FetchIntersectingSlabs(CreateFlyingSlab(slab)).ToArray();
        foreach (var stackedSlab in stackedSlabs) {
            // if the stacked slab has another slab to lean on, everything is okay
            if (FetchIntersectingSlabs(CreateFallenSlab(stackedSlab)).All(s => s.Id == slab.Id)) {
                return false; // if not we can't disintegrate
            }
        }

        return true;
    }

    private static Slab CreateFlyingSlab(Slab slab) {
        var maxZ = Math.Max(slab.StartPoint.Z, slab.EndPoint.Z) + 1;
        return new Slab(slab.StartPoint.With(Z, maxZ), slab.EndPoint.With(Z, maxZ)) {Id = slab.Id,};
    }

    public int CalculateEntireFallenBricks() {
        FallDown();
        var result = Slabs.OrderByDescending(s => s.StartPoint.values[Z]).ToDictionary(
            slab => slab,
            slab => {
                var sandSlabs = new SandSlabs(Slabs.Where(s => s.Id != slab.Id).Select(s => s.Copy()).ToList());
                return sandSlabs.TryFallDown().Count;
            });

        return result.Values.Sum();
    }
}