using System.IO;
using NUnit.Framework;

namespace AoC.day25;

public class CodeChronicleTest {
    [Test]
    public void Example1() {
        var example = new CodeChronicle(File.ReadAllLines(@"25\example.txt"));
        
        Assert.AreEqual(3,  example.CalculateFittingKeyLocks(7));       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new CodeChronicle(File.ReadAllLines(@"25\input.txt"));
        
        Assert.AreEqual(7,  puzzle.CalculateFittingKeyLocks(7));  
    }
}
