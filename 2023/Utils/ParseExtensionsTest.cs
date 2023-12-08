using System;
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
    [TestCase(12L, "1abc2")]
    [TestCase(38L, "pqr3stu8vwx")]
    [TestCase(12345L, "a1b2c3d4e5f")]
    [TestCase(7L, "treb7uchet")]
    public void TestExtractDigitsAsLong(long expected, string input) {
        Assert.AreEqual(expected, input.ExtractDigitsAsLong());
    }
    
    [Test]
    [TestCase("12", "1abc2")]
    [TestCase("38", "pqr3stu8vwx")]
    [TestCase("12345", "a1b2c3d4e5f")]
    [TestCase("7", "treb7uchet")]
    public void TestExtractDigitsAsString(string expected, string input) {
        Assert.AreEqual(expected, input.ExtractDigitsAsString());
    }
    
    [Test]
    public void TestParseMatrix() {
        var lines = new[] {"abc", "def", "ghi", "jkl",};
        var matrix = lines.ParseMatrix(char.ToUpper);

        Assert.AreEqual('A', matrix[0][0]);
        Assert.AreEqual('B', matrix[1][0]);
        Assert.AreEqual('C', matrix[2][0]);

        Assert.AreEqual('D', matrix[0][1]);
        Assert.AreEqual('E', matrix[1][1]);
        Assert.AreEqual('F', matrix[2][1]);

        Assert.AreEqual('G', matrix[0][2]);
        Assert.AreEqual('H', matrix[1][2]);
        Assert.AreEqual('I', matrix[2][2]);

        Assert.AreEqual('J', matrix[0][3]);
        Assert.AreEqual('K', matrix[1][3]);
        Assert.AreEqual('L', matrix[2][3]);
    }

    [Test]
    public void TestParseBoolMatrix() {
        var lines = new[] {"T  ", " T ", "  T",};
        var matrix = lines.ParseBoolMatrix('T', ' ');

        Assert.AreEqual(true, matrix[0][0]);
        Assert.AreEqual(false, matrix[1][0]);
        Assert.AreEqual(false, matrix[2][0]);

        Assert.AreEqual(false, matrix[0][1]);
        Assert.AreEqual(true, matrix[1][1]);
        Assert.AreEqual(false, matrix[2][1]);

        Assert.AreEqual(false, matrix[0][2]);
        Assert.AreEqual(false, matrix[1][2]);
        Assert.AreEqual(true, matrix[2][2]);
    }

    [Test]
    public void TestParseBoolMatrixException() {
        var lines = new[] {"TF-",};
        var exception = Assert.Throws<ArgumentException>(() => lines.ParseBoolMatrix('T', 'F'));

        Assert.NotNull(exception);
        Assert.AreEqual("Cannot parse: -", exception!.Message);
    }
    
    [Test]
    public void TestParseCharMatrix() {
        var lines = new[] {"abc", "def", "ghi", "jkl",};
        var matrix = lines.ParseCharMatrix();

        Assert.AreEqual('a', matrix[0][0]);
        Assert.AreEqual('b', matrix[1][0]);
        Assert.AreEqual('c', matrix[2][0]);

        Assert.AreEqual('d', matrix[0][1]);
        Assert.AreEqual('e', matrix[1][1]);
        Assert.AreEqual('f', matrix[2][1]);

        Assert.AreEqual('g', matrix[0][2]);
        Assert.AreEqual('h', matrix[1][2]);
        Assert.AreEqual('i', matrix[2][2]);

        Assert.AreEqual('j', matrix[0][3]);
        Assert.AreEqual('k', matrix[1][3]);
        Assert.AreEqual('l', matrix[2][3]);
    }

    [Test]
    [TestCase("", new int[0])]
    [TestCase("1 2 3", new[] {1, 2, 3})]
    [TestCase("  4 5 6  ", new[] {4, 5, 6})]
    public void TestParseIntArray(string input, int[] expected) {
        var someInts = input.ParseIntArray();

        Assert.AreEqual(expected, someInts);
    }
    
    [Test]
    [TestCase("", '.', new int[0])]
    [TestCase("1_2_3", '_', new[] {1, 2, 3})]
    [TestCase("  4|5|6  ", '|', new[] {4, 5, 6})]
    public void TestParseIntArray(string input, char separator, int[] expected) {
        var someInts = input.ParseIntArray(separator);

        Assert.AreEqual(expected, someInts);
    }
    
    [Test]
    [TestCase("", new long[0])]
    [TestCase("1 2 3", new[] {1L, 2, 3})]
    [TestCase("  4 5 6  ", new[] {4L, 5, 6})]
    public void TestParseLongArray(string input, long[] expected) {
        var someLongs = input.ParseLongArray();

        Assert.AreEqual(expected, someLongs);
    }
    
    [Test]
    [TestCase("", '.', new long[0])]
    [TestCase("1_2_3", '_', new[] {1L, 2, 3})]
    [TestCase("  4|5|6  ", '|', new[] {4L, 5, 6})]
    public void TestParseLongArray(string input, char separator, long[] expected) {
        var someLongs = input.ParseLongArray(separator);

        Assert.AreEqual(expected, someLongs);
    }
}