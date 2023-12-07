using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/7">Day 7: Camel Cards</a>: Find the rank of every hand in your set. What are the total winnings?
/// </summary>
public class CamelCards {
    public record Hand(params CardType[] Cards) : IComparable<Hand> {

        private HandType _type = HandType.Unknown;
        public HandType Type {
            get {
                if (_type == HandType.Unknown) {
                    _type = CalculateType();
                }

                return _type;
            }
        }
        
        public int CompareTo(Hand? other) {
            if (other == null) {
                return 1;
            }

            var compare = Type.CompareTo(other.Type);
            if (compare != 0) {
                return compare;
            }
            
            for (var i = 0; i < Cards.Length; i++) {
                compare = Cards[i].CompareTo(other.Cards[i]);
                if (compare != 0) {
                    return compare;
                }
            }
            
            return 0;
        }

        public override string ToString() => $"Hand ({string.Join("", Cards.Select(c => c.ToString().Replace("_", "")))})";

        private HandType CalculateType() {
            var sortedCards = new CardType[Cards.Length];
            Cards.CopyTo(sortedCards, 0);
            Array.Sort(sortedCards);

            var multiples = new List<int>();
            var startIndex = 0;
            do {
                var multiple = FindMultiples(sortedCards, startIndex);
                startIndex += multiple;
                multiples.Add(multiple);
            } while (startIndex < Cards.Length);
            
            multiples.Sort();
            var highestMultiple = multiples[^1];
            var lowestMultiple = multiples.Count > 1 ? multiples[^2] : 0;
            
            switch (highestMultiple) {
                case 5: return HandType.FiveOfAKind;
                case 4: return HandType.FourOfAKind;
                case 3: return lowestMultiple == 1 ? HandType.ThreeOfAKind : HandType.FullHouse;
                case 2: return lowestMultiple == 2 ? HandType.TwoPair : HandType.OnePair;
                default: return HandType.HighCard;
            }
        }
        
        private static int FindMultiples(CardType[] cards, int startIndex) {
            if (startIndex >= cards.Length) {
                return 0;
            }
            
            var count = 1;
            var card = cards[startIndex];
            for (var i = startIndex + 1; i < cards.Length; i++) {
                if (cards[i] == card) {
                    count++;
                } else {
                    break;
                }
            }

            return count;
        }
    }

    public enum CardType {
        A, K, Q, J, T, _9, _8, _7, _6, _5, _4, _3, _2
    }

    public enum HandType {
        FiveOfAKind, FourOfAKind, FullHouse, ThreeOfAKind, TwoPair, OnePair, HighCard, Unknown
    }

    public CamelCards(IEnumerable<string> input) {
        HandsAndBids = ParseHandsAndBids(input);
    }

    public IDictionary<Hand, int> HandsAndBids { get; set; }

    private static IDictionary<Hand, int> ParseHandsAndBids(IEnumerable<string> input) {
        return input.Select(ParseHandAndBid).ToDictionary(v => v.Hand, v => v.Bid);
    }

    private static (Hand Hand, int Bid) ParseHandAndBid(string input) {
        var split = input.Split(' ');
        return (ParseHand(split[0]), int.Parse(split[1]));
    }

    public static Hand ParseHand(string input) {
        return new Hand(input.Select(ParseCardType).ToArray());
    }
    
    private static CardType ParseCardType(char input) {
        if (char.IsDigit(input)) {
            return (CardType) Enum.Parse(typeof(CardType), "_" + input);
        }
        return (CardType) Enum.Parse(typeof(CardType), input.ToString());
    }

    public long CalculateTotalWinnings() {
        long handsCount = HandsAndBids.Count;
        return HandsAndBids.OrderBy(kv => kv.Key).Select((kv, i) => (handsCount - i) * kv.Value).Sum();
    }
}