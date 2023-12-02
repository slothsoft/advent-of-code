using System;
using NUnit.Framework;

namespace AoC;

public class ParseExtensionsTest {
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
}