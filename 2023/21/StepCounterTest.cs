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
    [TestCase(50, 1_594)]
    [TestCase(100, 6_536)]
    [TestCase(500, 167_004)]
    // [TestCase(1000, 668_697)] // too high for the naive algorithm
    // [TestCase(5000, 16_733_044)]
    public void Example2_CalculateReachablePlotsCount(int steps, long expectedCount) {
        var example = new StepCounter(File.ReadAllLines(@"21\example.txt"));
        
        Assert.AreEqual(expectedCount,  example.CalculateReachablePlotsCount(steps, true ));   
    }
    
    [Test]
    public void Puzzle2() {
        var puzzle = new StepCounter(File.ReadAllLines(@"21\input.txt"));
        
        Assert.AreEqual(610_321_885_082_978L,  puzzle.CalculateReachablePlotsCountViaAlgorithm(26_501_365));  
    }
}
