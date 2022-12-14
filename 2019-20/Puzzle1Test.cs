using System.IO;
using NUnit.Framework;

namespace AoC;

public class Puzzle1Test {

    [Test]
    public void Example1A()
    {
        var donutMaze = new Puzzle1(File.ReadAllLines(@"exampleA.txt"), 5);
        Assert.AreEqual(23, donutMaze.SolveReturnSteps());
    }
    
    [Test]
    public void Example1B() {
        var donutMaze = new Puzzle1(File.ReadAllLines(@"exampleB.txt"), 7);
        Assert.AreEqual(58, donutMaze.SolveReturnSteps());
    }
    
    [Test]
    public void Puzzle1() {
        var donutMaze = new Puzzle1(File.ReadAllLines(@"input.txt"), 33);
        var result = donutMaze.SolveReturnSteps();
        Assert.AreEqual(606, result);
        Assert.Pass("Puzzle 1: " + result);
    }
}