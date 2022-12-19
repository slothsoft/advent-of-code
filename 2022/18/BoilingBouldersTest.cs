using System.IO;
using NUnit.Framework;

namespace AoC._18;

public class BoilingBouldersTest {
    
    [Test]
    public void Example1A() {
        var boilingBoulders = new BoilingBoulders(File.ReadAllLines(@"18\exampleA.txt"), 2);
        
        Assert.AreEqual(10, boilingBoulders.CalculateSurfaceArea());
    }
    
    [Test]
    public void Example1B() {
        var boilingBoulders = new BoilingBoulders(File.ReadAllLines(@"18\exampleB.txt"), 6);
        
        Assert.AreEqual(64, boilingBoulders.CalculateSurfaceArea());
    }

    [Test]
    public void Puzzle1() {
        var boilingBoulders = new BoilingBoulders(File.ReadAllLines(@"18\input.txt"), 20);
        var result = boilingBoulders.CalculateSurfaceArea();
        Assert.AreEqual(3466, result);
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    public void Example2() {
        var boilingBoulders = new BoilingBoulders(File.ReadAllLines(@"18\exampleB.txt"), 6);
        
        Assert.AreEqual(58, boilingBoulders.CalculateExteriorSurfaceArea());
    }

    [Test]
    public void Puzzle2() {
        var boilingBoulders = new BoilingBoulders(File.ReadAllLines(@"18\input.txt"), 20);
        var result = boilingBoulders.CalculateExteriorSurfaceArea();
        //Assert.AreEqual(3466, result);
        Assert.Pass("Puzzle 2: " + result);
    }
}