 using System;
using System.IO;
using NUnit.Framework;

namespace AoC;

public class Puzzle2Test {

    [Test]
    public void Example2A() {
        var donutMaze = new Puzzle2(File.ReadAllLines(@"exampleA.txt"), 5);
        Assert.AreEqual(26, donutMaze.SolveReturnSteps());
    }
    
    [Test]
    public void Example2B() {
        var donutMaze = new Puzzle2(File.ReadAllLines(@"exampleB.txt"), 7);
        Console.WriteLine(donutMaze);
        // Assert.AreEqual(-1, donutMaze.SolveReturnSteps()); // not possible
    }
    
    [Test]
    public void Example2C() {
        var donutMaze = new Puzzle2(File.ReadAllLines(@"exampleC.txt"), 7);
        Console.WriteLine(donutMaze);
        Assert.AreEqual(396, donutMaze.SolveReturnSteps());
    }

    [Test]
    public void Puzzle2() {
        var donutMaze = new Puzzle2(File.ReadAllLines(@"input.txt"), 33);
        var result = donutMaze.SolveReturnSteps();
        Assert.AreEqual(7186, result);
        Assert.Pass("Puzzle 2: " + result);
    }
}