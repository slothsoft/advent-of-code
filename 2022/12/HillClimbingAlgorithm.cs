using System;
using System.Collections.Generic;
using System.Linq;
using AoC._15;

namespace AoC._12;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/12">Day 12: Hill Climbing Algorithm</a>: You try
/// contacting the Elves using your handheld device, but the river you're following must be
/// too low to get a decent signal.
/// </summary>
public class HillClimbingAlgorithm : DijkstraForIntAlgorithm<Point>.IIntNodeManager {
    private readonly char[][] _map;

    public HillClimbingAlgorithm(string[] lines) {
        _map = CreateMap(lines);
    }

    private char[][] CreateMap(string[] lines) {
        var result = CreateCharArray(lines[0].Length, lines.Length);

        for (var y = 0; y < lines.Length; y++) {
            for (var x = 0; x < lines[y].Length; x++) {
                result[x][y] = lines[y][x];
            }
        }

        return result;
    }

    private static char[][] CreateCharArray(int width, int height) {
        var result = new char[width][];

        for (var i = 0; i < result.Length; i++) {
            result[i] = new char[height];
        }

        return result;
    }

    public override string ToString() {
        var result = "";
        for (var y = 0; y < _map[0].Length; y++) {
            for (var x = 0; x < _map.Length; x++) {
                result += _map[x][y];
            }

            result += "\r\n";
        }

        return result;
    }

    public int SolveReturnDistance() {
        var startPoint = FindPoint('S');
        var endPoint = FindPoint('E');

        var dijkstra = new DijkstraForIntAlgorithm<Point>(this);
        return dijkstra.Solve(startPoint, endPoint);
    }

    private Point FindPoint(char c) {
        for (var x = 0; x < _map.Length; x++) {
            for (var y = 0; y < _map[x].Length; y++) {
                if (_map[x][y] == c) {
                    return new Point(x, y);
                }
            }
        }
        throw new ArgumentException("Cannot find point " + c);
    }

    public IEnumerable<KeyValuePair<Point, int>> FindAccessibleNodes(Point startNode) {
        var possiblePoints = new[] {
            startNode with {X = startNode.X - 1},
            startNode with {X = startNode.X + 1},
            startNode with {Y = startNode.Y - 1},
            startNode with {Y = startNode.Y + 1},
        };
        return possiblePoints.Where(p => IsWalkable(startNode, p)).Select(p => new KeyValuePair<Point, int>(p, 1));
    }

    private bool IsWalkable(Point startNode, Point point) {
        if (point.X < 0 || point.Y < 0)
            return false;
        if (point.X >= _map.Length || point.Y >= _map[0].Length)
            return false;
        if (_map[startNode.X][startNode.Y] == 'S')
            // the start point is treated like a
            return _map[point.X][point.Y] - 'a' <= 1;
        if (_map[point.X][point.Y] == 'E')
            // the end point is treated like a
            return 'z' - _map[startNode.X][startNode.Y] <= 1;
        
        return _map[point.X][point.Y] - _map[startNode.X][startNode.Y] <= 1;
    }

    public int SolveForHikingTrail() {
        var result = int.MaxValue;
        var dijkstra = new DijkstraForIntAlgorithm<Point>(this);
        var endPoint = FindPoint('E');

        for (var x = 0; x < _map.Length; x++) {
            for (var y = 0; y < _map[x].Length; y++) {
                if (_map[x][y] == 'a' || _map[x][y] == 'S') {
                    var possibleResult = dijkstra.Solve(new Point(x, y), endPoint);
                    if (possibleResult != 0) {
                        result = Math.Min(possibleResult, result);
                    }
                }
            }
        }
        return result;
    }
}

public record Point(int X, int Y);