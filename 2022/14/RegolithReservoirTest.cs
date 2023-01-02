using System.IO;
using NUnit.Framework;

namespace AoC._14;

public class RegolithReservoirTest {
    [Test]
    public void Example1PourInSand() {
        var regolithReservoir = new RegolithReservoir(File.ReadAllLines(@"14\example.txt"));

        Assert.AreEqual(@"..........
..........
..........
..........
....#...##
....#...#.
..###...#.
........#.
........#.
#########.
", regolithReservoir.ToString());

        Assert.IsTrue(regolithReservoir.PourInSand());

        Assert.AreEqual(@"..........
..........
..........
..........
....#...##
....#...#.
..###...#.
........#.
......o.#.
#########.
", regolithReservoir.ToString());

        Assert.IsTrue(regolithReservoir.PourInSand());

        Assert.AreEqual(@"..........
..........
..........
..........
....#...##
....#...#.
..###...#.
........#.
.....oo.#.
#########.
", regolithReservoir.ToString());

        Assert.IsTrue(regolithReservoir.PourInMoreSand(3));

        Assert.AreEqual(@"..........
..........
..........
..........
....#...##
....#...#.
..###...#.
......o.#.
....oooo#.
#########.
", regolithReservoir.ToString());

        Assert.IsTrue(regolithReservoir.PourInMoreSand(17));

        Assert.AreEqual(@"..........
..........
......o...
.....ooo..
....#ooo##
....#ooo#.
..###ooo#.
....oooo#.
...ooooo#.
#########.
", regolithReservoir.ToString());

        Assert.IsTrue(regolithReservoir.PourInMoreSand(2));

        Assert.AreEqual(@"..........
..........
......o...
.....ooo..
....#ooo##
...o#ooo#.
..###ooo#.
....oooo#.
.o.ooooo#.
#########.
", regolithReservoir.ToString());

        Assert.IsFalse(regolithReservoir.PourInSand());
        Assert.IsFalse(regolithReservoir.PourInMoreSand(5));

        Assert.AreEqual(@"..........
..........
......o...
.....ooo..
....#ooo##
...o#ooo#.
..###ooo#.
....oooo#.
.o.ooooo#.
#########.
", regolithReservoir.ToString());
    }

    [Test]
    public void Example1() {
        var regolithReservoir = new RegolithReservoir(File.ReadAllLines(@"14\example.txt"));

        Assert.AreEqual(24, regolithReservoir.SimulateSandPouring());
    }

    [Test]
    public void Puzzle1() {
        var regolithReservoir = new RegolithReservoir(File.ReadAllLines(@"14\input.txt"));
        var result = regolithReservoir.SimulateSandPouring();
        Assert.AreEqual(715, result);
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    public void Example2() {
        var regolithReservoir = new RegolithReservoir(File.ReadAllLines(@"14\example.txt"), true);

        Assert.AreEqual(93, regolithReservoir.SimulateSandPouring());
    }

    [Test]
    public void Puzzle2() {
        var regolithReservoir = new RegolithReservoir(File.ReadAllLines(@"14\input.txt"), true);
        var result = regolithReservoir.SimulateSandPouring();
        Assert.AreEqual(25248, result);
        Assert.Pass("Puzzle 2: " + result);
    }
}