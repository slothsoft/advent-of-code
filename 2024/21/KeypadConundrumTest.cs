using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC.day21;

public class KeypadConundrumTest {
    [Test]
    public void Example1_Step1() {
        AssertPathsEqual("<A^A>^^AvvvA", 
            KeypadConundrum.CalculateShortestPath(KeypadConundrum.numericKeypad, "029A"));
    }
    
    private void AssertPathsEqual(string expected, string actual) {
        if (expected == actual) {
            // everything is fine
            return;
        }
        Assert.AreEqual(expected.Length, actual.Length, $"No matter the order, the length should be equal.\n\texpected {expected}\n\tactual   {actual}");

        var expectedSplit = expected.Split("A");
        var actualSplit = actual.Split("A");
        Assert.AreEqual(expectedSplit.Length, actualSplit.Length, $"The number of activations should be equal.\n\texpected {expected}\n\tactual   {actual}");
        
        for (var i = 0; i < expectedSplit.Length; i++) {
            var expectedStep = string.Join(string.Empty, expectedSplit[i].Order());
            var actualStep = string.Join(string.Empty, actualSplit[i].Order());
            Assert.AreEqual(expectedStep, actualStep, $"The operations in step {i+1} should be the same, no matter the order.\n\texpected {expectedStep} ({expected})\n\tactual   {actualStep} ({actual})");
        }
    }
    
    [Test]
    public void Example1_Step2() {
        AssertPathsEqual("v<<A>>^A<A>AvA<^AA>A<vAAA>^A", 
            KeypadConundrum.CalculateShortestPath(KeypadConundrum.directionalKeypad,
            KeypadConundrum.CalculateShortestPath(KeypadConundrum.numericKeypad, "029A")));
    }

    [Test]
    public void Example1_Step3() {
        AssertPathsEqual("<vA<AA>>^AvAA<^A>A<v<A>>^AvA^A<vA>^A<v<A>^A>AAvA^A<v<A>A>^AAAvA<^A>A", 
            KeypadConundrum.CalculateShortestPath(KeypadConundrum.directionalKeypad,
            KeypadConundrum.CalculateShortestPath(KeypadConundrum.directionalKeypad,
            KeypadConundrum.CalculateShortestPath(KeypadConundrum.numericKeypad, "029A"))));
    }

    [Test]
    [TestCase("029A", 68 * 29)]
    [TestCase("980A", 60 * 980)]
    [TestCase("179A", 68 * 179)]
    [TestCase("456A", 64 * 456)]
    [TestCase("379A", 64 * 379)]
    public void Example1_Singles(string code, long expectedComplexity) {
        Assert.AreEqual(expectedComplexity, KeypadConundrum.CalculateComplexity(code));
    }
    
    [Test]
    public void Example1() {
        var example = new KeypadConundrum(File.ReadAllLines(@"21\example.txt"));

        Assert.AreEqual(7, example.CalculateComplexityOfInput());
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new KeypadConundrum(File.ReadAllLines(@"21\input.txt"));

        Assert.AreEqual(7, puzzle.CalculateComplexityOfInput());
    }

    [Test]
    public void Example2() {
        var example = new KeypadConundrum(File.ReadAllLines(@"21\example.txt"));

        Assert.AreEqual(7, example.CalculateComplexityOfInput());
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new KeypadConundrum(File.ReadAllLines(@"21\input.txt"));

        Assert.AreEqual(7, puzzle.CalculateComplexityOfInput());
    }
}