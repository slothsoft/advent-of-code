using System;
using System.IO;
using NUnit.Framework;

namespace AoC;

public class LavaductLagoonTest {
    [Test]
    public void Example1() {
        var example = new LavaductLagoon(File.ReadAllLines(@"18\example.txt"));
        
        Assert.AreEqual(62,  example.CalculateInsideTiles());       
    }

    [Test]
    public void Puzzle1() {
        var example = new LavaductLagoon(File.ReadAllLines(@"18\input.txt"));
        
        Assert.AreEqual(40714,  example.CalculateInsideTiles());    
    }

    [Test]
    public void Example2() {
        var example = new LavaductLagoon(File.ReadAllLines(@"18\example.txt"), true);
        
        // Assert.AreEqual(952408144115,  example.CalculateInsideTiles());       
    }

    [Test]
    public void Puzzle2() {
    }
}