using System;
using System.IO;
using NUnit.Framework;

namespace AoC._13;

public class DistressSignalTest {
    [Test]
    [TestCase("[1,1,3,1,1]", "[1,1,5,1,1]", DistressSignal.PacketOrder.Correct)]
    [TestCase("[[1],[2,3,4]]", "[[1],4]", DistressSignal.PacketOrder.Correct)]
    [TestCase("[9]", "[[8,7,6]]", DistressSignal.PacketOrder.Wrong)]
    [TestCase("[[4,4],4,4]", "[[4,4],4,4,4]", DistressSignal.PacketOrder.Correct)]
    [TestCase("[7,7,7,7]", "[7,7,7]", DistressSignal.PacketOrder.Wrong)]
    [TestCase("[]", "[3]", DistressSignal.PacketOrder.Correct)]
    [TestCase("[[[]]]", "[[]]", DistressSignal.PacketOrder.Wrong)]
    [TestCase("[1,[2,[3,[4,[5,6,7]]]],8,9]", "[1,[2,[3,[4,[5,6,0]]]],8,9]", DistressSignal.PacketOrder.Wrong)]
    public void Example1(string leftString, string rightString, DistressSignal.PacketOrder expectedPacketOrder) {
        var left = DistressSignal.ParsePacket(leftString);
        var right = DistressSignal.ParsePacket(rightString);

        Assert.AreEqual(expectedPacketOrder, left.CompareToPacket(right));
    }

    [Test]
    [TestCase("1,1,3", new[] {"1", "1", "3"})]
    [TestCase("[1],[2,3,4]", new[] {"[1]", "[2,3,4]"})]
    [TestCase("1,[2,[3,[4,[5,6,7]]]],8,9", new[] {"1", "[2,[3,[4,[5,6,7]]]]", "8", "9"})]
    public void Example1SplitList(string leftString, string[] expectedSplit) {
        Assert.AreEqual(expectedSplit, DistressSignal.SplitList(leftString));
    }

    [Test]
    public void Example1WithFile() {
        var distressSignal = new DistressSignal(File.ReadAllLines(@"13\example.txt"));

        Assert.AreEqual(13, distressSignal.CalculateCorrectOrderIndices());
    }

    [Test]
    [TestCase("[[[],3],[5,[1],[8,5],10,[5,8]]],[],[1]", new[] {"[[[],3],[5,[1],[8,5],10,[5,8]]]", "[]", "[1]"})]
    public void Puzzle1SplitList(string leftString, string[] expectedSplit) {
        Assert.AreEqual(expectedSplit, DistressSignal.SplitList(leftString));
    }

    [Test]
    [TestCase("[[[[],3],[5,[1],[8,5],10,[5,8]]],[],[1]]", "[[9,[4,9]]]", DistressSignal.PacketOrder.Correct)]
    public void Puzzle1Parsing(string leftString, string rightString, DistressSignal.PacketOrder expectedPacketOrder) {
        var left = DistressSignal.ParsePacket(leftString);
        var right = DistressSignal.ParsePacket(rightString);

        Assert.AreEqual(expectedPacketOrder, left.CompareToPacket(right));
    }

    [Test]
    public void Puzzle1() {
        var distressSignal = new DistressSignal(File.ReadAllLines(@"13\input.txt"));
        var result = distressSignal.CalculateCorrectOrderIndices();
        Assert.AreEqual(6086, result);
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    public void Example2() {
        var distressSignal = new DistressSignal(File.ReadAllLines(@"13\example.txt"));
        var dividerPacket1 = DistressSignal.ParsePacket("[[2]]");
        var dividerPacket2 = DistressSignal.ParsePacket("[[6]]");
        
        var orderedPackets = distressSignal.OrderPackets(dividerPacket1, dividerPacket2);
        Assert.AreEqual(10, Array.IndexOf(orderedPackets, dividerPacket1) + 1);
        Assert.AreEqual(14, Array.IndexOf(orderedPackets, dividerPacket2) + 1);
    }

    [Test]
    public void Puzzle2() {
        var distressSignal = new DistressSignal(File.ReadAllLines(@"13\input.txt"));
        var dividerPacket1 = DistressSignal.ParsePacket("[[2]]");
        var dividerPacket2 = DistressSignal.ParsePacket("[[6]]");
        
        var orderedPackets = distressSignal.OrderPackets(dividerPacket1, dividerPacket2);
        var result = (1 + Array.IndexOf(orderedPackets, dividerPacket1)) * (1 + Array.IndexOf(orderedPackets, dividerPacket2));
        Assert.AreEqual(27930, result);
        Assert.Pass("Puzzle 2: " + result);
    }
}