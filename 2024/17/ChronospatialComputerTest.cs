using System;
using System.IO;
using NUnit.Framework;

namespace AoC.day17;

public class ChronospatialComputerTest {
    private const int A = 4;
    private const int B = 5;
    private const int C = 6;
    
    [Test]
    public void Example1A() {
        var example = new ChronospatialComputer("2,6", c: 9);

        var register = example.RunProgram();
        Assert.AreEqual(1,  register.GetComboOperand(B)); 
        Assert.AreEqual(string.Empty,  register.Output);        
    }
    
    [Test]
    public void Example1B() {
        var example = new ChronospatialComputer("5,0,5,1,5,4", a: 10);

        Assert.AreEqual("0,1,2",  example.RunProgramReturnOutput());        
    }
    
    [Test]
    public void Example1C() {
        var example = new ChronospatialComputer("0,1,5,4,3,0", a: 2024);
        
        var register = example.RunProgram();
        Assert.AreEqual(0,  register.GetComboOperand(A)); 
        Assert.AreEqual("4,2,5,6,7,7,7,7,3,1,0",  register.Output);           
    }

    [Test]
    public void Example1D() {
        var example = new ChronospatialComputer("1,7", b: 29);
        
        var register = example.RunProgram();
        Assert.AreEqual(26,  register.GetComboOperand(B)); 
    }
    
    [Test]
    public void Example1E() {
        var example = new ChronospatialComputer("4,0", b: 2024, c: 43690);
        
        var register = example.RunProgram();
        Assert.AreEqual(44354,  register.GetComboOperand(B));   
    }
    
    [Test]
    public void Example1() {
        var example = new ChronospatialComputer(File.ReadAllLines(@"17\example.txt"));
        
        Assert.AreEqual("4,6,3,5,6,3,5,2,1,0",  example.RunProgramReturnOutput());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new ChronospatialComputer(File.ReadAllLines(@"17\input.txt"));
        
        Assert.AreEqual("4,1,5,3,1,5,3,5,7",  puzzle.RunProgramReturnOutput());  
    }

    [Test]
    public void Example2() {
        var example = new ChronospatialComputer(File.ReadAllLines(@"17\example.txt"));
        
        Assert.AreEqual(117_440,  example.FindAForSolution());   
    }
    
    [Test]
    public void Example2b() {
        var example = new ChronospatialComputer(File.ReadAllLines(@"17\example.txt"));

        for (var i = 0; i < 100; i++) {
            example.InitialRegister[0] = i;
            Console.WriteLine($"{i}   {example.RunProgramReturnOutput()}");
        }
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new ChronospatialComputer(File.ReadAllLines(@"17\input.txt"));
        
        
        for (var i = 0; i < 1000; i++) {
            puzzle.InitialRegister[0] = i;
            Console.WriteLine($"{i}   {puzzle.RunProgramReturnOutput()}");
        }
        
        // Assert.AreEqual(7,  puzzle.FindAForSolution());  
    }
    
    [Test]
    public void Puzzle2b() {
        var puzzle = new ChronospatialComputer(File.ReadAllLines(@"17\input.txt"));
        
        puzzle.InitialRegister[0] = 0;
        Assert.AreEqual(7,  puzzle.FindAForSolution());  
    }

}
