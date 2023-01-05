using System.Collections.Generic;
using System.Linq;

namespace AoC._15;

/// <summary>
/// <a href="https://adventofcode.com/2021/day/15">Day 15: Chiton</a>: You've almost reached the
/// exit of the cave, but the walls are getting closer together. Your submarine can barely still
/// fit, though; the main problem is that the walls of the cave are covered in chitons, and it
/// would be best not to bump any of them.
///
/// The cavern is large, but has a very low ceiling, restricting your motion to two dimensions.
/// The shape of the cavern resembles a square; a quick scan of chiton density produces a map
/// of risk level throughout the cave (your puzzle input).
/// </summary>
public class Chiton : DijkstraForIntAlgorithm<Point>.IIntNodeManager {
    private int[][] _risks;

    public Chiton(string[] lines) {
        _risks = CreateRisks(lines);
    }

    private int[][] CreateRisks(string[] lines) {
        var result = CreateIntArray(lines[0].Length, lines.Length);

        for (var y = 0; y < lines.Length; y++) {
            for (var x = 0; x < lines[y].Length; x++) {
                result[x][y] = int.Parse(lines[y][x].ToString());
            }
        }

        return result;
    }

    private static int[][] CreateIntArray(int width, int height) {
        var result = new int[width][];

        for (var i = 0; i < result.Length; i++) {
            result[i] = new int[height];
        }

        return result;
    }

    public override string ToString() {
        var result = "";
        for (var y = 0; y < _risks[0].Length; y++) {
            for (var x = 0; x < _risks.Length; x++) {
                result += _risks[x][y];
            }

            result += "\r\n";
        }

        return result;
    }

    public int SolveReturnRisks() {
        var startPoint = new Point(0, 0);
        var endPoint = new Point(_risks.Length - 1, _risks[0].Length - 1);

        var dijkstra = new DijkstraForIntAlgorithm<Point>(this);
        return dijkstra.Solve(startPoint, endPoint);
    }

    public IEnumerable<KeyValuePair<Point, int>> FindAccessibleNodes(Point startNode) {
        var possiblePoints = new[] {
            startNode with {X = startNode.X - 1},
            startNode with {X = startNode.X + 1},
            startNode with {Y = startNode.Y - 1},
            startNode with {Y = startNode.Y + 1},
        };
        return possiblePoints.Where(IsWalkable).Select(p => new KeyValuePair<Point, int>(p, _risks[p.X][p.Y]));
    }

    private bool IsWalkable(Point point) {
        if (point.X < 0 || point.Y < 0)
            return false;
        if (point.X >= _risks.Length || point.Y >= _risks[0].Length)
            return false;
        return true;
    }

    public void RepeatMap(int times) {
        var newRisks = CreateIntArray(_risks.Length * times, _risks[0].Length * times);

        for (var xt = 0; xt < times; xt++) {
            for (var yt = 0; yt < times; yt++) {
                for (var x = 0; x < _risks.Length; x++) {
                    for (var y = 0; y < _risks[x].Length; y++) {
                        newRisks[x + xt * _risks.Length][y + yt * _risks[x].Length] = (_risks[x][y] + xt + yt - 1) % 9 + 1;
                    }
                }
            }
        }
        _risks = newRisks;
    }
}

public record Point(int X, int Y);