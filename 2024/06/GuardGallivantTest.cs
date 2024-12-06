using System.IO;
using NUnit.Framework;

namespace AoC.day6;

public class GuardGallivantTest {
    [Test]
    public void Example1() {
        var example = new GuardGallivant(File.ReadAllLines(@"06\example.txt"));
        
        Assert.AreEqual(41,  example.CalculateDistinctPositions());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new GuardGallivant(File.ReadAllLines(@"06\input.txt"));
        
        Assert.AreEqual(5318,  puzzle.CalculateDistinctPositions());  
    }

    [Test]
    [TestCase(3, 6, true)]
    [TestCase(6, 7, true)]
    [TestCase(7, 7, true)]
    [TestCase(1, 8, true)]
    [TestCase(3, 8, true)]
    [TestCase(7, 9, true)]
    public void Example2_ContainsLoop(int x, int y, bool expected) {
        var example = new GuardGallivant(File.ReadAllLines(@"06\example.txt"));
        
        Assert.AreEqual(expected,  example.ContainsLoop((x, y)));   
    }
    
    [Test]
    public void Example2() {
        var example = new GuardGallivant(File.ReadAllLines(@"06\example.txt"));
        
        Assert.AreEqual(6,  example.CalculateLoopDeLoopsCount());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new GuardGallivant(File.ReadAllLines(@"06\input.txt"));
        
        Assert.AreEqual(1831,  puzzle.CalculateLoopDeLoopsCount());  
    }
}
