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

        ropeBridge.ExecuteCommands(File.ReadAllLines(@"09\example.txt"));
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
    public void Example2() {
    }
    
    [Test]
    public void Puzzle2() {
    }
}