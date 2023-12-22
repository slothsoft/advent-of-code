using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace AoC.day19;

public class AplentyTest {
    [Test]
    public void Example1_ParseWorkflow() {
        var workflow = Aplenty.ParseWorkflow("ex{x>10:one,m<20:two,a>30:R,A}");

        Assert.NotNull(workflow);
        Assert.AreEqual("ex", workflow.DisplayName);
        Assert.AreEqual("one", workflow.RatePart(new Dictionary<string, int> {{Aplenty.VARIABLE_X, 11}}));
        Assert.AreEqual("two", workflow.RatePart(new Dictionary<string, int> {{Aplenty.VARIABLE_X, 10}, {Aplenty.VARIABLE_M, 19}}));
        Assert.AreEqual("R", workflow.RatePart(new Dictionary<string, int> {{Aplenty.VARIABLE_X, 10}, {Aplenty.VARIABLE_M, 20}, {Aplenty.VARIABLE_A, 31}}));
        Assert.AreEqual("A", workflow.RatePart(new Dictionary<string, int> {{Aplenty.VARIABLE_X, 10}, {Aplenty.VARIABLE_M, 20}, {Aplenty.VARIABLE_A, 30}}));
    }

    [Test]
    public void Example1_RatePart() {
        var example = new Aplenty(File.ReadAllLines(@"19\example.txt"));

        Assert.AreEqual(true, example.RatePart(example.Parts[0]));
        Assert.AreEqual(false, example.RatePart(example.Parts[1]));
        Assert.AreEqual(true, example.RatePart(example.Parts[2]));
        Assert.AreEqual(false, example.RatePart(example.Parts[3]));
        Assert.AreEqual(true, example.RatePart(example.Parts[4]));
    }

    [Test]
    public void Example1() {
        var example = new Aplenty(File.ReadAllLines(@"19\example.txt"));

        Assert.AreEqual(19114, example.CalculateRatingNumber());
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new Aplenty(File.ReadAllLines(@"19\input.txt"));

        Assert.AreEqual(449531, puzzle.CalculateRatingNumber());
    }

    [Test]
    public void Example2_CalculatePartCount() {
        var ranges = new Dictionary<string, Range<long>> {
            {"a", new Range<long>(10, 19)}, // 10 *
            {"b", new Range<long>(20, 29)}, // 10 *
            {"c", new Range<long>(30, 39)}, // 10 *
            {"d", new Range<long>(40, 44)}, // 5
        };

        Assert.AreEqual(5000, Aplenty.CalculatePartCount(ranges));
    }

    [Test]
    public void Example2() {
        var example = new Aplenty(File.ReadAllLines(@"19\example.txt"));

        Assert.AreEqual(167_409_079_868_000L, example.CalculateAcceptedCombinations());
    }

    [Test]
    public void Puzzle2() {
        var example = new Aplenty(File.ReadAllLines(@"19\input.txt"));

        Assert.AreEqual(122_756_210_763_577L, example.CalculateAcceptedCombinations());
    }
}