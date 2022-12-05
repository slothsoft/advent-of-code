using System;
using System.IO;
using NUnit.Framework;

namespace AoC._05;

public class SupplyStacksTest {

    [Test]
    public void Example1Programmatic()
    {
        var supplyStacks = new SupplyStacks();
        supplyStacks.InitStack(1, 'N', 'Z');
        supplyStacks.InitStack(2, 'D', 'C', 'M');
        supplyStacks.InitStack(3, 'P');

        supplyStacks.Move(1, 2, 1);
        
        Assert.AreEqual(new[]{'D', 'N', 'Z'}, supplyStacks.PeekAtStack(1));
        Assert.AreEqual(new[]{'C', 'M'}, supplyStacks.PeekAtStack(2));
        Assert.AreEqual(new[]{'P'}, supplyStacks.PeekAtStack(3));
        
        supplyStacks.Move(3, 1, 3);
        
        Assert.AreEqual(Array.Empty<char>(), supplyStacks.PeekAtStack(1));
        Assert.AreEqual(new[]{'C', 'M'}, supplyStacks.PeekAtStack(2));
        Assert.AreEqual(new[]{'Z', 'N', 'D', 'P'}, supplyStacks.PeekAtStack(3));
        
        supplyStacks.Move(2, 2, 1);
        
        Assert.AreEqual(new[]{'M', 'C'}, supplyStacks.PeekAtStack(1));
        Assert.AreEqual(Array.Empty<char>(), supplyStacks.PeekAtStack(2));
        Assert.AreEqual(new[]{'Z', 'N', 'D', 'P'}, supplyStacks.PeekAtStack(3));
        
        supplyStacks.Move(1, 1, 2);
        
        Assert.AreEqual(new[]{'C'}, supplyStacks.PeekAtStack(1));
        Assert.AreEqual(new[]{'M'}, supplyStacks.PeekAtStack(2));
        Assert.AreEqual(new[]{'Z', 'N', 'D', 'P'}, supplyStacks.PeekAtStack(3));
        
        Assert.AreEqual("CMZ", supplyStacks.PeekAtTopBoxes());
    }
    
    [Test]
    public void Example1() {
        var supplyStacks = new SupplyStacks();
        Assert.AreEqual("CMZ", supplyStacks.ParseAndExecute(File.ReadAllLines(@"05\example.txt")));
    }
    
    [Test]
    public void Puzzle1() {
        var supplyStacks = new SupplyStacks();
        var result = supplyStacks.ParseAndExecute(File.ReadAllLines(@"05\input.txt"));
        Assert.AreEqual("RLFNRTNFB", result);
        Assert.Pass("Puzzle 1: " + result);
    }
    
    [Test]
    public void Example2Programmatic()
    {
        var supplyStacks = new SupplyStacks()
        {
            CraneMover9001 = true,
        };
        supplyStacks.InitStack(1, 'N', 'Z');
        supplyStacks.InitStack(2, 'D', 'C', 'M');
        supplyStacks.InitStack(3, 'P');

        supplyStacks.Move(1, 2, 1);
        
        Assert.AreEqual(new[]{'D', 'N', 'Z'}, supplyStacks.PeekAtStack(1));
        Assert.AreEqual(new[]{'C', 'M'}, supplyStacks.PeekAtStack(2));
        Assert.AreEqual(new[]{'P'}, supplyStacks.PeekAtStack(3));
        
        supplyStacks.Move(3, 1, 3);
        
        Assert.AreEqual(Array.Empty<char>(), supplyStacks.PeekAtStack(1));
        Assert.AreEqual(new[]{'C', 'M'}, supplyStacks.PeekAtStack(2));
        Assert.AreEqual(new[]{'D', 'N', 'Z', 'P'}, supplyStacks.PeekAtStack(3));
        
        supplyStacks.Move(2, 2, 1);
        
        Assert.AreEqual(new[]{'C', 'M'}, supplyStacks.PeekAtStack(1));
        Assert.AreEqual(Array.Empty<char>(), supplyStacks.PeekAtStack(2));
        Assert.AreEqual(new[]{'D', 'N', 'Z', 'P'}, supplyStacks.PeekAtStack(3));
        
        supplyStacks.Move(1, 1, 2);
        
        Assert.AreEqual(new[]{'M'}, supplyStacks.PeekAtStack(1));
        Assert.AreEqual(new[]{'C'}, supplyStacks.PeekAtStack(2));
        Assert.AreEqual(new[]{'D', 'N', 'Z', 'P'}, supplyStacks.PeekAtStack(3));
        
        Assert.AreEqual("MCD", supplyStacks.PeekAtTopBoxes());
    }


    [Test]
    public void Example2() {
        var supplyStacks = new SupplyStacks
        {
            CraneMover9001 = true,
        };
        Assert.AreEqual("MCD", supplyStacks.ParseAndExecute(File.ReadAllLines(@"05\example.txt")));
    }
    
    [Test]
    public void Puzzle2() {
        var supplyStacks = new SupplyStacks
        {
            CraneMover9001 = true,
        };
        var result = supplyStacks.ParseAndExecute(File.ReadAllLines(@"05\input.txt"));
        Assert.AreEqual("MHQTLJRLB", result);
        Assert.Pass("Puzzle 2: " + result);
    }
}