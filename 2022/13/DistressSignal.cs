using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._13;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/13">Day 13: Distress Signal</a>: You climb the
/// hill and again try contacting the Elves. However, you instead receive a signal you weren't
/// expecting: a distress signal.
///
/// Your handheld device must still not be working properly; the packets from the distress signal
/// got decoded out of order. You'll need to re-order the list of received packets (your puzzle
/// input) to decode the message.
///
/// Your list consists of pairs of packets; pairs are separated by a blank line. You need to
/// identify how many pairs of packets are in the right order.
/// </summary>
public class DistressSignal {
    public interface IPacket : IComparable<IPacket> { 
        
        PacketOrder CompareToPacket(IPacket right);
    }

    public enum PacketOrder {
        Correct = -1,
        Wrong = 1,
        Undetermined = 0,
    }

    record ValuePacket(int Value) : IPacket {
        public PacketOrder CompareToPacket(IPacket right) {
            if (right is ValuePacket packet) {
                if (Value > packet.Value)
                    return PacketOrder.Wrong;
                if (Value < packet.Value)
                    return PacketOrder.Correct;
                return PacketOrder.Undetermined;
            }

            // the right is a list - so make this a list and try again
            return new ListPacket(this).CompareToPacket(right);
        }

        public int CompareTo(IPacket? other) {
            return (int) CompareToPacket(other!);
        }
    }

    record ListPacket(params IPacket[] Elements) : IPacket {
        public PacketOrder CompareToPacket(IPacket right) {
            if (right is ValuePacket packet) {
                // the right is a single value - so make it a list and try again
                return CompareToPacket(new ListPacket(packet));
            }

            var rightList = (ListPacket) right;
            var index = 0;
            while (index < Elements.Length) {
                if (index >= rightList.Elements.Length) {
                    // the right list does not have any more elements
                    return PacketOrder.Wrong;
                }

                var order = Elements[index].CompareToPacket(rightList.Elements[index]);
                if (order != PacketOrder.Undetermined)
                    // one of the children has a determined order
                    return order;

                index++;
            }

            if (index < rightList.Elements.Length) {
                // the left list does not have any more elements, but the right does
                return PacketOrder.Correct;
            }

            return PacketOrder.Undetermined;
        }
        
        public int CompareTo(IPacket? other) {
            return (int) CompareToPacket(other!);
        }
    }

    public static IPacket ParsePacket(string line) {
        if (line.StartsWith("[")) {
            // this packet contains another list
            var elements = SplitList(line.Substring(1, line.Length - 2))
                .Where(l => l.Length > 0)
                .Select(ParsePacket).ToArray();
            return new ListPacket(elements);
        }

        try {
            // this packet contains a single value
            return new ValuePacket(int.Parse(line));
        } catch (FormatException e) {
            throw new FormatException(e.Message + $" ({line})", e.InnerException);
        }
    }

    internal static IEnumerable<string> SplitList(string line) {
        var bracketCounter = 0;
        var currentString = "";

        for (var i = 0; i < line.Length; i++) {
            switch (line[i]) {
                case '[':
                    currentString += line[i];
                    bracketCounter++;
                    break;
                case ']':
                    currentString += line[i];
                    bracketCounter--;
                    break;
                case ',' when bracketCounter == 0:
                    yield return currentString;
                    currentString = "";
                    break;
                default:
                    currentString += line[i];
                    break;
            }
        }

        if (currentString != "") {
            yield return currentString;
        }
    }

    private readonly IList<KeyValuePair<IPacket, IPacket>> _packetPairs = new List<KeyValuePair<IPacket, IPacket>>();

    public DistressSignal(string[] lines) {
        for (var i = 0; i < lines.Length; i += 3) {
            _packetPairs.Add(new KeyValuePair<IPacket, IPacket>(ParsePacket(lines[i]), ParsePacket(lines[i + 1])));
        }
    }

    public int CalculateCorrectOrderIndices() {
        var result = 0;
        for (var i = 0; i < _packetPairs.Count; i++) {
            if (_packetPairs[i].Key.CompareToPacket(_packetPairs[i].Value) == PacketOrder.Correct) {
                result += i + 1;
            }
        }

        return result;
    }

    public IPacket[] OrderPackets(params IPacket[] additionalPackets) {
        return _packetPairs
            .SelectMany(pair => new[] {pair.Key, pair.Value})
            .Concat(additionalPackets)
            .OrderBy(p => p)
            .ToArray();
    }
}