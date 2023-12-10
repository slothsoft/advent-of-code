using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC;

public class PipeMazeTest {
    [Test]
    [TestCase(@"10\example_big_loop.txt", 'F')]
    [TestCase(@"10\example_big_loop_with_extras.txt", 'F')]
    [TestCase(@"10\example_small_loop.txt", 'F')]
    [TestCase(@"10\example_small_loop_with_extras.txt", 'F')]
    public void Example1_ReplaceStartTile(string inputFile, char expectedTileValue) {
        var pipeMaze = new PipeMaze(File.ReadAllLines(inputFile));
        var startTile = pipeMaze.Tiles.SelectMany(t => t).Single(t => t.IsStartTile);
        var tile = PipeMaze.possibleTiles.Single(t => t.Value == expectedTileValue);
        var expectedTile = new PipeMaze.Tile(startTile.Value, tile.ConnectsNorth, tile.ConnectsSouth, tile.ConnectsEast, tile.ConnectsWest);

        Assert.AreEqual(expectedTile, startTile);
    }

    [Test]
    [TestCase(@"10\example_big_loop.txt", 16)]
    [TestCase(@"10\example_big_loop_with_extras.txt", 16)]
    [TestCase(@"10\example_small_loop.txt", 8)]
    [TestCase(@"10\example_small_loop_with_extras.txt", 8)]
    public void Example1(string inputFile, long expectedLength) {
        var example = new PipeMaze(File.ReadAllLines(inputFile));

        Assert.AreEqual(expectedLength, example.CalculateLoopLength());
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new PipeMaze(File.ReadAllLines(@"10\input.txt"));

        Assert.AreEqual(2 * 6800, puzzle.CalculateLoopLength());
    }

    [Test]
    [TestCase(@"10\example_big_loop.txt", 1)]
    [TestCase(@"10\example_big_loop_with_extras.txt", 1)]
    [TestCase(@"10\example_small_loop.txt", 1)]
    [TestCase(@"10\example_small_loop_with_extras.txt", 1)]
    [TestCase(@"10\example_with_large_nest.txt", 8)]
    [TestCase(@"10\example_with_large_nest_and_clutter.txt", 10)]
    [TestCase(@"10\example_with_nest.txt", 4)]
    [TestCase(@"10\example_with_tight_nest.txt", 4)]
    public void Example2(string inputFile, long expectedArea) {
        var example = new PipeMaze(File.ReadAllLines(inputFile));

        Assert.AreEqual(expectedArea, example.CalculateEnclosedAreaSize());
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new PipeMaze(File.ReadAllLines(@"10\input.txt"));

        Assert.AreEqual(483, puzzle.CalculateEnclosedAreaSize());
    }
}