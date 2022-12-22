using System;
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
    public void Example1AllConnected() {
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\example.txt"));
        foreach (var entry in monkeyMap.fields.squares) {
            Assert.NotNull(entry.Value.Up, $"Square {entry.Key} does not have a neighbor for Up");
            Assert.NotNull(entry.Value.Down, $"Square {entry.Key} does not have a neighbor for Down");
            Assert.NotNull(entry.Value.Right, $"Square {entry.Key} does not have a neighbor for Right");
            Assert.NotNull(entry.Value.Left, $"Square {entry.Key} does not have a neighbor for Left");
        }
    }

    [Test]
    public void Example2AllConnected() {
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\example.txt"), true);
        foreach (var entry in monkeyMap.fields.squares) {
            Assert.NotNull(entry.Value.Up, $"Square {entry.Key} does not have a neighbor for Up");
            Assert.NotNull(entry.Value.Down, $"Square {entry.Key} does not have a neighbor for Down");
            Assert.NotNull(entry.Value.Right, $"Square {entry.Key} does not have a neighbor for Right");
            Assert.NotNull(entry.Value.Left, $"Square {entry.Key} does not have a neighbor for Left");
        }
    }

    [Test]
    public void Input1AllConnected() {
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\input.txt"));
        foreach (var entry in monkeyMap.fields.squares) {
            Assert.NotNull(entry.Value.Up, $"Square {entry.Key} does not have a neighbor for Up");
            Assert.NotNull(entry.Value.Down, $"Square {entry.Key} does not have a neighbor for Down");
            Assert.NotNull(entry.Value.Right, $"Square {entry.Key} does not have a neighbor for Right");
            Assert.NotNull(entry.Value.Left, $"Square {entry.Key} does not have a neighbor for Left");
        }
    }

    [Test]
    public void Example1Connections() {
        // ... ... 2-0 ...
        // 0-1 1-1 2-1 ...
        // ... ... 2-2 3-2
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\example.txt"));
        foreach (var square in monkeyMap.fields.squares.Values) {
            var id = $"{square.quadrantX}-{square.quadrantY}";
            switch (id) {
                case "2-0":
                    Assert.AreEqual("-> 2-2", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 2-0", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 2-0", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 2-1", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                case "0-1":
                    Assert.AreEqual("-> 0-1", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 2-1", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-1", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 0-1", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                case "1-1":
                    Assert.AreEqual("-> 1-1", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 0-1", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 2-1", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-1", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                case "2-1":
                    Assert.AreEqual("-> 2-0", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-1", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 0-1", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 2-2", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                case "2-2":
                    Assert.AreEqual("-> 2-1", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 3-2", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 3-2", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 2-0", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                case "3-2":
                    Assert.AreEqual("-> 3-2", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 2-2", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 2-2", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 3-2", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                default:
                    throw new ArgumentException($"Square {id} is not known");
            }
        }
    }

    [Test]
    public void Input1Connections() {
        // ... 1-0 2-0
        // ... 1-1 ...
        // 0-2 1-2 ...
        // 0-3 ... ...
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\input.txt"));
        foreach (var square in monkeyMap.fields.squares.Values) {
            var id = $"{square.quadrantX}-{square.quadrantY}";
            switch (id) {
                case "1-0":
                    Assert.AreEqual("-> 1-2", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 2-0", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 2-0", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-1", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                case "2-0":
                    Assert.AreEqual("-> 2-0", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-0", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-0", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 2-0", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                case "1-1":
                    Assert.AreEqual("-> 1-0", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-1", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-1", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-2", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                case "0-2":
                    Assert.AreEqual("-> 0-3", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-2", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-2", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 0-3", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                case "1-2":
                    Assert.AreEqual("-> 1-1", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 0-2", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 0-2", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-0", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                case "0-3":
                    Assert.AreEqual("-> 0-2", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 0-3", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 0-3", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 0-2", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                default:
                    throw new ArgumentException($"Square {id} is not known");
            }
        }
    }
    
    [Test]
    public void Input2Connections() {
        // ... 1-0 2-0
        // ... 1-1 ...
        // 0-2 1-2 ...
        // 0-3 ... ...
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\input.txt"));
        foreach (var square in monkeyMap.fields.squares.Values) {
            var id = $"{square.quadrantX}-{square.quadrantY}";
            switch (id) {
                case "1-0":
                    Assert.AreEqual("-> 1-2", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 2-0", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 2-0", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-1", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                case "2-0":
                    Assert.AreEqual("-> 2-0", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-0", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-0", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 2-0", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                case "1-1":
                    Assert.AreEqual("-> 1-0", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-1", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-1", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-2", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                case "0-2":
                    Assert.AreEqual("-> 0-3", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-2", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-2", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 0-3", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                case "1-2":
                    Assert.AreEqual("-> 1-1", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 0-2", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 0-2", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 1-0", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                case "0-3":
                    Assert.AreEqual("-> 0-2", square.Up!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 0-3", square.Left!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 0-3", square.Right!.ToString(), $"Square {id} has wrong neighbor");
                    Assert.AreEqual("-> 0-2", square.Down!.ToString(), $"Square {id} has wrong neighbor");
                    break;
                default:
                    throw new ArgumentException($"Square {id} is not known");
            }
        }
    }

    [Test]
    public void Input2AllConnected() {
        var monkeyMap = new MonkeyMap(File.ReadAllLines(@"22\input.txt"), true);
        foreach (var entry in monkeyMap.fields.squares) {
            Assert.NotNull(entry.Value.Up, $"Square {entry.Key} does not have a neighbor for Up");
            Assert.NotNull(entry.Value.Down, $"Square {entry.Key} does not have a neighbor for Down");
            Assert.NotNull(entry.Value.Right, $"Square {entry.Key} does not have a neighbor for Right");
            Assert.NotNull(entry.Value.Left, $"Square {entry.Key} does not have a neighbor for Left");
        }
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