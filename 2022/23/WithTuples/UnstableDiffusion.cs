using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._23.WithTuples;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/23">Day 23: Unstable Diffusion</a>
/// </summary>
public class UnstableDiffusion {

    internal enum Direction {
        North,
        South,
        West,
        East,
    }

    private readonly HashSet<(int x, int y)> _elves = new();
    private readonly IList<Direction> _directionsToCheck;

    public UnstableDiffusion(string[] lines) {
        for (var y = 0; y < lines.Length; y++) {
            for (var x = 0; x < lines[y].Length; x++) {
                if (lines[y][x] == '#') {
                    _elves.Add((x, y));
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
        // find a valid direction per elf
        var proposedDirections = new Dictionary<(int x, int y), (int x, int y)>();
        var elvesThatMustNotMove = new HashSet<(int x, int y)>();
        foreach (var elf in _elves) {
            if (TryFindMoveForElf(elf, out var proposedMove)) {
                // if 2 elves want to move to the same spot, don't move either
                if (proposedDirections.TryGetValue(proposedMove, out var otherElf)) {
                    elvesThatMustNotMove.Add(elf);
                    elvesThatMustNotMove.Add(otherElf);
                } else {
                    proposedDirections[proposedMove] = elf;
                }
            }
        }

        // first, remove all elves that will move
        foreach (var (_, elf) in proposedDirections) {
            if (!elvesThatMustNotMove.Contains(elf)) {
                _elves.Remove(elf);
            }
        }

        // now, re-insert them into the set
        foreach (var (target, elf) in proposedDirections) {
            if (!elvesThatMustNotMove.Contains(elf)) {
                _elves.Add(target);
            }
        }

        var elvesThatMovedCount = proposedDirections.Count - elvesThatMustNotMove.Count;

        // change directions
        var firstDirection = _directionsToCheck[0];
        _directionsToCheck.Remove(firstDirection);
        _directionsToCheck.Add(firstDirection);

        return elvesThatMovedCount;
    }

    private bool TryFindMoveForElf((int x, int y) elf, out (int x, int y) proposedMove) {
        // lol
        proposedMove = default;

        if (IsDirectionValid(elf, Direction.North) && IsDirectionValid(elf, Direction.South) &&
            IsDirectionValid(elf, Direction.East) && IsDirectionValid(elf, Direction.West)) {
            // this elf is on his own, and he likes it
            return false;
        }

        foreach (var direction in _directionsToCheck) {
            if (IsDirectionValid(elf, direction)) {
                proposedMove = (elf.x + direction.GetXPlus(), elf.y + direction.GetYPlus());
                return true;
            }
        }

        return false;
    }

    private bool IsDirectionValid((int x, int y) location, Direction direction) {
        foreach (var x in direction.GetCheckX()) {
            foreach (var y in direction.GetCheckY()) {
                if (IsElfAtPosition(location.x + x, location.y + y)) {
                    return false;
                }
            }
        }

        return true;
    }

    private bool IsElfAtPosition(int x, int y) {
        return _elves.Contains(new(x, y));
    }

    public override string ToString() {
        var minX = _elves.Min(l => l.x);
        var maxX = _elves.Max(l => l.x);
        var minY = _elves.Min(l => l.y);
        var maxY = _elves.Max(l => l.y);

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
    private static readonly int[] MinusOneToOne = { -1, 0, 1 };
    private static readonly int[] MinusOne = { -1 };
    private static readonly int[] One = { 1 };

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