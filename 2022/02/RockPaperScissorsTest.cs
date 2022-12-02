using System.IO;
using NUnit.Framework;

namespace AoC;

public class RockPaperScissorsTest {

    private static readonly string[] ExampleInput = {
        "A Y",
        "B X",
        "C Z"
    };
    
    [Test]
    public void Example1() {
        Assert.AreEqual(15, RockPaperScissors.CalculateShapeScore(ExampleInput));
    }
    
    [Test]
    public void Puzzle1() {
        Assert.Pass("Total Score 1: " + RockPaperScissors.CalculateShapeScore(File.ReadAllLines(@"02\input.txt")));
    }

    [Test]
    public void Example2() {
        Assert.AreEqual(12, RockPaperScissors.CalculateResultScore(ExampleInput));
        
        // 1 for Rock
        // 2 for Paper
        // 3 for Scissors
        Assert.AreEqual(43, RockPaperScissors.CalculateResultScore(new [] {
            "A Z", // Rock Win (Paper) => 2 + 6
            "C Z", // Scissors Win (Rock) => 1 + 6
            "C Z", // Scissors Win (Rock) => 1 + 6
            "A Z", // Rock Win (Paper) => 2 + 6
            "C Y", // Scissors Draw (Scissors) => 3 + 3
            "C Z", // Scissors Win (Rock) => 1 + 6
        }));
    }
    
    [Test]
    public void Puzzle2() {
        Assert.Pass("Total Score 2: " + RockPaperScissors.CalculateResultScore(File.ReadAllLines(@"02\input.txt")));
    }
}