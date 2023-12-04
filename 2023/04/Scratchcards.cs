using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/4">Day 4: Scratchcards</a>: Take a seat in the large pile of colorful
/// cards. How many points are they worth in total?
/// </summary>
public class Scratchcards {

    public record Card(int Id, int[] WinningNumbers, int[] NumbersYouHave) {
        public int WinningNumbersYouHave { get => NumbersYouHave.Count(n => WinningNumbers.Contains(n));  }
        
        public long CalculatePoints() {
            var winingNumbersYouHave = WinningNumbersYouHave;
            if (winingNumbersYouHave == 0) {
                return 0;
            }

            return (long) Math.Pow(2, winingNumbersYouHave - 1);
        }
    }

    public Scratchcards(IEnumerable<string> input) {
        Cards = ParseCards(input);
    }

    public Card[] Cards { get; }

    private static Card[] ParseCards(IEnumerable<string> input) {
        return input.Select(ParseCard).ToArray();
    }

    internal static Card ParseCard(string input) {
        var cardAndNumbers = input.Split(':');
        var id = cardAndNumbers[0].ExtractDigitsAsInt();
        var winningAndNumbersYouHave = cardAndNumbers[1].Split('|');
        return new Card(id, ParseIntArray(winningAndNumbersYouHave[0]), ParseIntArray(winningAndNumbersYouHave[1]));
    }
    
    private static int[] ParseIntArray(string input) {
        return input.Replace("  ", " ").ParseIntArray();
    }

    public long CalculatePoints() {
        return Cards.Select(c => c.CalculatePoints()).Sum();
    }

    public long CalculateScratchcardDuplication() {
        var cardCount = Cards.Select(_ => 1L).ToArray();
        for (var i = 0; i < Cards.Length; i++) {
            var winingNumbersYouHave = Cards[i].WinningNumbersYouHave;
            for (var n = 0; n < winingNumbersYouHave; n++) {
                cardCount[i + n + 1] += cardCount[i];
            }
        }

        return cardCount.Sum();
    }
}