using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/2">Day 2: Rock Paper Scissors </a>: The winner of
/// the whole tournament is the player with the highest score. Your total score is the sum of your
/// scores for each round. The score for a single round is the score for the shape you selected
/// (1 for Rock, 2 for Paper, and 3 for Scissors) plus the score for the outcome of the round
/// (0 if you lost, 3 if the round was a draw, and 6 if you won).
/// </summary>
public static class RockPaperScissors {
    private record Shape(string Opponent, string Response, string Beats, int Score);
    private record Result(string Letter, ResultType Type);

    private enum ResultType {
        Lose = 0,
        Draw = 3,
        Win = 6,
    }
    
    private static readonly Shape[] Shapes = {
        // Rock
        new("A", "X", "C", 1),
        // Paper
        new("B", "Y", "A", 2),
        // Scissors
        new("C", "Z", "B", 3),
    };

    private static readonly Result[] Results = {
        new("Y", ResultType.Draw),
        new("X", ResultType.Lose),
        new("Z", ResultType.Win),
    };

    public static int CalculateShapeScore(IEnumerable<string> lines) {
        var result = 0;

        foreach (var line in lines) {
            var split = line.Split(" ");
            var opponent = Shapes.Single(s => s.Opponent.Equals(split[0]));
            var response = Shapes.Single(s => s.Response.Equals(split[1]));
            var winnigScore = opponent == response ? ResultType.Draw : response.Beats == opponent.Opponent ? ResultType.Win : ResultType.Lose;
            result += response.Score + (int) winnigScore;
        }

        return result;
    }

    public static int CalculateResultScore(IEnumerable<string> lines) {
        var result = 0;

        foreach (var line in lines) {
            var split = line.Split(" ");
            var opponent = Shapes.Single(s => s.Opponent.Equals(split[0]));
            var wantedResult = Results.Single(r => r.Letter.Equals(split[1]));

            var response = wantedResult.Type switch {
                ResultType.Lose => Shapes.Single(s => opponent.Beats == s.Opponent),
                ResultType.Win => Shapes.Single(s => s.Beats == opponent.Opponent),
                _ => opponent
            };
            result += response.Score + (int) wantedResult.Type;
        }

        return result;
    }
}