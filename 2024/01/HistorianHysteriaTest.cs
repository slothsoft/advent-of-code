using System.IO;
using NUnit.Framework;

namespace AoC.day1;

public class HistorianHysteriaTest {
    [Test]
    public void Example1() {
        var example = new HistorianHysteria(File.ReadAllLines(@"01\example.txt"));
        
        Assert.AreEqual(11,  example.CalculateTotalDistance());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new HistorianHysteria(File.ReadAllLines(@"01\input.txt"));
        
        Assert.AreEqual(3714264,  puzzle.CalculateTotalDistance());  
    }

    [Test]
    public void Example2() {
        var example = new HistorianHysteria(File.ReadAllLines(@"01\example.txt"));
        
        Assert.AreEqual(31,  example.CalculateSimilarityScore());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new HistorianHysteria(File.ReadAllLines(@"01\input.txt"));
        
        Assert.AreEqual(18805872,  puzzle.CalculateSimilarityScore());  
    }
}
