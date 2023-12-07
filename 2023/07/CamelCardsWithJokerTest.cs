using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using CardType = AoC.CamelCardsWithJoker.CardType;
using Hand = AoC.CamelCardsWithJoker.Hand;
using HandType = AoC.CamelCardsWithJoker.HandType;

namespace AoC;

public class CamelCardsWithJokerTest {

    private record ExampleData(string InputHand, Hand Hand, string SortedHand, HandType Type);

    private static readonly ExampleData[] ExampleDatas = {
        new("32T3K", new Hand(CardType._3, CardType._2, CardType.T, CardType._3, CardType.K), "KT332", HandType.OnePair),
        new("T55J5", new Hand(CardType.T, CardType._5, CardType._5, CardType.J, CardType._5), "T555J", HandType.FourOfAKind),
        new("KK677", new Hand(CardType.K, CardType.K, CardType._6, CardType._7, CardType._7), "KK776", HandType.TwoPair),
        new("KTJJT", new Hand(CardType.K, CardType.T, CardType.J, CardType.J, CardType.T), "KTTJJ", HandType.FourOfAKind),
        new("QQQJA", new Hand(CardType.Q, CardType.Q, CardType.Q, CardType.J, CardType.A), "AQQQJ", HandType.FourOfAKind),
    };
    
    [Test]
    [TestCaseSource(nameof(GetParseHandTestData))]
    public void Example2_ParseHand(string input, Hand expectedHand) {
        var actualHand = CamelCardsWithJoker.ParseHand(input);
        
        Assert.NotNull(actualHand);
        Assert.AreEqual(expectedHand.Cards, actualHand.Cards, $"expected {expectedHand}, but found {input}");
    }

    private static IEnumerable<TestCaseData> GetParseHandTestData() => ExampleDatas.Select(e => new TestCaseData(e.InputHand, e.Hand));

    [Test]
    [TestCaseSource(nameof(GetSortHandTestData))]
    public void Example2_SortHand(Hand input, Hand expectedSorting) {
        Array.Sort(input.Cards);
        Assert.AreEqual(expectedSorting.Cards, input.Cards, $"expected {expectedSorting}, but found {input}");
    }

    private static IEnumerable<TestCaseData> GetSortHandTestData() => ExampleDatas.Select(e => new TestCaseData(CamelCardsWithJoker.ParseHand(e.InputHand), CamelCardsWithJoker.ParseHand(e.SortedHand)));

    [Test]
    [TestCaseSource(nameof(GetHandTypeTestData))]
    public void Example2_HandType(Hand input, HandType expectedType) {
        Assert.AreEqual(expectedType, input.Type);
    }

    private static IEnumerable<TestCaseData> GetHandTypeTestData() => ExampleDatas.Select(e => new TestCaseData(CamelCardsWithJoker.ParseHand(e.InputHand), e.Type));
    
    [Test]
    public void Example2_SortHands() {
        var hands = ExampleDatas.Select(e => CamelCardsWithJoker.ParseHand(e.InputHand)).ToArray();
        Array.Sort(hands);

        var index = 0;
        AssertAreEqual(hands[index++], "KTJJT");
        AssertAreEqual(hands[index++], "QQQJA");
        AssertAreEqual(hands[index++], "T55J5");
        AssertAreEqual(hands[index++], "KK677");
        AssertAreEqual(hands[index], "32T3K");
    }

    private static void AssertAreEqual(Hand actualHand, string expectedHand) {
        Assert.True(actualHand.ToString().Contains(expectedHand), $"expected {expectedHand}, but was {actualHand}");
    }

    [Test]
    public void Example2_ParseHandsAndBids() {
        var example = new CamelCardsWithJoker(File.ReadAllLines(@"07\example.txt"));

        Assert.AreEqual(483, example.HandsAndBids.Single(kv => kv.Key.ToString().Contains("QQQJA")).Value);
        Assert.AreEqual(220, example.HandsAndBids.Single(kv => kv.Key.ToString().Contains("KTJJT")).Value);
        Assert.AreEqual(28, example.HandsAndBids.Single(kv => kv.Key.ToString().Contains("KK677")).Value);
        Assert.AreEqual(684, example.HandsAndBids.Single(kv => kv.Key.ToString().Contains("T55J5")).Value);
        Assert.AreEqual(765, example.HandsAndBids.Single(kv => kv.Key.ToString().Contains("32T3K")).Value);
    }
    
    [Test]
    public void Example2() {
        var example = new CamelCardsWithJoker(File.ReadAllLines(@"07\example.txt"));
        
        Assert.AreEqual(5905, example.CalculateTotalWinnings());
    }
    
    [Test]
    public void Puzzle2() {
        var puzzle = new CamelCardsWithJoker(File.ReadAllLines(@"07\input.txt"));
        
        Assert.AreEqual(250665248, puzzle.CalculateTotalWinnings());
    }
}