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
", monkeyMap.fields.ToString());
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
", monkeyMap.fields.ToString());

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
", monkeyMap.fields.ToString());

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
", monkeyMap.fields.ToString());

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
", monkeyMap.fields.ToString());

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
", monkeyMap.fields.ToString());

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
", monkeyMap.fields.ToString());
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
", monkeyMap.fields.ToString());

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
", monkeyMap.fields.ToString());

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
", monkeyMap.fields.ToString());

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
", monkeyMap.fields.ToString());

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
", monkeyMap.fields.ToString());

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
", monkeyMap.fields.ToString());
    }

    [Test]
    // Quadrant -1-1 respectively 3-3
    [TestCase(13, 11, MonkeyMap.Facing.Down, 0, 6, MonkeyMap.Facing.Right)]
    [TestCase(0, 6, MonkeyMap.Facing.Left, 13, 11, MonkeyMap.Facing.Up)]
    // Quadrant 0-0 respectively 2-(-1)
    [TestCase(2, 4, MonkeyMap.Facing.Up, 9, 0, MonkeyMap.Facing.Down)]
    [TestCase(9, 0, MonkeyMap.Facing.Up, 2, 4, MonkeyMap.Facing.Down)]
    // Quadrant 0-2 respectively 2-3
    [TestCase(2, 7, MonkeyMap.Facing.Down, 9, 11, MonkeyMap.Facing.Up)]
    [TestCase(9, 11, MonkeyMap.Facing.Down, 2, 7, MonkeyMap.Facing.Up)]
    // Quadrant 1-0
    [TestCase(6, 4, MonkeyMap.Facing.Up, 6, 4, MonkeyMap.Facing.Up)] // there is a wall in the way xD
    [TestCase(8, 2, MonkeyMap.Facing.Left, 6, 4, MonkeyMap.Facing.Down)]
    [TestCase(7, 4, MonkeyMap.Facing.Up, 8, 3, MonkeyMap.Facing.Right)]
    [TestCase(8, 3, MonkeyMap.Facing.Left, 7, 4, MonkeyMap.Facing.Down)]
    // Quadrant 1-2
    [TestCase(6, 7, MonkeyMap.Facing.Down, 8, 9, MonkeyMap.Facing.Right)]
    [TestCase(8, 9, MonkeyMap.Facing.Left, 6, 7, MonkeyMap.Facing.Up)]
    // Quadrant 3-0 respectively 4-2
    [TestCase(15, 9, MonkeyMap.Facing.Right, 11, 2, MonkeyMap.Facing.Left)]
    [TestCase(11, 2, MonkeyMap.Facing.Right, 15, 9, MonkeyMap.Facing.Left)]
    // Quadrant 3-1
    [TestCase(11, 5, MonkeyMap.Facing.Right, 14, 8, MonkeyMap.Facing.Down)]
    [TestCase(14, 8, MonkeyMap.Facing.Up, 11, 5, MonkeyMap.Facing.Left)]    
    public void Example2Transition(int startX, int startY, MonkeyMap.Facing startFacing, int expectedX, int expectedY, MonkeyMap.Facing expectedFacing) {
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\example.txt"), true);
        monkeyMap.fields.ChangePosition(startX, startY, startFacing);
        monkeyMap.Move("1");
        
        Assert.AreEqual(expectedX, monkeyMap.fields.x);
        Assert.AreEqual(expectedY, monkeyMap.fields.y);
        Assert.AreEqual(expectedFacing, monkeyMap.fields.facing);
    }

    [Test]
    public void Example2() {
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\example.txt"), true);
        monkeyMap.Move();

        Assert.AreEqual(5031, monkeyMap.CalculatePassword());
    }

    [Test]
    // Quadrant 1-(-1) respectively -1-3
    [TestCase(0, 150, MonkeyMap.Facing.Left, 50, 0, MonkeyMap.Facing.Down)]
    [TestCase(50, 0, MonkeyMap.Facing.Up, 0, 150, MonkeyMap.Facing.Right)]
    // Quadrant 0-0 respectively -1-2
    [TestCase(50, 2, MonkeyMap.Facing.Left, 0, 147, MonkeyMap.Facing.Right)]
    [TestCase(0, 147, MonkeyMap.Facing.Left, 50, 2, MonkeyMap.Facing.Right)]
    // Quadrant 2-(-1) respectively 0-4
    [TestCase(105, 0, MonkeyMap.Facing.Up, 5, 199, MonkeyMap.Facing.Up)]
    [TestCase(5, 199, MonkeyMap.Facing.Down, 105, 0, MonkeyMap.Facing.Down)]
    // Quadrant 3-0 respectively 2-2
    [TestCase(149, 10, MonkeyMap.Facing.Right, 99, 139, MonkeyMap.Facing.Left)]
    [TestCase(99, 140, MonkeyMap.Facing.Right, 149, 9, MonkeyMap.Facing.Left)]
    // Quadrant 0-1
    [TestCase(20, 100, MonkeyMap.Facing.Up, 50, 70, MonkeyMap.Facing.Right)]
    [TestCase(50, 70, MonkeyMap.Facing.Left, 20, 100, MonkeyMap.Facing.Down)]
    // Quadrant 2-1
    [TestCase(99, 60, MonkeyMap.Facing.Right, 110, 49, MonkeyMap.Facing.Up)]
    [TestCase(110, 49, MonkeyMap.Facing.Down, 99, 60, MonkeyMap.Facing.Left)]
    // Quadrant 1-3
    [TestCase(49, 160, MonkeyMap.Facing.Right, 60, 149, MonkeyMap.Facing.Up)]
    [TestCase(60, 149, MonkeyMap.Facing.Down, 49, 160, MonkeyMap.Facing.Left)]
    public void Input2Transition(int startX, int startY, MonkeyMap.Facing startFacing, int expectedX, int expectedY, MonkeyMap.Facing expectedFacing) {
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\input.txt"), true);
        monkeyMap.fields.ChangePosition(startX, startY, startFacing);
        monkeyMap.Move("1");
        
        Assert.AreEqual(expectedX, monkeyMap.fields.x);
        Assert.AreEqual(expectedY, monkeyMap.fields.y);
        Assert.AreEqual(expectedFacing, monkeyMap.fields.facing);
    }

    [Test]
    public void Puzzle2() {
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\input.txt"), true);
        monkeyMap.Move();

        var result = monkeyMap.CalculatePassword();
        Assert.AreEqual(95291, result);
        Assert.Pass("Puzzle 2: " + result);
    }
}