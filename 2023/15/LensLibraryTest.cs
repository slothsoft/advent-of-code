using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC;

public class LensLibraryTest {
    [Test]
    [TestCase("", 0)]
    [TestCase("H", 200)]
    [TestCase("HA", 153)]
    [TestCase("HAS", 172)]
    [TestCase("HASH", 52)]
    public void Example1_CalculateHash(string input, int expectedHash) {
        Assert.AreEqual(expectedHash, LensLibrary.CalculateHash(input));
    }
    
    [Test]
    [TestCase("rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7", 1320)]
    public void Example1_CalculateHashFromInitSequence(string input, int expectedHash) {
        Assert.AreEqual(expectedHash, LensLibrary.CalculateHashFromInitSequence(input));
    }

    [Test]
    public void Example1() {
        var result = LensLibrary.CalculateHashFromInitSequence(File.ReadAllLines(@"15\example.txt").Single());

        Assert.AreEqual(1320, result);
    }

    [Test]
    public void Puzzle1() {
        var result = LensLibrary.CalculateHashFromInitSequence(File.ReadAllLines(@"15\input.txt").Single());

        Assert.AreEqual(495972, result);
    }

    [Test]
    public void Example2() {
        var result = new LensLibrary(File.ReadAllLines(@"15\example.txt").Single());
        result.ExecuteInitializationSequence();
        
        Assert.AreEqual(145, result.CalculateFocusingPower());
    }

    [Test]
    public void Puzzle2() {
        var result = new LensLibrary(File.ReadAllLines(@"15\input.txt").Single());
        result.ExecuteInitializationSequence();
        
        Assert.AreEqual(245223, result.CalculateFocusingPower());
    }
}