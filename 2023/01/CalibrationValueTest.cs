using System.IO;
using NUnit.Framework;

namespace AoC;

public class CalibrationValueTest {
    private static readonly string[] ExampleInput1 = {"1abc2", "pqr3stu8vwx", "a1b2c3d4e5f", "treb7uchet"};

    private static readonly string[] ExampleInput2 = {
        "two1nine", "eightwothree", "abcone2threexyz", "xtwone3four", "4nineeightseven2", "zoneight234", "7pqrstsixteen",
    };

    [Test]
    [TestCase(12, "1abc2")]
    [TestCase(38, "pqr3stu8vwx")]
    [TestCase(15, "a1b2c3d4e5f")]
    [TestCase(77, "treb7uchet")]
    public void Example1Single(int expected, string input) {
        Assert.AreEqual(expected, CalibrationValue.CalculateSingle(input));
    }

    [Test]
    public void Example1() {
        Assert.AreEqual(142, CalibrationValue.Calculate(ExampleInput1));
    }

    [Test]
    public void Puzzle1() {
        Assert.AreEqual(54630, CalibrationValue.Calculate(File.ReadAllLines(@"01\input.txt")));
    }

    [Test]
    [TestCase(29, "two1nine")]
    [TestCase(83, "eightwothree")]
    [TestCase(13, "abcone2threexyz")]
    [TestCase(24, "xtwone3four")]
    [TestCase(42, "4nineeightseven2")]
    [TestCase(14, "zoneight234")]
    [TestCase(76, "7pqrstsixteen")]
    public void Example2Single(int expected, string input) {
        Assert.AreEqual(expected, CalibrationValue.CalculateSingleWithDigitAsStrings(input));
    }

    [Test]
    public void Example2() {
        Assert.AreEqual(281, CalibrationValue.CalculateWithDigitAsStrings(ExampleInput2));
    }

    [Test]
    public void Puzzle2() {
        Assert.AreEqual(54770, CalibrationValue.CalculateWithDigitAsStrings(File.ReadAllLines(@"01\input.txt")));
    }
}