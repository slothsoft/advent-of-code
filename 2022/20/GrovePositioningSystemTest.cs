using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC._20;

public class GrovePositioningSystemTest {
    private const int DecryptionKey = 811_589_153;

    [Test]
    public void Example1MoveNumberA() {
        var array = new long[] {4, 5, 6, 1, 7, 8, 9};

        var circularList = new CircularList(array);
        circularList.MoveNumberAt(3);

        Assert.AreEqual(new[] {4, 5, 6, 7, 1, 8, 9}, circularList.ToArray(4));
    }

    [Test]
    public void Example1MoveNumberB() {
        var array = new long[] {4, -2, 5, 6, 7, 8, 9};

        var circularList = new CircularList(array);
        circularList.MoveNumberAt(1);

        Assert.AreEqual(new[] {4, 5, 6, 7, 8, -2, 9}, circularList.ToArray(4));
    }

    [Test]
    public void Example1MoveNumberC() {
        var array = new long[] {1, 2, 3, -11, -3, 0, 4};

        var circularList = new CircularList(array);
        circularList.MoveNumberAt(3);

        Assert.AreEqual(new[] {1, 2, 3, -3, -11, 0, 4}, circularList.ToArray(1));
    }

    [Test]
    public void Example1MoveNumberD() {
        var array = new long[] {1, 2, 3, 12, -3, 0, 4};

        var circularList = new CircularList(array);
        circularList.MoveNumberAt(3);

        Assert.AreEqual(new[] {1, 2, 3, 12, -3, 0, 4}, circularList.ToArray(1));
    }

    [Test]
    public void Example1MoveNumberE() {
        var array = new long[] {1, 2, 3, 10, 4, 5, 10};

        var circularList = new CircularList(array);
        circularList.MoveNumberAt(3);

        Assert.AreEqual(new[] {1, 10, 2, 3, 4, 5, 10}, circularList.ToArray(1));

        circularList.MoveNumberAt(6);

        Assert.AreEqual(new[] {1, 10, 2, 3, 10, 4, 5}, circularList.ToArray(1));
    }

    [Test]
    public void Example1MixFileManually() {
        var array = new long[] {1, 2, -3, 3, -2, 0, 4};

        var circularList = new CircularList(array);
        circularList.MoveNumberAt(0);
        Assert.AreEqual(new[] {2, 1, -3, 3, -2, 0, 4}, circularList.ToArray(2));

        circularList.MoveNumberAt(1);
        Assert.AreEqual(new[] {1, -3, 2, 3, -2, 0, 4}, circularList.ToArray(1));

        circularList.MoveNumberAt(2);
        Assert.AreEqual(new[] {1, 2, 3, -2, -3, 0, 4}, circularList.ToArray(1));

        circularList.MoveNumberAt(3);
        Assert.AreEqual(new[] {1, 2, -2, -3, 0, 3, 4}, circularList.ToArray(1));

        circularList.MoveNumberAt(4);
        Assert.AreEqual(new[] {1, 2, -3, 0, 3, 4, -2}, circularList.ToArray(1));

        circularList.MoveNumberAt(5);
        Assert.AreEqual(new[] {1, 2, -3, 0, 3, 4, -2}, circularList.ToArray(1));

        circularList.MoveNumberAt(6);
        Assert.AreEqual(new[] {1, 2, -3, 4, 0, 3, -2}, circularList.ToArray(1));
    }

    [Test]
    public void Example1MixFile() {
        var system = new GrovePositioningSystem(File.ReadAllLines(@"20\example.txt"));

        Assert.AreEqual(new[] {1, 2, -3, 4, 0, 3, -2}, system.MixFile(1));
    }

    [Test]
    public void Example1MixFile2() {
        var system = new GrovePositioningSystem(new long[] {7, 8, 3, 3, -8, 0, 10});

        Assert.AreEqual(new[] {7, 8, 3, 10, 0, 3, -8}, system.MixFile(7));
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
        Assert.AreEqual(2215, result);
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    [TestCase(0, new[] {0, 3246356612, 811589153, 1623178306, -2434767459, 2434767459, -1623178306})]
    [TestCase(1, new[] {0, -2434767459, 3246356612, -1623178306, 2434767459, 1623178306, 811589153})]
    [TestCase(2, new[] {0, 2434767459, 1623178306, 3246356612, -2434767459, -1623178306, 811589153})]
    [TestCase(3, new[] {0, 811589153, 2434767459, 3246356612, 1623178306, -1623178306, -2434767459})]
    [TestCase(4, new[] {0, 1623178306, -2434767459, 811589153, 2434767459, 3246356612, -1623178306})]
    [TestCase(5, new[] {0, 811589153, -1623178306, 1623178306, -2434767459, 3246356612, 2434767459})]
    [TestCase(6, new[] {0, 811589153, -1623178306, 3246356612, -2434767459, 1623178306, 2434767459})]
    [TestCase(7, new[] {0, -2434767459, 2434767459, 1623178306, -1623178306, 811589153, 3246356612})]
    [TestCase(8, new[] {0, 1623178306, 3246356612, 811589153, -2434767459, 2434767459, -1623178306})]
    [TestCase(9, new[] {0, 811589153, 1623178306, -2434767459, 3246356612, 2434767459, -1623178306})]
    [TestCase(10, new[] {0, -2434767459, 1623178306, 3246356612, -1623178306, 2434767459, 811589153})]
    public void Example2InSteps(int mixCount, long[] expectedOutput) {
        var system = new GrovePositioningSystem(File.ReadAllLines(@"20\example.txt")) {
            DecryptionKey = DecryptionKey,
            MixCount = mixCount,
        };
        var actualOutput = system.MixFileAsCircularList().ToArray(0);
        Assert.AreEqual(expectedOutput, actualOutput);
    }
    
    [Test]
    public void Example2() {
        var system = new GrovePositioningSystem(File.ReadAllLines(@"20\example.txt")) {
            DecryptionKey = DecryptionKey,
            MixCount = 10,
        };

        var coordinates = system.CalculateGrooveCoordinates();
        Assert.AreEqual(811_589_153, coordinates[0]);
        Assert.AreEqual(2_434_767_459, coordinates[1]);
        Assert.AreEqual(-1_623_178_306, coordinates[2]);

        Assert.AreEqual(1_623_178_306, coordinates.Sum());
    }

    [Test]
    public void Puzzle2() {
        var system = new GrovePositioningSystem(File.ReadAllLines(@"20\input.txt")) {
            DecryptionKey = 811589153,
            MixCount = 10,
        };

        var result = system.CalculateGrooveCoordinates().Sum();
        Assert.AreEqual(8_927_480_683, result);
        Assert.Pass("Puzzle 2: " + result);
    }
}