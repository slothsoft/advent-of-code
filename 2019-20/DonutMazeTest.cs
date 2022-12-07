using System;
using System.IO;
using NUnit.Framework;

namespace AoC;

public class DonutMazeTest {

    [Test]
    public void Example1A()
    {
        var donutMaze = new DonutMaze(File.ReadAllLines(@"exampleA.txt"), 5);
        Assert.AreEqual(23, donutMaze.SolveReturnSteps());
    }
    
    [Test]
    public void Example1B() {
        var donutMaze = new DonutMaze(File.ReadAllLines(@"exampleB.txt"), 7);
        Assert.AreEqual(58, donutMaze.SolveReturnSteps());
    }
    
    [Test]
    public void Puzzle1() {
        var donutMaze = new DonutMaze(File.ReadAllLines(@"input.txt"), 33);
        var result = donutMaze.SolveReturnSteps();
        Assert.AreEqual(606, result);
        Assert.Pass("Puzzle 1: " + result);
    }
    
    [Test]
    public void Example2A() {
        var donutMaze = new DonutMaze(File.ReadAllLines(@"exampleA.txt"), 5);
        Assert.AreEqual(26, donutMaze.SolveRecursiveReturnSteps());
    }
    
    [Test]
    public void Example2B() {
        var donutMaze = new DonutMaze(File.ReadAllLines(@"exampleB.txt"), 7);
        Console.WriteLine(donutMaze);
        Assert.AreEqual(-1, donutMaze.SolveRecursiveReturnSteps()); // not possible
    }
    
    [Test]
    public void Example2C() {
        var donutMaze = new DonutMaze(File.ReadAllLines(@"exampleC.txt"), 7);
        Console.WriteLine(donutMaze);
        Assert.AreEqual(396, donutMaze.SolveRecursiveReturnSteps());
    }

    [Test]
    public void RemoveDeadEnds() {
        var donutMaze = new DonutMaze(File.ReadAllLines(@"input.txt"), 33);
        Console.WriteLine(donutMaze);
        donutMaze.RemoveDeadEnds();
        Console.WriteLine(donutMaze);
    }

    [Test]
    public void Puzzle2() {
        var donutMaze = new DonutMaze(File.ReadAllLines(@"input.txt"), 33) {
            MaxStepLength = 10000,
            MaxLevel = 20,
        };
        var result = donutMaze.SolveRecursiveReturnSteps();
        Assert.AreEqual(7186, result);
        Assert.Pass("Puzzle 2: " + result);
    }
}