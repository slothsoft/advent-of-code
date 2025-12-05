using System.IO;
using NUnit.Framework;

namespace AoC.day5;

public class CafeteriaTest {
    [Test]
    public void Example1() {
        var example = new Cafeteria(File.ReadAllLines(@"05\example.txt"));
        
        Assert.AreEqual(3,  example.CalculateFreshAvailableIngredientsCount());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new Cafeteria(File.ReadAllLines(@"05\input.txt"));
        
        Assert.AreEqual(798,  puzzle.CalculateFreshAvailableIngredientsCount());  
    }

    [Test]
    public void Example2() {
        var example = new Cafeteria(File.ReadAllLines(@"05\example.txt"));
        
        Assert.AreEqual(14,  example.CalculateFreshIngredientsCount());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new Cafeteria(File.ReadAllLines(@"05\input.txt"));
        
        Assert.AreEqual(7,  puzzle.CalculateFreshIngredientsCount());  
    }
}
