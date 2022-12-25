using System.IO;
using NUnit.Framework;

namespace AoC._25;

public class FullOfHotAirTest {
    [Test]
    [TestCase(1, "1")]
    [TestCase(2, "2")]
    [TestCase(3, "1=")]
    [TestCase(4, "1-")]
    [TestCase(5, "10")]
    [TestCase(6, "11")]
    [TestCase(7, "12")]
    [TestCase(8, "2=")]
    [TestCase(9, "2-")]
    [TestCase(10, "20")]
    [TestCase(15, "1=0")]
    [TestCase(20, "1-0")]
    [TestCase(2022, "1=11-2")]
    [TestCase(12345, "1-0---0")]
    [TestCase(314159265, "1121-1110-1=0")]
    public void Example1DecimalToSnafu(long decimalNumber, string snafuNumber) {
        Assert.AreEqual(snafuNumber, decimalNumber.ConvertToSnafu());
        Assert.AreEqual(decimalNumber,snafuNumber.ConvertFromSnafu());
    }

    [Test]
    [TestCase("2=-01", 976)]
    [TestCase("1=-0-2", 1747)]
    [TestCase("12111", 906)]
    [TestCase("2=0=", 198)]
    [TestCase("21", 11)]
    [TestCase("2=01", 201)]
    [TestCase("111", 31)]
    [TestCase("20012", 1257)]
    [TestCase("112", 32)]
    [TestCase("1=-1=", 353)]
    [TestCase("1-12", 107)]
    [TestCase("12", 7)]
    [TestCase("1=", 3)]
    [TestCase("122", 37)]
    public void Example1SnafuToDecimal(string snafuNumber, long decimalNumber) {
        Assert.AreEqual(decimalNumber,snafuNumber.ConvertFromSnafu());
        Assert.AreEqual(snafuNumber, decimalNumber.ConvertToSnafu());
    }

    [Test]
    public void Example1() {
        var fullOfHotAir = new FullOfHotAir(File.ReadAllLines(@"25\example.txt"));

        Assert.AreEqual(4890, fullOfHotAir.CalculateSumInDecimal());
        Assert.AreEqual("2=-1=0", fullOfHotAir.CalculateSumInSnafu());
    }

    [Test]
    public void Puzzle1() {
        var blizzardBasin = new FullOfHotAir(File.ReadAllLines(@"25\input.txt"));

        var result = blizzardBasin.CalculateSumInSnafu();
        Assert.AreEqual("2-0-020-1==1021=--01", result);
        Assert.Pass("Puzzle 1: " + result);
    }
}