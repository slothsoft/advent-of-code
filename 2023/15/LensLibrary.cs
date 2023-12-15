using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/15">Day 15: Hot Springs</a>:  Unfortunately, the reindeer has stolen the page containing the expected
/// verification number and is currently running around the facility with it excitedly.
/// </summary>
public class LensLibrary {

    public class Box {
        private IList<string> _lenses = new List<string>();
        private IList<int> _focalLengths = new List<int>();

        public void RemoveLens(string lens) {
            var index = _lenses.IndexOf(lens);
            if (index >= 0) {
                _lenses.RemoveAt(index);
                _focalLengths.RemoveAt(index);
            }
        }
        
        public void AddLens(string lens, int focalLength) {
            var index = _lenses.IndexOf(lens);
            if (index < 0) {
                _lenses.Add(lens);
                _focalLengths.Add(focalLength);
            } else {
                _focalLengths.RemoveAt(index);
                _focalLengths.Insert(index, focalLength);
            }
        }

        public long CalculateFocalLength() {
            var result = 0L;
            for (var i = 0; i < _focalLengths.Count; i++) {
                result += (i + 1) * _focalLengths[i];
            }

            return result;
        }
    }
    
    public const int BoxCount = 256;
    public const char OperatorRemove = '-';
    public const char OperatorAdd = '=';
    
    private string[] _input;
    private Box?[] _boxes = new Box[BoxCount];

    public LensLibrary(string input) {
        _input = input.Split(",");
    }

    public static int CalculateHash(string input) {
        var result = 0;
        foreach (var c in input) {
            result += c;
            result = (result * 17) % BoxCount;
        }

        return result;
    }

    public static long CalculateHashFromInitSequence(string input) {
        return input.Split(",").Select(CalculateHash).Sum();
    }
    
    public void ExecuteInitializationSequence() {
        foreach (var command in _input) {
            ExecuteCommand(command);
        }
    }
    
    private void ExecuteCommand(string command) {
        var split = command.Split(OperatorAdd, OperatorRemove);
        var boxIndex = CalculateHashFromInitSequence(split[0]);
        _boxes[boxIndex] ??= new Box();
        
        if (command[^1] == OperatorRemove) {
            _boxes[boxIndex]!.RemoveLens(split[0]);
        } else if (command.Contains(OperatorAdd)) {
            _boxes[boxIndex]!.AddLens(split[0], int.Parse(split[1]));
        } else {
            throw new ArgumentException("Do not know what to do with: " + command);
        }
    }
    
    public long CalculateFocusingPower() {
        var result = 0L;
        for (var i = 0; i < _boxes.Length; i++) {
            result += (i + 1) * (_boxes[i]?.CalculateFocalLength() ?? 0);
        }

        return result;
    }
}