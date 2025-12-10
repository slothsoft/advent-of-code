using System.IO;
using NUnit.Framework;

namespace AoC.day10;

public class FactoryTest {
    [Test]
    [TestCase("[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}", 2)]
    [TestCase("[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}", 3)]
    [TestCase("[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}", 2)]
    public void Example1_ParseInput(string input, int expected) {
        var result = Factory.CalculateIndicatorLightsForInputLine(input);
        Assert.AreEqual(expected,  result);
    }
    
    [Test]
    public void Example1() {
        var example = new Factory(File.ReadAllLines(@"10\example.txt"));
        
        Assert.AreEqual(7,  example.CalculateIndicatorLights());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new Factory(File.ReadAllLines(@"10\input.txt"));
        
        Assert.AreEqual(578,  puzzle.CalculateIndicatorLights());  
    }

    [Test]
    public void Example2() {
        var example = new Factory(File.ReadAllLines(@"10\example.txt"));
        
        Assert.AreEqual(7,  example.CalculateIndicatorLights());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new Factory(File.ReadAllLines(@"10\input.txt"));
        
        Assert.AreEqual(7,  puzzle.CalculateIndicatorLights());  
    }
}
