using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC._20;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/20">Day 20: Grove Positioning System</a>
/// </summary>
public class GrovePositioningSystem {
    private readonly int[] input;

    public GrovePositioningSystem(string[] lines) {
        input = lines.Select(int.Parse).ToArray();
    }

    public int[] CalculateGrooveCoordinates() {
        var resultingFile = MixFileAsCircularList();
        // Console.WriteLine(string.Join(", ", resultingFile.ToArray(1)));
        var indexOfZero = resultingFile.IndexOf(0);
        return new[] {
            resultingFile.GetAtIndex((indexOfZero + 1000) % input.Length), // X
            resultingFile.GetAtIndex((indexOfZero + 2000) % input.Length), // Y
            resultingFile.GetAtIndex((indexOfZero + 3000) % input.Length), // Z
        };
    }

    public int[] MixFile(int startNumber = 0) => MixFileAsCircularList().ToArray(startNumber);
    
    private CircularList MixFileAsCircularList() {
        var list  = new CircularList(input);
        foreach (var number in input) {
            list.MoveNumber(number);
        }
        return list;
    }
}

class CircularList {

    private readonly IList<int> _list;

    public CircularList(IEnumerable<int> enumerable) {
        _list = enumerable.ToList();
    }

    public void MoveNumber(int number) {
        var currentIndex = _list.IndexOf(number);
        var futureIndex = WrapIndex(currentIndex + number);

        if (currentIndex == futureIndex) {
            // nothing to do
            return;
        } 
        
        _list.RemoveAt(currentIndex);
        _list.Insert(futureIndex, number);
    }
    
    private int WrapIndex(int index) {
        var result = index % (_list.Count - 1);
        
        if (result < 0) {
            while (result < 0) {
                result += _list.Count - 1;
            }
        }
        return result;
    }
    
    public int IndexOf(int number) {
        return _list.IndexOf(number);
    }

    public int GetAtIndex(int index) {
        return _list[WrapIndex(index)];
    }
    
    public int[] ToArray(int startNumber) {
        var result = new int[_list.Count];

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