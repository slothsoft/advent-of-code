using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC.day15;

public class WarehouseWoesTest {
    [Test]
    public void Example1A_Steps() {
        var example = new WarehouseWoes(File.ReadAllLines(@"15\example1.txt"));

        var expected = @"########
#..O.O.#
##@.O..#
#...O..#
#.#.O..#
#...O..#
#......#
########";
        Assert.AreEqual(expected, example.Stringify());  
        
        example.Move(WarehouseWoes.AllDirections['<']);
        expected = @"########
#..O.O.#
##@.O..#
#...O..#
#.#.O..#
#...O..#
#......#
########";
        Assert.AreEqual(expected, example.Stringify());  
        
        example.Move(WarehouseWoes.AllDirections['^']);
        expected = @"########
#.@O.O.#
##..O..#
#...O..#
#.#.O..#
#...O..#
#......#
########";
        Assert.AreEqual(expected, example.Stringify());  
        
        example.Move(WarehouseWoes.AllDirections['^']);
        expected = @"########
#.@O.O.#
##..O..#
#...O..#
#.#.O..#
#...O..#
#......#
########";
        Assert.AreEqual(expected, example.Stringify());  
        
        example.Move(WarehouseWoes.AllDirections['>']);
        expected = @"########
#..@OO.#
##..O..#
#...O..#
#.#.O..#
#...O..#
#......#
########";
        Assert.AreEqual(expected, example.Stringify());  
        
        example.Move(WarehouseWoes.AllDirections['>']);
        expected = @"########
#...@OO#
##..O..#
#...O..#
#.#.O..#
#...O..#
#......#
########";
        Assert.AreEqual(expected, example.Stringify());  
        
        example.Move(WarehouseWoes.AllDirections['>']);
        expected = @"########
#...@OO#
##..O..#
#...O..#
#.#.O..#
#...O..#
#......#
########";
        Assert.AreEqual(expected, example.Stringify());  
        
        example.Move(WarehouseWoes.AllDirections['v']);
        expected = @"########
#....OO#
##..@..#
#...O..#
#.#.O..#
#...O..#
#...O..#
########";
        Assert.AreEqual(expected, example.Stringify());  
        
        example.Move(WarehouseWoes.AllDirections['v']);
        expected = @"########
#....OO#
##..@..#
#...O..#
#.#.O..#
#...O..#
#...O..#
########";
        Assert.AreEqual(expected, example.Stringify());  
    }

    [Test]
    public void Example1A_MoveAll() {
        var example = new WarehouseWoes(File.ReadAllLines(@"15\example1.txt"));
        example.MoveAll();
        
        var expected = @"########
#....OO#
##.....#
#.....O#
#.#O@..#
#...O..#
#...O..#
########";
        Assert.AreEqual(expected, example.Stringify());  
    }

    [Test]
    public void Example1A() {
        var example = new WarehouseWoes(File.ReadAllLines(@"15\example1.txt"));
        example.MoveAll();
        
        Assert.AreEqual(2028, example.CalculateGpsCoords().Sum());  
    }

    [Test]
    public void Example1B() {
        var example = new WarehouseWoes(File.ReadAllLines(@"15\example2.txt"));
        
        Assert.AreEqual(10092,  example.MoveAndCalculateGpsCoordsSum());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new WarehouseWoes(File.ReadAllLines(@"15\input.txt"));
        
        Assert.AreEqual(1511865,  puzzle.MoveAndCalculateGpsCoordsSum());  
    }

    [Test]
    public void Example2A() {
        var example = new WarehouseWoes(File.ReadAllLines(@"15\example1.txt"));
        
        Assert.AreEqual(7,  example.MoveAndCalculateGpsCoordsSum());   
    }
    
    [Test]
    public void Example2B() {
        var example = new WarehouseWoes(File.ReadAllLines(@"15\example2.txt"));
        
        Assert.AreEqual(7,  example.MoveAndCalculateGpsCoordsSum());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new WarehouseWoes(File.ReadAllLines(@"15\input.txt"));
        
        Assert.AreEqual(7,  puzzle.MoveAndCalculateGpsCoordsSum());  
    }
}
