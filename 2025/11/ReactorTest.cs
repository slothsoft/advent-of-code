using System.IO;
using NUnit.Framework;

namespace AoC.day11;

public class ReactorTest {
    [Test]
    public void Example1() {
        var example = new Reactor(File.ReadAllLines(@"11\example1.txt"));
        
        Assert.AreEqual(5,  example.CalculatePathsCount());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new Reactor(File.ReadAllLines(@"11\input.txt"));
        
        Assert.AreEqual(413,  puzzle.CalculatePathsCount());  
    }

    [Test]
    public void Example2() {
        var example = new Reactor(File.ReadAllLines(@"11\example2.txt"));
        
        Assert.AreEqual(2,  example.CalculatePathsWithDevicesCount());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new Reactor(File.ReadAllLines(@"11\input.txt"));
        
        Assert.AreEqual(7,  puzzle.CalculatePathsWithDevicesCount());  
    }
}
