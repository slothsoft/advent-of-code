using System.Collections.Generic;
using System.Linq;
using AoC.algorithm.backtrack;

namespace AoC;

/// <summary>
/// This uses a backtrack algorithm, <see cref="BacktrackAlgorithm{TPosition}"/>.
/// </summary>
public class Puzzle1 : BacktrackAlgorithm<Point>.IPositionManager {

    private readonly DonutMaze _maze;
    
    public Puzzle1(string[] donutAsStrings, int ringSize) {
        _maze = new DonutMaze(donutAsStrings, ringSize);
    }

    public int SolveReturnSteps() {
        var backtrack = new BacktrackAlgorithm<Point>(this);
        return backtrack.Solve().MinBy(s => s.Length)?.Length ?? -1;
    }

    public Point StartPosition => _maze.FetchStartPortal().Location;
    
    public IEnumerable<Point> FindPossibleNextPositions(Point position) {
        var result = _maze.FindPossibleNextPositions(position).ToList();
        if (_maze.PortalConnections.ContainsKey(position)) {
            result.Add(_maze.PortalConnections[position]);
        }
        return result;
    }

    public bool IsEndPosition(Point position) {
        return _maze.Maze[position.X][position.Y] == DonutMaze.End;
    }
}