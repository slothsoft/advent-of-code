using System.IO;
using NUnit.Framework;

namespace AoC._09;

public class RopeBridgeTest {
    
    [Test]
    public void Example1ProgrammaticallyExtended() {
        var ropeBridge = new RopeBridge();
        
        // R 4
        
        ropeBridge.MoveRight();
        Assert.AreEqual(new Point(1, 0), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(0, 0), ropeBridge.TailPosition);
        
        ropeBridge.MoveRight();
        Assert.AreEqual(new Point(2, 0), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(1, 0), ropeBridge.TailPosition);
        
        ropeBridge.MoveRight();
        Assert.AreEqual(new Point(3, 0), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(2, 0), ropeBridge.TailPosition);
        
        ropeBridge.MoveRight();
        Assert.AreEqual(new Point(4, 0), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(3, 0), ropeBridge.TailPosition);
        
        // U 4
        
        ropeBridge.MoveUp();
        Assert.AreEqual(new Point(4, -1), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(3, 0), ropeBridge.TailPosition);
        
        ropeBridge.MoveUp();
        Assert.AreEqual(new Point(4, -2), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(4, -1), ropeBridge.TailPosition);
        
        ropeBridge.MoveUp();
        Assert.AreEqual(new Point(4, -3), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(4, -2), ropeBridge.TailPosition);
        
        ropeBridge.MoveUp();
        Assert.AreEqual(new Point(4, -4), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(4, -3), ropeBridge.TailPosition);
        
        // L 3
        
        ropeBridge.MoveLeft();
        Assert.AreEqual(new Point(3, -4), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(4, -3), ropeBridge.TailPosition);
        
        ropeBridge.MoveLeft();
        Assert.AreEqual(new Point(2, -4), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(3, -4), ropeBridge.TailPosition);
        
        ropeBridge.MoveLeft();
        Assert.AreEqual(new Point(1, -4), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(2, -4), ropeBridge.TailPosition);
        
        // D 1
        
        ropeBridge.MoveDown();
        Assert.AreEqual(new Point(1, -3), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(2, -4), ropeBridge.TailPosition);
        
        // R 4
        
        ropeBridge.MoveRight();
        Assert.AreEqual(new Point(2, -3), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(2, -4), ropeBridge.TailPosition);
        
        ropeBridge.MoveRight();
        Assert.AreEqual(new Point(3, -3), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(2, -4), ropeBridge.TailPosition);
        
        ropeBridge.MoveRight();
        Assert.AreEqual(new Point(4, -3), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(3, -3), ropeBridge.TailPosition);
        
        ropeBridge.MoveRight();
        Assert.AreEqual(new Point(5, -3), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(4, -3), ropeBridge.TailPosition);
        
        // D 1
        
        ropeBridge.MoveDown();
        Assert.AreEqual(new Point(5, -2), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(4, -3), ropeBridge.TailPosition);
        
        // L 5
        
        ropeBridge.MoveLeft();
        Assert.AreEqual(new Point(4, -2), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(4, -3), ropeBridge.TailPosition);
        
        ropeBridge.MoveLeft();
        Assert.AreEqual(new Point(3, -2), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(4, -3), ropeBridge.TailPosition);
        
        ropeBridge.MoveLeft();
        Assert.AreEqual(new Point(2, -2), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(3, -2), ropeBridge.TailPosition);
        
        ropeBridge.MoveLeft();
        Assert.AreEqual(new Point(1, -2), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(2, -2), ropeBridge.TailPosition);
        
        ropeBridge.MoveLeft();
        Assert.AreEqual(new Point(0, -2), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(1, -2), ropeBridge.TailPosition);
        
        // R 2
        
        ropeBridge.MoveRight();
        Assert.AreEqual(new Point(1, -2), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(1, -2), ropeBridge.TailPosition);
        
        ropeBridge.MoveRight();
        Assert.AreEqual(new Point(2, -2), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(1, -2), ropeBridge.TailPosition);
        
        // Aaand the solution is...;
        Assert.AreEqual(13, ropeBridge.VisitedTailPositionsCount);
    }
    
