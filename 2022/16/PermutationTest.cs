using System.Linq;
using NUnit.Framework;

namespace AoC._16;

[TestFixture]
public class PermutationTest {
    [Test]
    public void Permute() {
        var result = "ABC".Permute().ToArray();

        Assert.NotNull(result);
        Assert.IsTrue(result.Contains("ABC"));
        Assert.IsTrue(result.Contains("ACB"));
        Assert.IsTrue(result.Contains("BAC"));
        Assert.IsTrue(result.Contains("BCA"));
        Assert.IsTrue(result.Contains("CBA"));
        Assert.IsTrue(result.Contains("CAB"));
        Assert.AreEqual(6, result.Length);
    }

    [Test]
    public void PermuteBools() {
        var numbers = new[] {true, false};
        var result = numbers.Permute().ToArray();

        Assert.NotNull(result);
        var resultAsString = result.Select(a => string.Join("", a.Select(b => b ? 1 : 0))).ToArray();
        var errorMessage = "Could not find permutation in " + string.Join(", ", resultAsString);
        Assert.IsTrue(resultAsString.Contains("01"), "Could not find permutation in " + string.Join(", ", resultAsString));
        Assert.IsTrue(resultAsString.Contains("10"), "Could not find permutation in " + string.Join(", ", resultAsString));
        Assert.AreEqual(2, result.Length);
    }

    [Test]
    public void PermuteInts() {
        var numbers = new[] {1, 2, 3};
        var result = numbers.Permute().ToArray();

        Assert.NotNull(result);
        var resultAsString = result.Select(a => string.Join("", a)).ToArray();
        var errorMessage = "Could not find permutation in " + string.Join(", ", resultAsString);
        Assert.IsTrue(resultAsString.Contains("123"), errorMessage);
        Assert.IsTrue(resultAsString.Contains("132"), errorMessage);
        Assert.IsTrue(resultAsString.Contains("213"), errorMessage);
        Assert.IsTrue(resultAsString.Contains("231"), errorMessage);
        Assert.IsTrue(resultAsString.Contains("312"), errorMessage);
        Assert.IsTrue(resultAsString.Contains("321"), errorMessage);
        Assert.AreEqual(6, result.Length);
    }
}