using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._17;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/16">Day 17: Pyroclastic Flow</a>
/// </summary>
public class PyroclasticFlow {
    private static readonly string[] Stones = {
        "####",
        ".#.\n###\n.#.",
        "..#\n..#\n###",
        "#\n#\n#\n#",
        "##\n##"
    };

    private Tetris _tetris;
    private int _stoneIndex;
    private readonly string _jetPattern;
    private int _jetPatternIndex;

    public PyroclasticFlow(string jetPattern) {
        _jetPattern = jetPattern;
        _tetris = new Tetris(7, 4);
    }

    internal Stone? Stone => _tetris.Stone;
    public int Height => _tetris.Height - _tetris.HighestBlockY;

    public void ReleaseRocks(int rocks) {
        for (var i = 0; i < rocks; i++) {
            ReleaseRock();
        }
    }

    /// <summary>
    /// </summary>
    /// <returns>difference in HighestBlockY</returns>
    internal int ReleaseRock() {
        var stoneHeight = 0;
        var result = _tetris.HighestBlockY;
        while (PerformStep()) {
            // should return false when the rock arrives at the floor
            stoneHeight = _tetris.Stone?.Height ?? 0;
        }

        result = result - _tetris.HighestBlockY;
        // add more rows at top
        for (var i = 0; i < stoneHeight; i++) {
            _tetris.AddRowAtTop();
        }

        CreateRock();
        return result;
    }

    internal bool PerformStep() {
        if (_tetris.Stone == null) {
            // the first step is to create a new rock
            CreateRock();
            return true;
        }

        PerformJetPatternStep();
        return PerformGravityStep();
    }

    private void CreateRock() {
        var stone = Stones[_stoneIndex++ % Stones.Length];
        _tetris.CreateStone(stone, 2, _tetris.HighestBlockY - 3 - stone.Split("\n").Length);
    }

    internal void PerformJetPatternStep() {
        var jet = _jetPattern[_jetPatternIndex++ % _jetPattern.Length];
        switch (jet) {
            case '>':
                _tetris.MoveStoneRight();
                break;
            case '<':
                _tetris.MoveStoneLeft();
                break;
            default: throw new ArgumentException("Do not know jet " + jet);
        }
    }

    public bool PerformGravityStep() {
        return _tetris.MoveStoneDown();
    }

    public string Stringify() {
        return _tetris.Stringify();
    }

    public long CalculateRocksHeight(long rocks) {
        const int maxSimulateRocks = 6000;
        if (rocks < maxSimulateRocks) {
            ReleaseRocks((int) rocks);
            return _tetris.HighestBlockY;
        }

        var (startOfRepetition, repetitionLength, additionalHighestBlockY) = CalculateRepetition(maxSimulateRocks);

        // calculate the height of the non-repeating start
        rocks -= startOfRepetition;
        var result = (long) additionalHighestBlockY.Take(startOfRepetition).Sum();

        // calculate the repetition height
        var repetitionHeight = (long) additionalHighestBlockY.Skip(startOfRepetition).Take(repetitionLength).Sum();
        var fullRepetitions = rocks / repetitionLength;
        rocks -= fullRepetitions * repetitionLength;
        result += fullRepetitions * repetitionHeight;

        // calculate the remainder
        var remainderHeight = (long) additionalHighestBlockY.Skip(startOfRepetition).Take((int) rocks).Sum();
        result += remainderHeight;

        return result;
    }

    private (int, int, IList<int>) CalculateRepetition(int simulateRocks = 6000) {
        // we know the pattern repeats about every 2600 lines, starting with line ~300

        // simulate part of the rock dropping
        _tetris = new Tetris(7, 4);
        var additionalHighestBlockY = new List<int>();
        var repetitionLength = -1;
        var startOfRepetition = 300;

        // drop some more rocks if repetition could not be found yet
        for (var i = 0; i < simulateRocks; i++) {
            additionalHighestBlockY.Add(ReleaseRock());
        }

        // find the point where the sequence repeats
        for (var l = startOfRepetition + 10; l < additionalHighestBlockY.Count; l++) {
            if (additionalHighestBlockY[startOfRepetition] == additionalHighestBlockY[l]) {
                var allEqual = true;
                for (var i = 0; i < l - startOfRepetition; i++) {
                    if (l + i >= additionalHighestBlockY.Count) {
                        allEqual = false;
                        break;
                    }

                    if (additionalHighestBlockY[startOfRepetition + i] != additionalHighestBlockY[l + i]) {
                        allEqual = false;
                        break;
                    }
                }

                if (allEqual) {
                    repetitionLength = l - startOfRepetition;
                }
            }
        }

        // move the start of the repetition closer to the start of the list
        for (var l = startOfRepetition - 1; l >= 0; l--) {
            if (additionalHighestBlockY[l] != additionalHighestBlockY[l + repetitionLength]) {
                startOfRepetition = l + 1;
                break;
            }
        }

        Console.WriteLine();
        Console.WriteLine("startOfRepetition = " + startOfRepetition);
        Console.WriteLine("repetitionLength = " + repetitionLength);

        return (startOfRepetition, repetitionLength, additionalHighestBlockY);
    }
}