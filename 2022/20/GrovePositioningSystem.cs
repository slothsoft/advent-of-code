using System.Collections.Generic;
using System.Linq;

namespace AoC._20;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/20">Day 20: Grove Positioning System</a>
/// </summary>
public class GrovePositioningSystem {
    private readonly long[] _input;

    public GrovePositioningSystem(IEnumerable<string> lines) : this(lines.Select(long.Parse)) {
    }
    
    public GrovePositioningSystem(IEnumerable<long> lines) {
        _input = lines.ToArray();
    }

    public long DecryptionKey { get; init; } = 1;
    public long MixCount { get; init; } = 1;

    public long[] CalculateGrooveCoordinates() {
        var resultingFile = MixFileAsCircularList();
        // Console.WriteLine("\n" + string.Join(", ", resultingFile.ToArray(0)));
        var indexOfZero = resultingFile.IndexOf(0);
        return new[] {
            resultingFile.GetAtIndex((indexOfZero + 1000) % _input.Length), // X
            resultingFile.GetAtIndex((indexOfZero + 2000) % _input.Length), // Y
            resultingFile.GetAtIndex((indexOfZero + 3000) % _input.Length), // Z
        };
    }

    public long[] MixFile(long startNumber = 0) => MixFileAsCircularList().ToArray(startNumber);
    
    internal CircularList MixFileAsCircularList() {
        var list  = new CircularList(_input.Select(i => i * DecryptionKey));
        for (var m = 0; m < MixCount; m++) {
            for (var i = 0; i < _input.Length; i++) {
                list.MoveNumberAt(i);
            }
        }
        return list;
    }
}

class CircularList {

    private readonly IList<long> _list;
    private readonly IList<int> _indexes;

    public CircularList(IEnumerable<long> enumerable) {
        _list = enumerable.ToList();
        _indexes = Enumerable.Range(0, _list.Count).ToList();
    }

    public void MoveNumberAt(int originalIndex) {
        var currentIndex = _indexes.IndexOf(originalIndex);
        var number = _list[currentIndex];
        var futureIndex = WrapIndex(currentIndex + number);

        if (currentIndex == futureIndex) {
            // nothing to do
            return;
        } 
        
        _list.RemoveAt(currentIndex);
        _list.Insert(futureIndex, number);
        
        _indexes.RemoveAt(currentIndex);
        _indexes.Insert(futureIndex, originalIndex);
    }
    
    private int WrapIndex(long index) {
        var result = index % (_list.Count - 1);
        
        if (result < 0) {
            while (result < 0) {
                result += _list.Count - 1;
            }
        }
        return (int) result;
    }
    
    public int IndexOf(long number) {
        return _list.IndexOf(number);
    }

    public long GetAtIndex(int index) {
        return _list[index];
    }
    
    public long[] ToArray(long startNumber) {
        var result = new long[_list.Count];

        var arrayIndex = 0;
        var startIndex = _list.IndexOf(startNumber);
        if (startIndex < 0) {
            // could not find startNumber
            startIndex = 0;
        }
        
        for (var i = startIndex; i < _list.Count; i++) {
            result[arrayIndex++] = _list[i];
        }
        for (var i = 0; i < startIndex; i++) {
            result[arrayIndex++] = _list[i];
        }

        return result;
    }
}