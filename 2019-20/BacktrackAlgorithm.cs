using System.Collections.Generic;
using System.Linq;

namespace AoC.algorithm.backtrack; 

/// <summary>
/// Extract of the original puzzle 1 algorithm, to be a bit more general.
/// </summary>
public class BacktrackAlgorithm<TPosition> {
    
    public interface IPositionManager {
        TPosition StartPosition { get; }
        IEnumerable<TPosition> FindPossibleNextPositions(TPosition position);
        bool IsEndPosition(TPosition position);
    }

    private readonly IPositionManager _positionManager;
    
    public BacktrackAlgorithm(IPositionManager positionManager) {
        _positionManager = positionManager;
    }
    
    public List<TPosition[]> Solve() {
        var solutions = new List<TPosition[]>();
        var currentSteps = new List<TPosition> {
            _positionManager.StartPosition
        };
        Solve(solutions, currentSteps);
        return solutions;
    }

    private void Solve(List<TPosition[]> solutions, List<TPosition> currentSteps) {
        var position = currentSteps[^1];
        var possibleNextPositions = _positionManager.FindPossibleNextPositions(position)
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

        // there are many ways to go, so split up
        foreach (var possibleNextPosition in possibleNextPositions) {
            Step(solutions, new List<TPosition>(currentSteps), possibleNextPosition);
        }
    }

    private void Step(List<TPosition[]> solutions, List<TPosition> currentSteps, TPosition nextPosition) {
        if (_positionManager.IsEndPosition(nextPosition)) {
            // the next step is THE END
            solutions.Add(currentSteps.ToArray());
        } else {
            // the next step is a regular field
            currentSteps.Add(nextPosition);
            Solve(solutions, currentSteps);
        }
    }
}