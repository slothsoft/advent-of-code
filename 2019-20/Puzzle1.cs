using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// This is a modified backtrack algorithm.
/// </summary>
public class Puzzle1 {

    private readonly DonutMaze _maze;
    
    public Puzzle1(string[] donutAsStrings, int ringSize) {
        _maze = new DonutMaze(donutAsStrings, ringSize);
    }

    public int SolveReturnSteps() {
        var solutions = new List<Point[]>();
        var currentSteps = new List<Point> {
            _maze.Portals.Single(p => p.Name == DonutMaze.StartPortal).Location
        };
        Solve(solutions, currentSteps);
        return solutions.MinBy(s => s.Length)?.Length ?? -1;
    }

    private void Solve(List<Point[]> solutions, List<Point> currentSteps) {
        var position = currentSteps[^1];
        var possibleNextPositions = _maze.FindPossibleNextPositions(position)
            .Where(p => !currentSteps.Contains(p)) // we will not backtrack
            .ToArray();

        if (possibleNextPositions.Length == 0) {
            return; // there is no way to go
        }

        if (possibleNextPositions.Length == 1) {
            // there is only one way to go, so go there
            Step(solutions, currentSteps, possibleNextPositions[0]);
            return;
        }

        foreach (var possibleNextPosition in possibleNextPositions) {
            Step(solutions, new List<Point>(currentSteps), possibleNextPosition);
        }
    }

    private void Step(List<Point[]> solutions, List<Point> currentSteps, Point nextPosition) {
        if (_maze.PortalConnections.ContainsKey(nextPosition)) {
            // the next step is a portal field
            currentSteps.Add(nextPosition);
            currentSteps.Add(_maze.PortalConnections[nextPosition]);
            Solve(solutions, currentSteps);
        } else if (_maze.Maze[nextPosition.X][nextPosition.Y] == DonutMaze.End) {
            // the next step is THE END
            solutions.Add(currentSteps.ToArray());
        } else {
            // the next step is a regular field
            currentSteps.Add(nextPosition);
            Solve(solutions, currentSteps);
        }
    }
}