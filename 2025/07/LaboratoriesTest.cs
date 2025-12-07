using System.IO;
using NUnit.Framework;

namespace AoC.day7;

public class LaboratoriesTest {
    [Test]
    public void Example1() {
        var example = new Laboratories(File.ReadAllLines(@"07\example.txt"));
        
        Assert.AreEqual(21,  example.CalculateBeamSplitting());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new Laboratories(File.ReadAllLines(@"07\input.txt"));
        
        Assert.AreEqual(1516,  puzzle.CalculateBeamSplitting());  
    }

    [Test]
    public void Example2() {
        var example = new Laboratories(File.ReadAllLines(@"07\example.txt"));
        
        Assert.AreEqual(40,  example.CalculateTimelines());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new Laboratories(File.ReadAllLines(@"07\input.txt"));
        
        Assert.AreEqual(1_393_669_447_690,  puzzle.CalculateTimelines());  
    }
}
