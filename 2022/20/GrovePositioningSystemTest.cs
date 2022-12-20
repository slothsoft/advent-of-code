using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC._20;

public class GrovePositioningSystemTest {
    [Test]
    public void Example1MoveNumberA() {
        var array = new[] {4, 5, 6, 1, 7, 8, 9};

        var circularList = new CircularList(array);
        circularList.MoveNumber(1);

        Assert.AreEqual(new[] {4, 5, 6, 7, 1, 8, 9}, circularList.ToArray(4));
    }

    [Test]
    public void Example1MoveNumberB() {
        var array = new[] {4, -2, 5, 6, 7, 8, 9};

        var circularList = new CircularList(array);
        circularList.MoveNumber(-2);

        Assert.AreEqual(new[] {4, 5, 6, 7, 8, -2, 9}, circularList.ToArray(4));
    }

    [Test]
    public void Example1MoveNumberC() {
        var array = new[] {1, 2, 3, -11, -3, 0, 4};

        var circularList = new CircularList(array);
        circularList.MoveNumber(-11);

        Assert.AreEqual(new[] {1, 2, 3, -3, -11, 0, 4}, circularList.ToArray(1));
    }

    [Test]
    public void Example1MoveNumberD() {
        var array = new[] {1, 2, 3, 12, -3, 0, 4};

        var circularList = new CircularList(array);
        circularList.MoveNumber(12);

        Assert.AreEqual(new[] {1, 2, 3, 12, -3, 0, 4}, circularList.ToArray(1));
    }

    [Test]
    public void Example1MixFileManually() {
        var array = new[] {1, 2, -3, 3, -2, 0, 4};

        var circularList = new CircularList(array);
        circularList.MoveNumber(1);
        Assert.AreEqual(new[] {2, 1, -3, 3, -2, 0, 4}, circularList.ToArray(2));

        circularList.MoveNumber(2);
        Assert.AreEqual(new[] {1, -3, 2, 3, -2, 0, 4}, circularList.ToArray(1));

        circularList.MoveNumber(-3);
        Assert.AreEqual(new[] {1, 2, 3, -2, -3, 0, 4}, circularList.ToArray(1));

        circularList.MoveNumber(3);
        Assert.AreEqual(new[] {1, 2, -2, -3, 0, 3, 4}, circularList.ToArray(1));

        circularList.MoveNumber(-2);
        Assert.AreEqual(new[] {1, 2, -3, 0, 3, 4, -2}, circularList.ToArray(1));

        circularList.MoveNumber(0);
        Assert.AreEqual(new[] {1, 2, -3, 0, 3, 4, -2}, circularList.ToArray(1));

        circularList.MoveNumber(4);
        Assert.AreEqual(new[] {1, 2, -3, 4, 0, 3, -2}, circularList.ToArray(1));
    }

    [Test]
    public void Example1MixFile() {
        var system = new GrovePositioningSystem(File.ReadAllLines(@"20\example.txt"));

        Assert.AreEqual(new[] {1, 2, -3, 4, 0, 3, -2}, system.MixFile(1));
    }

    [Test]
    public void Example1() {
        var system = new GrovePositioningSystem(File.ReadAllLines(@"20\example.txt"));

        var coordinates = system.CalculateGrooveCoordinates();
        Assert.AreEqual(4, coordinates[0]);
        Assert.AreEqual(-3, coordinates[1]);
        Assert.AreEqual(2, coordinates[2]);

        Assert.AreEqual(3, coordinates.Sum());
    }

    [Test]
    public void Puzzle1() {
        var system = new GrovePositioningSystem(File.ReadAllLines(@"20\input.txt"));

        var result = system.CalculateGrooveCoordinates().Sum();
        //Assert.AreEqual(14116, result); // FIXME: 14116 too high, -1165 too low
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    public void Example2() {
    }

    [Test]
    public void Puzzle2() {
    }
}