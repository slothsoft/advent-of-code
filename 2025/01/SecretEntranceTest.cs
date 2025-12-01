using System.IO;
using NUnit.Framework;

namespace AoC.day1;

public class SecretEntranceTest {
    [Test]
    public void Example1() {
        var example = new SecretEntrance(File.ReadAllLines(@"01\example.txt"));
        
        Assert.AreEqual(3,  example.Calculate());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new SecretEntrance(File.ReadAllLines(@"01\input.txt"));
        
        Assert.AreEqual(962,  puzzle.Calculate());  
    }

    [Test]
    public void Example2() {
        var example = new SecretEntrance(File.ReadAllLines(@"01\example.txt"));
        
        Assert.AreEqual(6,  example.Calculate0x434C49434B());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new SecretEntrance(File.ReadAllLines(@"01\input.txt"));
        
        // not 2410, 5765, 5682, 6287
        Assert.AreEqual(0,  puzzle.Calculate0x434C49434B());  
    }
}
