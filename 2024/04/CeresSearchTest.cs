using System.IO;
using NUnit.Framework;

namespace AoC.day4;

public class CeresSearchTest {
    [Test]
    [TestCase(@"04\example1.txt", 4)]
    [TestCase(@"04\example2.txt", 18)]
    [TestCase(@"04\example3.txt", 18)]
    public void Example1(string exampleFile, long expectedXmas) {
        var example = new CeresSearch(File.ReadAllLines(exampleFile));
        
        Assert.AreEqual(expectedXmas,  example.CalculateXmasCount());       
    }
    

    [Test]
    public void Puzzle1() {
        var puzzle = new CeresSearch(File.ReadAllLines(@"04\input.txt"));
        
        Assert.AreEqual(2569,  puzzle.CalculateXmasCount());  
    }

    [Test]
    [TestCase(@"04\example4.txt", 1)]
    [TestCase(@"04\example5.txt", 9)]
    public void Example2(string exampleFile, long expectedCrossMas) {
        var example = new CeresSearch(File.ReadAllLines(exampleFile));
        
        Assert.AreEqual(expectedCrossMas,  example.CalculateCrossMasCount());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new CeresSearch(File.ReadAllLines(@"04\input.txt"));
        
        Assert.AreEqual(1998,  puzzle.CalculateCrossMasCount());   
    }
}