    [Test]
    public void Example1Programmatically() {
        var ropeBridge = new RopeBridge();
        
        // R 4
        
        ropeBridge.MoveRight(4);
        Assert.AreEqual(new Point(4, 0), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(3, 0), ropeBridge.TailPosition);
        
        // U 4
        
        ropeBridge.MoveUp(4);
        Assert.AreEqual(new Point(4, -4), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(4, -3), ropeBridge.TailPosition);
        
        // L 3
        
        ropeBridge.MoveLeft(3);
        Assert.AreEqual(new Point(1, -4), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(2, -4), ropeBridge.TailPosition);
        
        // D 1
        
        ropeBridge.MoveDown(1);
        Assert.AreEqual(new Point(1, -3), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(2, -4), ropeBridge.TailPosition);
        
        // R 4
        
        ropeBridge.MoveRight(4);
        Assert.AreEqual(new Point(5, -3), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(4, -3), ropeBridge.TailPosition);
        
        // D 1
        
        ropeBridge.MoveDown(1);
        Assert.AreEqual(new Point(5, -2), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(4, -3), ropeBridge.TailPosition);
        
        // L 5
        
        ropeBridge.MoveLeft(5);
        Assert.AreEqual(new Point(0, -2), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(1, -2), ropeBridge.TailPosition);
        
        // R 2
        
        ropeBridge.MoveRight(2);
        Assert.AreEqual(new Point(2, -2), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(1, -2), ropeBridge.TailPosition);
        
        // Aaand the solution is...;
        Assert.AreEqual(13, ropeBridge.VisitedTailPositionsCount);
    }
    
    [Test]
    public void Example1() {
        var ropeBridge = new RopeBridge();

        ropeBridge.ExecuteCommands(File.ReadAllLines(@"09\example1.txt"));
        Assert.AreEqual(new Point(2, -2), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(1, -2), ropeBridge.TailPosition);
        
        // Aaand the solution is...;
        Assert.AreEqual(13, ropeBridge.VisitedTailPositionsCount);
    }
    
    [Test]
    public void Puzzle1() {
        var ropeBridge = new RopeBridge();
        ropeBridge.ExecuteCommands(File.ReadAllLines(@"09\input.txt"));
        var result = ropeBridge.VisitedTailPositionsCount;
        Assert.AreEqual(6057, result);
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    public void Example2Programmatically() {
        var ropeBridge = new RopeBridge(9);
        
        // R 5
        
        ropeBridge.MoveRight(5);
        Assert.AreEqual(new Point(5, 0), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(0, 0), ropeBridge.TailPosition);
        
        // U 8
        
        ropeBridge.MoveUp(8);
        Assert.AreEqual(new Point(5, -8), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(0, 0), ropeBridge.TailPosition);
        
        // L 8
        
        ropeBridge.MoveLeft(8);
        Assert.AreEqual(new Point(-3, -8), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(1, -3), ropeBridge.TailPosition);
        
        // D 3
        
        ropeBridge.MoveDown(3);
        Assert.AreEqual(new Point(-3, -5), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(1, -3), ropeBridge.TailPosition);
        
        // R 17
        
        ropeBridge.MoveRight(17);
        Assert.AreEqual(new Point(14, -5), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(5, -5), ropeBridge.TailPosition);
        
        // D 10
        
        ropeBridge.MoveDown(10);
        Assert.AreEqual(new Point(14, 5), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(10, 0), ropeBridge.TailPosition);
        
        // L 25
        
        ropeBridge.MoveLeft(25);
        Assert.AreEqual(new Point(-11, 5), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(-2, 5), ropeBridge.TailPosition);
        
        // U 20
        
        ropeBridge.MoveUp(20);
        Assert.AreEqual(new Point(-11, -15), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(-11, -6), ropeBridge.TailPosition);
        
        Assert.AreEqual(36, ropeBridge.VisitedTailPositionsCount);
    }
    
    [Test]
    public void Example2() {
        var ropeBridge = new RopeBridge(9);

        ropeBridge.ExecuteCommands(File.ReadAllLines(@"09\example2.txt"));
        Assert.AreEqual(new Point(-11, -15), ropeBridge.HeadPosition);
        Assert.AreEqual(new Point(-11, -6), ropeBridge.TailPosition);
        
        Assert.AreEqual(36, ropeBridge.VisitedTailPositionsCount);
    }
    
    [Test]
    public void Puzzle2() {
        var ropeBridge = new RopeBridge(9);
        ropeBridge.ExecuteCommands(File.ReadAllLines(@"09\input.txt"));
        var result = ropeBridge.VisitedTailPositionsCount;
        Assert.AreEqual(2514, result);
        Assert.Pass("Puzzle 2: " + result);
    }
}