using NUnit.Framework;

namespace AoC;

public class ParseExtensionsTest {
    [Test]
    [TestCase(12, "1abc2")]
    [TestCase(38, "pqr3stu8vwx")]
    [TestCase(12345, "a1b2c3d4e5f")]
    [TestCase(7, "treb7uchet")]
    public void TestExtractDigitsAsInt(int expected, string input) {
        Assert.AreEqual(expected, input.ExtractDigitsAsInt());
    }
    
    [Test]
    [TestCase("12", "1abc2")]
    [TestCase("38", "pqr3stu8vwx")]
    [TestCase("12345", "a1b2c3d4e5f")]
    [TestCase("7", "treb7uchet")]
    public void TestExtractDigitsAsString(string expected, string input) {
        Assert.AreEqual(expected, input.ExtractDigitsAsString());
    }
}