using System.IO;
using NUnit.Framework;

namespace AoC._22;

public class MonkeyMapTest {
    [Test]
    public void Example1Fields() {
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\example.txt"));

        Assert.AreEqual(@"        >..#    
        .#..    
        #...    
        ....    
...#.......#    
........#...    
..#....#....    
..........#.    
        ...#....
        .....#..
        .#......
        ......#.
", monkeyMap.Fields.ToString());
    }

    [Test]
    public void Example1Steps() {
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\example.txt"));

        monkeyMap.Move("10R");
        Assert.AreEqual(@"        ..v#    
        .#..    
        #...    
        ....    
...#.......#    
........#...    
..#....#....    
..........#.    
        ...#....
        .....#..
        .#......
        ......#.
", monkeyMap.Fields.ToString());

        monkeyMap.Move("5L");
        Assert.AreEqual(@"        ...#    
        .#..    
        #...    
        ....    
...#.......#    
........#.>.    
..#....#....    
..........#.    
        ...#....
        .....#..
        .#......
        ......#.
", monkeyMap.Fields.ToString());

        monkeyMap.Move("5R");
        Assert.AreEqual(@"        ...#    
        .#..    
        #...    
        ....    
...#.......#    
...v....#...    
..#....#....    
..........#.    
        ...#....
        .....#..
        .#......
        ......#.
", monkeyMap.Fields.ToString());

        monkeyMap.Move("10L");
        Assert.AreEqual(@"        ...#    
        .#..    
        #...    
        ....    
...#.......#    
........#...    
..#....#....    
...>......#.    
        ...#....
        .....#..
        .#......
        ......#.
", monkeyMap.Fields.ToString());

        monkeyMap.Move("4R");
        Assert.AreEqual(@"        ...#    
        .#..    
        #...    
        ....    
...#.......#    
........#...    
..#....#....    
.......v..#.    
        ...#....
        .....#..
        .#......
        ......#.
", monkeyMap.Fields.ToString());

        monkeyMap.Move("5L5");
        Assert.AreEqual(@"        ...#    
        .#..    
        #...    
        ....    
...#.......#    
.......>#...    
..#....#....    
..........#.    
        ...#....
        .....#..
        .#......
        ......#.
", monkeyMap.Fields.ToString());
    }

    [Test]
    public void Example1() {
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\example.txt"));
        monkeyMap.Move();

        Assert.AreEqual(6032, monkeyMap.CalculatePassword());
    }

    [Test]
    public void Puzzle1() {
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\input.txt"));
        monkeyMap.Move();

        var result = monkeyMap.CalculatePassword();
        Assert.AreEqual(67390, result);
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    public void Example2Steps() {
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\example.txt"), true);

        monkeyMap.Move("10R");
        Assert.AreEqual(@"        ..v#    
        .#..    
        #...    
        ....    
...#.......#    
........#...    
..#....#....    
..........#.    
        ...#....
        .....#..
        .#......
        ......#.
", monkeyMap.Fields.ToString());

        monkeyMap.Move("5L");
        Assert.AreEqual(@"        ...#    
        .#..    
        #...    
        ....    
...#.......#    
........#.>.    
..#....#....    
..........#.    
        ...#....
        .....#..
        .#......
        ......#.
", monkeyMap.Fields.ToString());

        monkeyMap.Move("5R");
        Assert.AreEqual(@"        ...#    
        .#..    
        #...    
        ....    
...#.......#    
........#...    
..#....#....    
..........#.    
        ...#....
        .....#..
        .#....<.
        ......#.
", monkeyMap.Fields.ToString());

        monkeyMap.Move("10L");
        Assert.AreEqual(@"        ...#    
        .#..    
        #...    
        ....    
...#.......#    
........#...    
..#....#....    
..........#.    
        ...#....
        .....#..
        .#v.....
        ......#.
", monkeyMap.Fields.ToString());

        monkeyMap.Move("4R");
        Assert.AreEqual(@"        ...#    
        .#..    
        #...    
        ....    
...#.......#    
.>......#...    
..#....#....    
..........#.    
        ...#....
        .....#..
        .#......
        ......#.
", monkeyMap.Fields.ToString());

        monkeyMap.Move("5L5");
        Assert.AreEqual(@"        ...#    
        .#..    
        #...    
        ....    
...#..^....#    
........#...    
..#....#....    
..........#.    
        ...#....
        .....#..
        .#......
        ......#.
", monkeyMap.Fields.ToString());
    }

    [Test]
    // Quadrant 3|1
    [TestCase(11, 5, MonkeyMap.Facing.Right, 14, 8, MonkeyMap.Facing.Down)]
    [TestCase(14, 8, MonkeyMap.Facing.Up, 11, 5, MonkeyMap.Facing.Left)]
    // Quadrant 0|2 respectively 2|3
    [TestCase(2, 7, MonkeyMap.Facing.Down, 9, 11, MonkeyMap.Facing.Up)]
    [TestCase(9, 11, MonkeyMap.Facing.Down, 2, 7, MonkeyMap.Facing.Up)]
    // Quadrant 1|0
    [TestCase(6, 4, MonkeyMap.Facing.Up, 6, 4, MonkeyMap.Facing.Up)] // there is a wall in the way xD
    [TestCase(8, 2, MonkeyMap.Facing.Left, 6, 4, MonkeyMap.Facing.Down)]
    [TestCase(7, 4, MonkeyMap.Facing.Up, 8, 3, MonkeyMap.Facing.Right)]
    [TestCase(8, 3, MonkeyMap.Facing.Left, 7, 4, MonkeyMap.Facing.Down)]
    public void Example2Transition(int startX, int startY, MonkeyMap.Facing startFacing, int expectedX, int expectedY, MonkeyMap.Facing expectedFacing) {
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\example.txt"), true);
        monkeyMap.Fields.ChangePosition(startX, startY, startFacing);
        monkeyMap.Move("1");
        
        Assert.AreEqual(expectedX, monkeyMap.Fields.X);
        Assert.AreEqual(expectedY, monkeyMap.Fields.Y);
        Assert.AreEqual(expectedFacing, monkeyMap.Fields.Facing);
    }

    [Test]
    public void Example2() {
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\example.txt"), true);
        monkeyMap.Move();

        Assert.AreEqual(5031, monkeyMap.CalculatePassword());
    }

    [Test]
    public void Puzzle2() {
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\input.txt"), true);
        monkeyMap.Move();

        var result = monkeyMap.CalculatePassword();
        // Assert.AreEqual(5031, result);
        Assert.Pass("Puzzle 2: " + result);
    }
}