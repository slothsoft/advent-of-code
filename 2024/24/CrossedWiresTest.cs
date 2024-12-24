using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC.day24;

public class CrossedWiresTest {
    [Test]
    public void Example1A_Values() {
        var example = new CrossedWires(File.ReadAllLines(@"24\example1.txt"));

        var values = example.CalculateOutputValues();
        Assert.AreEqual(false,  values["z00"]);       
        Assert.AreEqual(false,  values["z01"]);     
        Assert.AreEqual(true,  values["z02"]);         
    }
    
    [Test]
    public void Example1A_BinaryNumber() {
        var example = new CrossedWires(File.ReadAllLines(@"24\example1.txt"));

        Assert.AreEqual("100",  example.CalculateBinaryNumber());       
    }
    
    [Test]
    public void Example1A() {
        var example = new CrossedWires(File.ReadAllLines(@"24\example1.txt"));

        Assert.AreEqual(4L,  example.CalculateNumber());       
    }
    
    [Test]
    public void Example1B() {
        var example = new CrossedWires(File.ReadAllLines(@"24\example2.txt"));
        
        Assert.AreEqual(2024L,  example.CalculateNumber());   
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new CrossedWires(File.ReadAllLines(@"24\input.txt"));
        
        Assert.AreEqual(51_410_244_478_064,  puzzle.CalculateNumber());  
    }

    // example 2 won't work because it's not an addition
    
    [Test]
    public void Example2A() {
        var example = new CrossedWires(File.ReadAllLines(@"24\example1.txt"));

        foreach (var checkResult in example.CheckGates()) {
            Console.WriteLine(checkResult);
        }
    }

    [Test]
    public void Example2B() {
        var example = new CrossedWires(File.ReadAllLines(@"24\example2.txt"));

        foreach (var checkResult in example.CheckGates()) {
            Console.WriteLine(checkResult);
        }
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new CrossedWires(File.ReadAllLines(@"24\input.txt"));

        foreach (var checkResult in puzzle.CheckGates().Select(r => r.errorMessage)) {
            Console.WriteLine(checkResult);
        }
    }
}
