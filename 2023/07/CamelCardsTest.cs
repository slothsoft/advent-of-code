using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using CardType = AoC.CamelCards.CardType;
using Hand = AoC.CamelCards.Hand;
using HandType = AoC.CamelCards.HandType;

namespace AoC;

public class CamelCardsTest {

    private record ExampleData(string InputHand, Hand Hand, string SortedHand, HandType Type);

    private static readonly ExampleData[] ExampleDatas = {
        new("32T3K", new Hand(CardType._3, CardType._2, CardType.T, CardType._3, CardType.K), "KT332", HandType.OnePair),
        new("T55J5", new Hand(CardType.T, CardType._5, CardType._5, CardType.J, CardType._5), "JT555", HandType.ThreeOfAKind),
        new("KK677", new Hand(CardType.K, CardType.K, CardType._6, CardType._7, CardType._7), "KK776", HandType.TwoPair),
        new("KTJJT", new Hand(CardType.K, CardType.T, CardType.J, CardType.J, CardType.T), "KJJTT", HandType.TwoPair),
        new("QQQJA", new Hand(CardType.Q, CardType.Q, CardType.Q, CardType.J, CardType.A), "AQQQJ", HandType.ThreeOfAKind),
    };
    
    [Test]
    [TestCaseSource(nameof(GetParseHandTestData))]
    public void Example1_ParseHand(string input, Hand expectedHand) {
        var actualHand = CamelCards.ParseHand(input);
        
        Assert.NotNull(actualHand);
        Assert.AreEqual(expectedHand.Cards, actualHand.Cards, $"expected {expectedHand}, but found {input}");
    }

    private static IEnumerable<TestCaseData> GetParseHandTestData() => ExampleDatas.Select(e => new TestCaseData(e.InputHand, e.Hand));

    [Test]
    [TestCaseSource(nameof(GetSortHandTestData))]
    public void Example1_SortHand(Hand input, Hand expectedSorting) {
        Array.Sort(input.Cards);
        Assert.AreEqual(expectedSorting.Cards, input.Cards, $"expected {expectedSorting}, but found {input}");
    }

    private static IEnumerable<TestCaseData> GetSortHandTestData() => ExampleDatas.Select(e => new TestCaseData(CamelCards.ParseHand(e.InputHand), CamelCards.ParseHand(e.SortedHand)));

    [Test]
    [TestCaseSource(nameof(GetHandTypeTestData))]
    public void Example1_HandType(Hand input, HandType expectedType) {
        Assert.AreEqual(expectedType, input.Type);
    }

    private static IEnumerable<TestCaseData> GetHandTypeTestData() => ExampleDatas.Select(e => new TestCaseData(CamelCards.ParseHand(e.InputHand), e.Type));
    
    [Test]
    public void Example1_SortHands() {
        var hands = ExampleDatas.Select(e => CamelCards.ParseHand(e.InputHand)).ToArray();
        Array.Sort(hands);

        var index = 0;
        AssertAreEqual(hands[index++], "QQQJA");
        AssertAreEqual(hands[index++], "T55J5");
        AssertAreEqual(hands[index++], "KK677");
        AssertAreEqual(hands[index++], "KTJJT");
        AssertAreEqual(hands[index], "32T3K");
    }

    private static void AssertAreEqual(Hand actualHand, string expectedHand) {
        Assert.True(actualHand.ToString().Contains(expectedHand), $"expected {expectedHand}, but was {actualHand}");
    }

    [Test]
    public void Example1_ParseHandsAndBids() {
        var example = new CamelCards(File.ReadAllLines(@"07\example.txt"));

        Assert.AreEqual(483, example.HandsAndBids.Single(kv => kv.Key.ToString().Contains("QQQJA")).Value);
        Assert.AreEqual(220, example.HandsAndBids.Single(kv => kv.Key.ToString().Contains("KTJJT")).Value);
        Assert.AreEqual(28, example.HandsAndBids.Single(kv => kv.Key.ToString().Contains("KK677")).Value);
        Assert.AreEqual(684, example.HandsAndBids.Single(kv => kv.Key.ToString().Contains("T55J5")).Value);
        Assert.AreEqual(765, example.HandsAndBids.Single(kv => kv.Key.ToString().Contains("32T3K")).Value);
    }
    
    [Test]
    public void Example1() {
        var example = new CamelCards(File.ReadAllLines(@"07\example.txt"));
        
        Assert.AreEqual(6440, example.CalculateTotalWinnings());
    }
    
    [Test]
    public void Puzzle1() {
        var puzzle = new CamelCards(File.ReadAllLines(@"07\input.txt"));
        
        Assert.AreEqual(250120186, puzzle.CalculateTotalWinnings());
    }
}