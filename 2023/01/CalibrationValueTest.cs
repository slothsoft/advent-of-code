using System;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AoC;

public class CalibrationValueTest {
    private static readonly string[] ExampleInput1 = {
        "1abc2", "pqr3stu8vwx", "a1b2c3d4e5f", "treb7uchet"
    };

    private static readonly string[] ExampleInput2 = {
        "two1nine", "eightwothree", "abcone2threexyz", "xtwone3four", "4nineeightseven2", "zoneight234", "7pqrstsixteen",
    };

    [Test]
    public void Example1Single() {
        Assert.AreEqual(12, CalibrationValue.CalculateSingle("1abc2"));
        Assert.AreEqual(38, CalibrationValue.CalculateSingle("pqr3stu8vwx"));
        Assert.AreEqual(15, CalibrationValue.CalculateSingle("a1b2c3d4e5f"));
        Assert.AreEqual(77, CalibrationValue.CalculateSingle("treb7uchet"));
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
    public void Example2Single() {
        Assert.AreEqual(29, CalibrationValue.CalculateSingleWithDigitAsStrings("two1nine"));
        Assert.AreEqual(83, CalibrationValue.CalculateSingleWithDigitAsStrings("eightwothree"));
        Assert.AreEqual(13, CalibrationValue.CalculateSingleWithDigitAsStrings("abcone2threexyz"));
        Assert.AreEqual(24, CalibrationValue.CalculateSingleWithDigitAsStrings("xtwone3four"));
        Assert.AreEqual(42, CalibrationValue.CalculateSingleWithDigitAsStrings("4nineeightseven2"));
        Assert.AreEqual(14, CalibrationValue.CalculateSingleWithDigitAsStrings("zoneight234"));
        Assert.AreEqual(76, CalibrationValue.CalculateSingleWithDigitAsStrings("7pqrstsixteen"));
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