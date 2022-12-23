using System.IO;
using NUnit.Framework;

namespace AoC._23.WithTuples;

public class UnstableDiffusionTest {
    [Test]
    public void Example1ASteps() {
        var unstableDiffusion = new UnstableDiffusion(File.ReadAllLines(@"23\exampleA.txt"));

        Assert.AreEqual(@"##
#.
..
##
", unstableDiffusion.ToString());

        unstableDiffusion.ExecuteRound();

        Assert.AreEqual(@"##
..
#.
.#
#.
", unstableDiffusion.ToString());

        unstableDiffusion.ExecuteRound();

        Assert.AreEqual(@".##.
#...
...#
....
.#..
", unstableDiffusion.ToString());

        unstableDiffusion.ExecuteRound();

        Assert.AreEqual(@"..#..
....#
#....
....#
.....
..#..
", unstableDiffusion.ToString());

        unstableDiffusion.ExecuteRound();

        Assert.AreEqual(@"..#..
....#
#....
....#
.....
..#..
", unstableDiffusion.ToString());
    }

    [Test]
    public void Example1BSteps() {
        var unstableDiffusion = new UnstableDiffusion(File.ReadAllLines(@"23\exampleB.txt"));

        Assert.AreEqual(@"....#..
..###.#
#...#.#
.#...##
#.###..
##.#.##
.#..#..
", unstableDiffusion.ToString());

        unstableDiffusion.ExecuteRound();

        Assert.AreEqual(@".....#...
...#...#.
.#..#.#..
.....#..#
..#.#.##.
#..#.#...
#.#.#.##.
.........
..#..#...
", unstableDiffusion.ToString());

        unstableDiffusion.ExecuteRound();

        Assert.AreEqual(@"......#....
...#.....#.
..#..#.#...
......#...#
..#..#.#...
#...#.#.#..
...........
.#.#.#.##..
...#..#....
", unstableDiffusion.ToString());

        unstableDiffusion.ExecuteRounds(8);

        Assert.AreEqual(@"......#.....
..........#.
.#.#..#.....
.....#......
..#.....#..#
#......##...
....##......
.#........#.
...#.#..#...
............
...#..#..#..
", unstableDiffusion.ToString());
    }

    [Test]
    public void Example1B() {
        var unstableDiffusion = new UnstableDiffusion(File.ReadAllLines(@"23\exampleB.txt"));

        Assert.AreEqual(110, unstableDiffusion.CalculateEmptyGroundAfterRounds(10));
    }

    [Test]
    public void Puzzle1() {
        var unstableDiffusion = new UnstableDiffusion(File.ReadAllLines(@"23\input.txt"));

        var result = unstableDiffusion.CalculateEmptyGroundAfterRounds(10);
        Assert.AreEqual(3864, result);
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    public void Example2() {
        var unstableDiffusion = new UnstableDiffusion(File.ReadAllLines(@"23\exampleB.txt"));

        Assert.AreEqual(20, unstableDiffusion.CalculateRoundWhereNoElfMoved());
    }

    [Test]
    public void Puzzle2() {
        var unstableDiffusion = new UnstableDiffusion(File.ReadAllLines(@"23\input.txt"));

        var result = unstableDiffusion.CalculateRoundWhereNoElfMoved();
        //Assert.AreEqual(3864, result);
        Assert.Pass("Puzzle 2: " + result);
    }
}