using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC.day12;

public class GardenGroupsTest {
    private const string FileExampleA = @"12\example1.txt";
    private const string FileExampleB = @"12\example2.txt";
    private const string FileExampleC = @"12\example3.txt";
    
    [Test]
    [TestCase(FileExampleA, 'A', 0,0)]
    [TestCase(FileExampleA, 'A', 1,0)]
    [TestCase(FileExampleA, 'A', 2,0)]
    [TestCase(FileExampleA, 'A', 3,0)]
    
    [TestCase(FileExampleA, 'B', 0,1)]
    [TestCase(FileExampleA, 'B', 1,1)]
    [TestCase(FileExampleA, 'C', 2,1)]
    [TestCase(FileExampleA, 'D', 3,1)]
    
    [TestCase(FileExampleA, 'B', 0,2)]
    [TestCase(FileExampleA, 'B', 1,2)]
    [TestCase(FileExampleA, 'C', 2,2)]
    [TestCase(FileExampleA, 'C', 3,2)]
    
    [TestCase(FileExampleA, 'E', 0,3)]
    [TestCase(FileExampleA, 'E', 1,3)]
    [TestCase(FileExampleA, 'E', 2,3)]
    [TestCase(FileExampleA, 'C', 3,3)]
    public void Example1_ParseInput(string file, char checkedRegion, int expectedX, int expectedY) {
        var result = GardenGroups.FetchRegions(File.ReadAllLines(file).ParseCharMatrix());
        
        Assert.NotNull(result);

        var region = result.Single(r => r.PlantType == checkedRegion);
        Assert.AreEqual(true,  region.IncludedCoords.Contains((expectedX, expectedY)), string.Join("\n", region.IncludedCoords));
    }
    
    [Test]
    [TestCase(FileExampleA, 140)]
    [TestCase(FileExampleB, 772)]
    [TestCase(FileExampleC, 1930)]
    public void Example1(string file, long expectedFencingPrice) {
        var example = new GardenGroups(File.ReadAllLines(file));
        
        Assert.AreEqual(expectedFencingPrice,  example.CalculateFencingPrice());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new GardenGroups(File.ReadAllLines(@"12\input.txt"));
        
        Assert.AreEqual(1522850,  puzzle.CalculateFencingPrice());  
    }

    [Test]
    public void Example2() {
        var example = new GardenGroups(File.ReadAllLines(@"12\example.txt"));
        
        Assert.AreEqual(7,  example.CalculateFencingPrice());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new GardenGroups(File.ReadAllLines(@"12\input.txt"));
        
        Assert.AreEqual(7,  puzzle.CalculateFencingPrice());  
    }
}
