using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/16">Day 16: The Floor Will Be Lava</a>:  With the beam starting in the top-left heading right,
/// how many tiles end up being energized?
/// </summary>
public class TheFloorWillBeLava {
    public interface ITile {
        static ITile CreateTile(char tileChar) {
            switch (tileChar) {
                case '.':
                    return FloorTile.instance;
                case '\\':
                    return Mirror.topLeftToBottomRight;
                case '/':
                    return Mirror.bottomLeftToTopRight;
                case '-':
                    return Splitter.leftToRight;
                case '|':
                    return Splitter.topToBottom;
                default:
                    throw new ArgumentException($"Do not know tile {tileChar}");
            }
        }

        void Handle(ContraptionContext context, RayOfLight rayOfLight);
    }

    public class FloorTile : ITile {
        public static readonly FloorTile instance = new();

        public void Handle(ContraptionContext context, RayOfLight rayOfLight) {
            rayOfLight.X += rayOfLight.Direction.XPlus;
            rayOfLight.Y += rayOfLight.Direction.YPlus;
        }
    }

    public class Mirror : ITile {
        public static readonly Mirror topLeftToBottomRight = new(Direction.North, Direction.West); // \
        public static readonly Mirror bottomLeftToTopRight = new(Direction.North, Direction.East); // /

        private readonly IDictionary<Direction, Direction> _translation;

        public Mirror(Direction exampleFrom, Direction exampleTo) {
            _translation = new Dictionary<Direction, Direction> {
                {exampleFrom, exampleTo},
                {exampleTo, exampleFrom},
                {exampleTo.ToOppositeDirection(), exampleFrom.ToOppositeDirection()},
                {exampleFrom.ToOppositeDirection(), exampleTo.ToOppositeDirection()},
            };
        }

        public void Handle(ContraptionContext context, RayOfLight rayOfLight) {
            rayOfLight.Direction = _translation[rayOfLight.Direction];
            rayOfLight.X += rayOfLight.Direction.XPlus;
            rayOfLight.Y += rayOfLight.Direction.YPlus;
        }
    }

    public class Splitter : ITile {
        public static readonly Splitter topToBottom = new(Direction.North, Direction.South, Direction.East); // \
        public static readonly Splitter leftToRight = new(Direction.West, Direction.East, Direction.North); // /

        private readonly IDictionary<Direction, Direction[]> _translation;

        public Splitter(Direction from, Direction to, Direction exampleForSplitDirection) {
            _translation = new Dictionary<Direction, Direction[]> {
                {from, new[] {from}},
                {to, new[] {to}},
                {exampleForSplitDirection, new[] {from, to}},
                {exampleForSplitDirection.ToOppositeDirection(), new[] {from, to}},
            };
        }

        public void Handle(ContraptionContext context, RayOfLight rayOfLight) {
            var targetDirections = _translation[rayOfLight.Direction];
            var originalX = rayOfLight.X;
            var originalY = rayOfLight.Y;

            rayOfLight.Direction = targetDirections[0];
            rayOfLight.X += rayOfLight.Direction.XPlus;
            rayOfLight.Y += rayOfLight.Direction.YPlus;

            for (var i = 1; i < targetDirections.Length; i++) {
                context.RaysOfLight.Add(new RayOfLight {
                    Direction = targetDirections[i], X = originalX + targetDirections[i].XPlus, Y = originalY + targetDirections[i].YPlus,
                });
            }
        }
    }

    public record RayOfLight {
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Direction { get; set; } = Direction.East;

        public bool IsOutOfBounds(int width, int height) {
            return X < 0 || Y < 0 || X >= width || Y >= height;
        }
    }

    public struct Direction {
        public static Direction North = new("N", 0, -1);
        public static Direction South = new("S", 0, 1);
        public static Direction East = new("E", 1, 0);
        public static Direction West = new("W", -1, 0);

        public Direction(string displayName, int xPlus, int yPlus) {
            DisplayName = displayName;
            XPlus = xPlus;
            YPlus = yPlus;
        }

