using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC;

public class ScratchcardsTest {
    private static readonly string[] ExampleInput = {
        "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",
        "Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",
        "Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",
        "Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 ",
        "Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36",
        "Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11",
    };

    [Test]
    public void Example1_ParseCard1() {
        var card = Scratchcards.ParseCard("Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53");
        Assert.AreEqual(1, card.Id);

        var numbers = card.WinningNumbers;
        Assert.AreEqual(new [] {41, 48, 83, 86, 17}, card.WinningNumbers);
        Assert.AreEqual(new [] {83, 86,  6, 31, 17,  9, 48, 53}, card.NumbersYouHave);
    }
    
    [Test]
    [TestCase(1, 8)]
    [TestCase(2, 2)]
    [TestCase(3, 2)]
    [TestCase(4, 1)]
    [TestCase(5, 0)]
    [TestCase(6, 0)]
    public void Example1_CalculatePoints(int cardId, int expectedPoints) {
        var card = Scratchcards.ParseCard(ExampleInput[cardId - 1]);
        Assert.AreEqual(expectedPoints, card.CalculatePoints());
    }

    [Test]
    public void Example1() {
        var example = new Scratchcards(ExampleInput);
        
        Assert.AreEqual(13, example.CalculatePoints());
    }

    [Test]
    public void Puzzle1() {
        var example = new Scratchcards(File.ReadAllLines(@"04\input.txt"));
        
        Assert.AreEqual(17803, example.CalculatePoints());
    }
    
    [Test]
    public void Example2() {
        var example = new Scratchcards(ExampleInput);
    
        Assert.AreEqual(30, example.CalculateScratchcardDuplication());
    }
    
    [Test]
    public void Puzzle2() {
        var example = new Scratchcards(File.ReadAllLines(@"04\input.txt"));
        
        Assert.AreEqual(5554894, example.CalculateScratchcardDuplication());
    }
}