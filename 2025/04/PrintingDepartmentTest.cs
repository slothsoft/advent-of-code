using System.IO;
using NUnit.Framework;

namespace AoC.day4;

public class PrintingDepartmentTest {
    [Test]
    public void Example1() {
        var example = new PrintingDepartment(File.ReadAllLines(@"04\example.txt"));
        
        Assert.AreEqual(13,  example.CalculateAccessibleRollsCount());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new PrintingDepartment(File.ReadAllLines(@"04\input.txt"));
        
        Assert.AreEqual(1451,  puzzle.CalculateAccessibleRollsCount());  
    }

    [Test]
    public void Example2() {
        var example = new PrintingDepartment(File.ReadAllLines(@"04\example.txt"));
        
        Assert.AreEqual(43,  example.CalculateRemoveableRollsCount());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new PrintingDepartment(File.ReadAllLines(@"04\input.txt"));
        
        Assert.AreEqual(8701,  puzzle.CalculateRemoveableRollsCount());  
    }
}