        public int XPlus { get; }
        public int YPlus { get; }
        public string DisplayName { get; }

        public Direction ToOppositeDirection() {
            if (Equals(North)) {
                return South;
            }

            if (Equals(South)) {
                return North;
            }

            if (Equals(East)) {
                return West;
            }

            if (Equals(West)) {
                return East;
            }

            throw new ArgumentException("Do not know direction " + this);
        }

        public override string ToString() => $"Direction({DisplayName})";
    }

    public record ContraptionContext {
        public ContraptionContext() {
        }

        public ContraptionContext(int x, int y, Direction direction) {
            var rayOfLight = RaysOfLight.Single();
            rayOfLight.X = x;
            rayOfLight.Y = y;
            rayOfLight.Direction = direction;
        }

        public IList<RayOfLight> RaysOfLight { get; } = new List<RayOfLight> {new()};
        public HashSet<string> RaysHistory { get; } = new();
    }

    private ITile[][] _contraption;
    private ContraptionContext _context = new();

    public TheFloorWillBeLava(IEnumerable<string> input) {
        _contraption = input.ParseMatrix(ITile.CreateTile);
    }

    public IEnumerable<RayOfLight> RaysOfLight { get => _context.RaysOfLight; }

    internal void MoveStep() {
        foreach (var rayOfLight in _context.RaysOfLight.ToArray()) {
            MoveStep(rayOfLight);

            if (_contraption.IsOutOfBounds(rayOfLight)) {
                _context.RaysOfLight.Remove(rayOfLight);
            }

            // since some beams go in circles, check the history if we've already been at that point
            var historyString = $"{rayOfLight.X}|{rayOfLight.Y}->{rayOfLight.Direction}";
            if (!_context.RaysHistory.Add(historyString)) {
                _context.RaysOfLight.Remove(rayOfLight);
            }
        }
    }

    internal void MoveStep(RayOfLight rayOfLight) {
        // just to be on the safe side, check that the ray is inside the contraption still
        if (_contraption.IsOutOfBounds(rayOfLight)) {
            return;
        }

        _contraption[rayOfLight.X][rayOfLight.Y].Handle(_context, rayOfLight);
    }

    public long CalculateEnergizedTilesCount() {
        var energizedTiles = new bool[_contraption.Length][];
        for (var i = 0; i < energizedTiles.Length; i++) {
            energizedTiles[i] = new bool[_contraption[i].Length];
        }

        foreach (var rayOfLight in _context.RaysOfLight) {
            energizedTiles[rayOfLight.X][rayOfLight.Y] = true;
        }

        do {
            MoveStep();

            foreach (var rayOfLight in _context.RaysOfLight) {
                if (!_contraption.IsOutOfBounds(rayOfLight)) {
                    energizedTiles[rayOfLight.X][rayOfLight.Y] = true;
                }
            }
        } while (_context.RaysOfLight.Any());

        return energizedTiles.SelectMany(e => e).Count(b => b);
    }

    public long CalculateMaxEnergizedTilesCount() {
        var result = 0L;
        for (var x = 0; x < _contraption.Length; x++) {
            _context = new ContraptionContext(x, 0, Direction.South);
            result = Math.Max(result, CalculateEnergizedTilesCount());

            _context = new ContraptionContext(x, _contraption[x].Length - 1, Direction.North);
            result = Math.Max(result, CalculateEnergizedTilesCount());
        }

        for (var y = 0; y < _contraption[0].Length; y++) {
            _context = new ContraptionContext(0, y, Direction.East);
            result = Math.Max(result, CalculateEnergizedTilesCount());

            _context = new ContraptionContext(_contraption.Length - 1, y, Direction.West);
            result = Math.Max(result, CalculateEnergizedTilesCount());
        }

        return result;
    }
}

internal static class TheFloorWillBeLavaExtensions {
    public static bool IsOutOfBounds(this TheFloorWillBeLava.ITile[][] contraption, TheFloorWillBeLava.RayOfLight rayOfLight) {
        return rayOfLight.IsOutOfBounds(contraption.Length, contraption[0].Length);
    }
}