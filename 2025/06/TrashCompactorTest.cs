using System.IO;
using NUnit.Framework;

namespace AoC.day6;

public class TrashCompactorTest {
    [Test]
    public void Example1() {
        var example = new TrashCompactor(File.ReadAllLines(@"06\example.txt"));
        
        Assert.AreEqual(4_277_556L,  example.CalculateResult());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new TrashCompactor(File.ReadAllLines(@"06\input.txt"));
        
        Assert.AreEqual(8_108_520_669_952L,  puzzle.CalculateResult());  
    }

    [Test]
    public void Example2() {
        var example = new TrashCompactor(File.ReadAllLines(@"06\example.txt"), true);
        
        Assert.AreEqual(3_263_827L,  example.CalculateResult());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new TrashCompactor(File.ReadAllLines(@"06\input.txt"), true);
        
        Assert.AreEqual(11_708_563_470_209L,  puzzle.CalculateResult());  
    }
}
