using System;
using System.Collections.Generic;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/17">Day 17: Clumsy Crucible:</a> Directing the crucible from the lava pool to the machine parts factory, but not
/// moving more than three consecutive blocks in the same direction, what is the least heat loss it can incur?
/// </summary>
public class ClumsyCrucible {
    public record HeatLossStep(int X, int Y, Direction LastDirection);

    public struct Direction {
        public static Direction North = new("N", 0, -1);
        public static Direction South = new("S", 0, 1);
        public static Direction East = new("E", 1, 0);
        public static Direction West = new("W", -1, 0);
        public static Direction[] All = {North, South, East, West};

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

    private int[][] _input;

    public ClumsyCrucible(IEnumerable<string> input) {
        _input = input.ParseMatrix(i => int.Parse(i.ToString()));
    }

    public int MinDistance { get; set; } = 1;
    public int MaxDistance { get; set; } = 3;

    public int CalculateHeatLoss() {
        var dijkstra = new DijkstraForIntAlgorithm<HeatLossStep>(FindAccessibleNodes);
        return dijkstra.Solve(new HeatLossStep(0, 0, Direction.East), node => node.X == _input.Length - 1 && node.Y == _input[0].Length - 1);
    }

    public IEnumerable<KeyValuePair<HeatLossStep, int>> FindAccessibleNodes(HeatLossStep startNode) {
        foreach (var direction in Direction.All) {
            if (startNode is not {X: 0, Y: 0}) {
                // after going south 3 steps, we can not continue south (except for the "east" of the starting point)
                if (direction.Equals(startNode.LastDirection)) {
                    continue;
                }

                // we cannot turn 180° and go back
                if (direction.Equals(startNode.LastDirection.ToOppositeDirection())) {
                    continue;
                }
            }

            for (var multiplier = MinDistance; multiplier <= MaxDistance; multiplier++) {
                var newX = startNode.X + multiplier * direction.XPlus;
                var newY = startNode.Y + multiplier * direction.YPlus;

                if (newX >= 0 && newX < _input.Length && newY >= 0 && newY < _input[0].Length) {
                    var heatLoss = 0;
                    for (var i = 0; i < multiplier; i++) {
                        heatLoss += _input[startNode.X + (i + 1) * direction.XPlus][startNode.Y + (i + 1) * direction.YPlus];
                    }

                    yield return new KeyValuePair<HeatLossStep, int>(new HeatLossStep(newX, newY, direction), heatLoss);
                } else {
                    break;
                }
            }
        }
    }
}