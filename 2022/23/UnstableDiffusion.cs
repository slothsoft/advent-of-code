using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._23;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/23">Day 23: Unstable Diffusion</a>
/// </summary>
public class UnstableDiffusion {
    internal readonly struct Point {
        internal int X { get; }
        internal int Y { get; }

        public Point(int x, int y) : this() {
            X = x;
            Y = y;
        }
    }

    internal record Elf(Point Location) {
        internal Point Location { get; set; } = Location;
    }

    internal enum Direction {
        North,
        South,
        West,
        East,
    }

    private readonly IList<Elf> _elves;
    private readonly IList<Direction> _directionsToCheck;

    private IList<Point>? _elvesLocations;

    public UnstableDiffusion(string[] lines) {
        _elves = new List<Elf>();
        for (var y = 0; y < lines.Length; y++) {
            for (var x = 0; x < lines[y].Length; x++) {
                if (lines[y][x] == '#') {
                    _elves.Add(new Elf(new Point(x, y)));
                }
            }
        }

        _directionsToCheck = new List<Direction> {
            Direction.North, Direction.South, Direction.West, Direction.East,
        };
    }

    public int CalculateEmptyGroundAfterRounds(int rounds) {
        ExecuteRounds(rounds);

        return ToString().Count(c => c == '.');
    }

    internal void ExecuteRounds(int rounds) {
        for (var i = 0; i < rounds; i++) {
            ExecuteRound();
        }
    }

    internal int ExecuteRound() {
        _elvesLocations = _elves.Select(e => e.Location).ToList();

        // find a valid direction per elf
        var proposedDirections = new Dictionary<Elf, Point>();
        foreach (var elf in _elves) {
            var proposedDirection = ExecuteElf(elf);
            if (proposedDirection != null) {
                proposedDirections.Add(elf, proposedDirection.Value);
            }
        }

        // flip map to remove duplicates
        var cannotMove = proposedDirections
            .GroupBy(e => e.Value)
            .Where(g => g.Count() > 1)
            .SelectMany(g => g)
            .Select(e => e.Key);
        foreach (var elf in cannotMove) {
            proposedDirections.Remove(elf);
        }

        // now we should be able to move the remaining elves
        foreach (var validMove in proposedDirections) {
            validMove.Key.Location = validMove.Value;
        }

        // change directions
        var firstDirection = _directionsToCheck[0];
        _directionsToCheck.Remove(firstDirection);
        _directionsToCheck.Add(firstDirection);

        return proposedDirections.Count;
    }

    private Point? ExecuteElf(Elf elf) {
        // lol

        if (IsDirectionValid(elf.Location, Direction.North) && IsDirectionValid(elf.Location, Direction.South) &&
            IsDirectionValid(elf.Location, Direction.East) && IsDirectionValid(elf.Location, Direction.West)) {
            // this elf is on his own, and he likes it
            return null;
        }

        foreach (var direction in _directionsToCheck) {
            if (IsDirectionValid(elf.Location, direction)) {
                return new Point(elf.Location.X + direction.GetXPlus(), elf.Location.Y + direction.GetYPlus());
            }
        }

        return null;
    }

    private bool IsDirectionValid(Point location, Direction direction) {
        foreach (var x in direction.GetCheckX()) {
            foreach (var y in direction.GetCheckY()) {
                if (IsElfAtPosition(location.X + x, location.Y + y)) {
                    return false;
                }
            }
        }

        return true;
    }

    private bool IsElfAtPosition(int x, int y) {
        var position = new Point(x, y);
        return _elvesLocations!.Any(l => Equals(l, position));
    }

    public override string ToString() {
        _elvesLocations = _elves.Select(e => e.Location).ToArray();
        var minX = _elvesLocations.Min(l => l.X);
        var maxX = _elvesLocations.Max(l => l.X);
        var minY = _elvesLocations.Min(l => l.Y);
        var maxY = _elvesLocations.Max(l => l.Y);

        var result = "";
        for (var y = minY; y <= maxY; y++) {
            for (var x = minX; x <= maxX; x++) {
                if (IsElfAtPosition(x, y)) {
                    result += '#';
                } else {
                    result += '.';
                }
            }

            result += "\r\n";
        }

        return result;
    }

    public int CalculateRoundWhereNoElfMoved() {
        var result = 1;
        while (ExecuteRound() > 0) {
            result++;
        }

        return result;
    }
}

internal static class DirectionExtensions {
    private static readonly int[] MinusOneToOne = {-1, 0, 1};
    private static readonly int[] MinusOne = {-1};
    private static readonly int[] One = {1};

    public static int[] GetCheckX(this UnstableDiffusion.Direction direction) {
        return direction switch {
            UnstableDiffusion.Direction.North => MinusOneToOne,
            UnstableDiffusion.Direction.South => MinusOneToOne,
            UnstableDiffusion.Direction.West => MinusOne,
            UnstableDiffusion.Direction.East => One,
            _ => throw new ArgumentOutOfRangeException("Do not know Direction " + direction)
        };
    }

    public static int[] GetCheckY(this UnstableDiffusion.Direction direction) {
        return direction switch {
            UnstableDiffusion.Direction.North => MinusOne,
            UnstableDiffusion.Direction.South => One,
            UnstableDiffusion.Direction.West => MinusOneToOne,
            UnstableDiffusion.Direction.East => MinusOneToOne,
            _ => throw new ArgumentOutOfRangeException("Do not know Direction " + direction)
        };
    }

    public static int GetXPlus(this UnstableDiffusion.Direction direction) {
        return direction switch {
            UnstableDiffusion.Direction.North => 0,
            UnstableDiffusion.Direction.South => 0,
            UnstableDiffusion.Direction.West => -1,
            UnstableDiffusion.Direction.East => +1,
            _ => throw new ArgumentOutOfRangeException("Do not know Direction " + direction)
        };
    }

    public static int GetYPlus(this UnstableDiffusion.Direction direction) {
        return direction switch {
            UnstableDiffusion.Direction.North => -1,
            UnstableDiffusion.Direction.South => +1,
            UnstableDiffusion.Direction.West => 0,
            UnstableDiffusion.Direction.East => 0,
            _ => throw new ArgumentOutOfRangeException("Do not know Direction " + direction)
        };
    }
}