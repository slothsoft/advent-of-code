using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/3">Day 3: Gear Ratios</a>: What is the sum of all of the part numbers in the engine schematic?
/// </summary>
public static class GearRatios {
    public record EngineSchematic(char[][] Symbols, PartNumber[] PartNumbers) {
        internal IEnumerable<PartNumber> FetchPartNumbersNextToSymbols() => PartNumbers.Where(n => n.IsNextToSymbol(Symbols));

        internal IEnumerable<long> FetchGearRatios() {
            return FetchGearCoordinates().Select(g => (long) g.PartNumbers[0].Value * g.PartNumbers[1].Value);
        }
        
        internal IEnumerable<(int X, int Y, PartNumber[] PartNumbers)> FetchGearCoordinates() {
            return FetchAsterixCoordinates()
                .Select(c => (c.X, c.Y, PartNumbers: PartNumbers.Where(n => n.IsNextTo(c)).ToArray()))
                .Where(c => c.PartNumbers.Length == 2);
        }
        
        internal IEnumerable<(int X, int Y)> FetchAsterixCoordinates() {
            for (var x = 0; x < Symbols.Length; x++) {
                for (var y = 0; y < Symbols[x].Length; y++) {
                    if (Symbols[x][y] == '*') {
                        yield return (x, y);
                    }
                }
            }
        }
    }

    public record PartNumber(int X, int Y, int Value) {
        public int Length { get => Value.ToString().Length; }

        internal bool IsNextTo((int X, int Y) coordinates) {
            var bounds = FetchBounds();
            return coordinates.X >= bounds.MinX
                   && coordinates.X < bounds.MaxX
                   && coordinates.Y >= bounds.MinY
                   && coordinates.Y < bounds.MaxY;
        }

        internal bool IsNextToSymbol(char[][] symbols) {
            var bounds = FetchBounds(symbols.Length, symbols[0].Length);

            for (var x = bounds.MinX; x < bounds.MaxX; x++) {
                for (var y = bounds.MinY; y < bounds.MaxY; y++) {
                    if (char.IsDigit(symbols[x][y])) {
                        // char is probably this very same number
                        continue;
                    }

                    if (symbols[x][y] == '.') {
                        // Periods (.) do not count as a symbol.
                        continue;
                    }

                    return true;
                }
            }

            return false;
        }
        
        private (int MinX, int MinY, int MaxX, int MaxY) FetchBounds(int width = int.MaxValue, int height = int.MaxValue) {
            var minX = Math.Max(0, X - 1);
            var maxX = Math.Min(width, X + Length + 1);
            var minY = Math.Max(0, Y - 1);
            var maxY = Math.Min(height, Y + 2);
            return (minX, minY, maxX, maxY);
        }
    }

    internal static EngineSchematic ParseEngineSchematic(IEnumerable<string> input) {
        var symbols = input.ParseCharMatrix();
        var partNumbers = new List<PartNumber>();

        for (var y = 0; y < symbols[0].Length; y++) {
            var currentNumber = string.Empty;
            for (var x = 0; x < symbols.Length; x++) {
                if (char.IsDigit(symbols[x][y])) {
                    currentNumber += symbols[x][y];
                } else if (currentNumber.Length > 0) {
                    partNumbers.Add(new PartNumber(x - currentNumber.Length, y, int.Parse(currentNumber)));
                    currentNumber = string.Empty;
                }
            }

            // add end of line number
            if (currentNumber.Length > 0) {
                partNumbers.Add(new PartNumber(symbols.Length - currentNumber.Length, y, int.Parse(currentNumber)));
            }
        }

        return new EngineSchematic(symbols, partNumbers.ToArray());
    }
}