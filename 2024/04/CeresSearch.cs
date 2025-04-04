﻿using System;
using System.Collections.Generic;

namespace AoC.day4;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/4">Day 4: Ceres Search</a>
/// </summary>
public class CeresSearch {

    private const int InvalidCoord = -1;
    
    internal struct Direction(CeresSearch parent, string name, Func<int, int> modifyX, Func<int, int> modifyY) {
        public (int X, int Y) Modify(int oldX, int oldY) => (
            Sanitize(modifyX(oldX), parent.Input.Length), 
            Sanitize(modifyY(oldY), parent.Input[0].Length)
        );
        private static int Sanitize(int coord, int length) => coord < 0 || coord >= length ? InvalidCoord : coord;
        public override string ToString() => name;
    }

    public CeresSearch(IEnumerable<string> input) {
        Input = input.ParseCharMatrix();

        NorthEast = new Direction(this, "NE", x => x + 1, y => y - 1);
        SouthEast = new Direction(this, "SE", x => x + 1, y => y + 1);
        NorthWest = new Direction(this, "NW", x => x - 1, y => y - 1);
        SouthWest = new Direction(this, "SW", x => x - 1, y => y + 1);
        
        AllDirections = [
            new Direction(this, "North", x => x, y => y - 1),
            new Direction(this, "South", x => x, y => y + 1),
            new Direction(this, "West", x => x - 1, y => y),
            new Direction(this, "East", x => x + 1, y => y),
            
            NorthEast,
            SouthEast,
            NorthWest,
            SouthWest,
        ];

        OppositeDirections = new Dictionary<Direction, Direction> {
            {NorthEast, SouthWest}, {SouthWest, NorthEast}, {NorthWest, SouthEast}, {SouthEast, NorthWest},
        };
    }

    internal char[][] Input { get; }
    private Direction[] AllDirections { get; }
    private Direction NorthEast { get; }
    private Direction NorthWest { get; }
    private Direction SouthEast { get; }
    private Direction SouthWest { get; }
    private IDictionary<Direction, Direction> OppositeDirections { get; }

    public long CalculateXmasCount() {
        long result = 0;
        for (var x = 0; x < Input.Length; x++) {
            for (var y = 0; y < Input[x].Length; y++) {
                result += CalculateWordCount(x, y, "XMAS", AllDirections);
            }
        }
        return result;
    }
    
    private long CalculateWordCount(int x, int y, string word, params Direction[] allDirections) {
        long result = 0;
        foreach (var direction in allDirections) {
            var foundWord = false;
            var index = 0;
            var (currentX, currentY) = (x, y);
            
            while (Input[currentX][currentY] == word[index]) {
                index++;
                if (index >= word.Length) {
                    foundWord = true;
                    break;
                }   
                
                (currentX, currentY) = direction.Modify(currentX, currentY);
                if (currentX == InvalidCoord || currentY == InvalidCoord) break;
            }

            if (foundWord) {
                result++;
                Console.WriteLine($"({x}, {y}): {direction}");
            }
        }
        return result;
    }
    
    public long CalculateCrossMasCount() {
        long result = 0;
        for (var x = 0; x < Input.Length; x++) {
            for (var y = 0; y < Input[x].Length; y++) {
                result += CalculateCrossCount(x, y);
            }
        }
        return result;
    }
    
    private long CalculateCrossCount(int x, int y) {
        // if this character is not A, there cannot be a cross surrounding it
        if (Input[x][y] != 'A') return 0;

        var masCount = 0L;
        foreach (var (direction, opposite) in OppositeDirections) {
            // we check all diagonal directions if they have a MAS in the opposite direction
            var (startX, startY) = direction.Modify(x, y);
            if (startX == InvalidCoord || startY == InvalidCoord) break;
            masCount += CalculateWordCount(startX, startY, "MAS", opposite);
        }

        // we can find at most 2 "MAS", and that means it's a cross
        return masCount / 2;
    }
}
