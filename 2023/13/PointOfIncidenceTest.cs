using System.IO;
using NUnit.Framework;

namespace AoC;

public class PointOfIncidenceTest {
    [Test]
    [TestCase(@"13\example.txt", 405)]
    [TestCase(@"13\example1.txt", 5)]
    [TestCase(@"13\example2.txt", 400)]
    [TestCase(@"13\example_from_input1.txt", 1500)]
    [TestCase(@"13\example_from_input2.txt", 10)]
    [TestCase(@"13\example_from_input3.txt", 2)]
    public void Example1(string fileName, int expectedMirrorNumber) {
        var example = new PointOfIncidence(File.ReadAllLines(fileName));

        Assert.AreEqual(expectedMirrorNumber, example.CalculateMirrorNumber());
    }

    [Test]
    public void Puzzle1() {
        var example = new PointOfIncidence(File.ReadAllLines(@"13\input.txt"));

        // 15679 is too low
        // 33317, 33297 is too high
        Assert.AreEqual(30487, example.CalculateMirrorNumber());
    }

    [Test]
    [TestCase(@"13\example.txt", 400)]
    [TestCase(@"13\example1.txt", 300)]
    [TestCase(@"13\example2.txt", 100)]
    public void Example2(string fileName, int expectedMirrorNumber) {
        var example = new PointOfIncidence(File.ReadAllLines(fileName)) {
            AllowedDifferences = 1
        };

        Assert.AreEqual(expectedMirrorNumber, example.CalculateMirrorNumber());
    }

    [Test]
    public void Puzzle2() {
        var example = new PointOfIncidence(File.ReadAllLines(@"13\input.txt")) {
            AllowedDifferences = 1
        };

        Assert.AreEqual(31954, example.CalculateMirrorNumber());
    }
}