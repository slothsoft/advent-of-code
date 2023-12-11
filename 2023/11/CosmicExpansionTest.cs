using System.IO;
using NUnit.Framework;

namespace AoC;

public class CosmicExpansionTest {
    [Test]
    [TestCase(1, 6, 5, 11, 9)]
    [TestCase(4, 0, 9, 10, 15)]
    [TestCase(0, 2, 12, 7, 17)]
    [TestCase(0, 12, 5, 12, 5)]
    [TestCase(12, 0, 12, 5, 5)]
    public void Example1_CalculateDistanceTo(int galaxy1X, int galaxy1Y, int galaxy2X, int galaxy2Y, int expectedDistance) {
        var galaxy1 = new CosmicExpansion.GalaxyLocation(galaxy1X, galaxy1Y);
        var galaxy2 = new CosmicExpansion.GalaxyLocation(galaxy2X, galaxy2Y);

        Assert.AreEqual(expectedDistance, galaxy1.CalculateDistanceTo(galaxy2));
        Assert.AreEqual(expectedDistance, galaxy2.CalculateDistanceTo(galaxy1));

        galaxy1 = new CosmicExpansion.GalaxyLocation(galaxy1Y, galaxy1X);
        galaxy2 = new CosmicExpansion.GalaxyLocation(galaxy2Y, galaxy2X);

        Assert.AreEqual(expectedDistance, galaxy1.CalculateDistanceTo(galaxy2));
        Assert.AreEqual(expectedDistance, galaxy2.CalculateDistanceTo(galaxy1));
    }

    [Test]
    public void Example1_Expand() {
        var cosmicExpansion = new CosmicExpansion(File.ReadAllLines(@"11\example.txt"));
        cosmicExpansion.ExpandUniverse();
        
        var expandedCosmicExpansion = new CosmicExpansion(File.ReadAllLines(@"11\example_expanded.txt"));

        Assert.AreEqual(expandedCosmicExpansion.galaxyLocations, cosmicExpansion.galaxyLocations);
    }
    
    [Test]
    public void Example1() {
        var example = new CosmicExpansion(File.ReadAllLines(@"11\example.txt"));
        example.ExpandUniverse();

        Assert.AreEqual(374, example.CalculateShortestDistanceSum());
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new CosmicExpansion(File.ReadAllLines(@"11\input.txt"));
        puzzle.ExpandUniverse();

        Assert.AreEqual(9556712, puzzle.CalculateShortestDistanceSum());
    }

    [Test]
    public void Example2_Ten() {
        var example = new CosmicExpansion(File.ReadAllLines(@"11\example.txt"));
        example.ExpandUniverse(9);

        Assert.AreEqual(1030, example.CalculateShortestDistanceSum());
    }
    
    [Test]
    public void Example2_Hundred() {
        var example = new CosmicExpansion(File.ReadAllLines(@"11\example.txt"));
        example.ExpandUniverse(99);

        Assert.AreEqual(8410, example.CalculateShortestDistanceSum());
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new CosmicExpansion(File.ReadAllLines(@"11\input.txt"));
        puzzle.ExpandUniverse(999_999);

        Assert.AreEqual(678626199476, puzzle.CalculateShortestDistanceSum());
    }
}