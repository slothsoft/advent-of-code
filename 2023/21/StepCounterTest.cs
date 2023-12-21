using System;
using System.IO;
using NUnit.Framework;

namespace AoC.day21;

public class StepCounterTest {
    [Test]
    public void Example1() {
        var example = new StepCounter(File.ReadAllLines(@"21\example.txt"));
        Console.WriteLine(example.StringifyReachablePlots(6));
        
        Assert.AreEqual(16,  example.CalculateReachablePlotsCount(6));       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new StepCounter(File.ReadAllLines(@"21\input.txt"));
        
        Assert.AreEqual(3687,  puzzle.CalculateReachablePlotsCount(64));  
    }

    [Test]
    [TestCase(6, 16)]
    [TestCase(10, 50)]
    [TestCase(50, 1594)]
    [TestCase(100, 6536)]
    [TestCase(500, 167004)]
    [TestCase(1000, 668697)]
    [TestCase(5000, 16733044)]
    public void Example2(int steps, long expectedCount) {
        var example = new StepCounter(File.ReadAllLines(@"21\example.txt"), true);
        
        Assert.AreEqual(expectedCount,  example.CalculateReachablePlotsCount(steps));   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new StepCounter(File.ReadAllLines(@"21\input.txt"));
        
        Assert.AreEqual(7,  puzzle.CalculateReachablePlotsCount(26501365));  
    }
}
