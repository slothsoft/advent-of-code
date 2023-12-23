using System.IO;
using NUnit.Framework;

namespace AoC.day23;

public class ALongWalkTest {
    [Test]
    public void Example1() {
        var example = new ALongWalk(File.ReadAllLines(@"23\example.txt"));
        
        Assert.AreEqual(94,  example.CalculateLongestHike());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new ALongWalk(File.ReadAllLines(@"23\input.txt"));
        
        Assert.AreEqual(2358,  puzzle.CalculateLongestHike());  
    }

    [Test]
    public void Example2() {
        var example = new ALongWalk(File.ReadAllLines(@"23\example.txt"));
        
        Assert.AreEqual(154,  example.CalculateLongestHike());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new ALongWalk(File.ReadAllLines(@"23\input.txt"));
        
        Assert.AreEqual(2358,  puzzle.CalculateLongestHike());  
    }
}
