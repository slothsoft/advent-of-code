using System.IO;
using NUnit.Framework;

namespace AoC._12;

public class HillClimbingAlgorithmTest {
    
    [Test]
    public void Example1() {
        var hillClimbingAlgorithm = new HillClimbingAlgorithm(File.ReadAllLines(@"12\example.txt"));
        Assert.AreEqual(31, hillClimbingAlgorithm.SolveReturnDistance());
    }
    
    [Test]
    public void Puzzle1() {
        var hillClimbingAlgorithm = new HillClimbingAlgorithm(File.ReadAllLines(@"12\input.txt"));
        var result = hillClimbingAlgorithm.SolveReturnDistance();
        Assert.AreEqual(462, result);
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    public void Example2() {
        var hillClimbingAlgorithm = new HillClimbingAlgorithm(File.ReadAllLines(@"12\example.txt"));
        Assert.AreEqual(29, hillClimbingAlgorithm.SolveForHikingTrail());
    }
    
    [Test]
    public void Puzzle2() {
        var hillClimbingAlgorithm = new HillClimbingAlgorithm(File.ReadAllLines(@"12\input.txt"));
        var result = hillClimbingAlgorithm.SolveForHikingTrail();
        Assert.AreEqual(451, result);
        Assert.Pass("Puzzle 2: " + result);
    }
}