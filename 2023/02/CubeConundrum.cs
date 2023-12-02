using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/2">Day 2: Cube Conundrum</a>: The Elf would first like to know which games would have been possible if the
/// bag contained only 12 red cubes, 13 green cubes, and 14 blue cubes?
/// </summary>
public class CubeConundrum {
    public record Draw {
        public int[] Colors { get; } = new int[MAX_COLORS];
        public int Red { get => Colors[RED]; set => Colors[RED] = value; }
        public int Blue { get => Colors[BLUE]; set => Colors[BLUE] = value; }
        public int Green { get => Colors[GREEN]; set => Colors[GREEN] = value; }

        public long CalculatePower() {
            return Colors.Aggregate(1L, (current, color) => current * color);
        }
    }

    public record Game(int Id, params Draw[] Draws) {
        public bool IsPossibleForBagContent(Draw content) {
            foreach (var draw in Draws) {
                for (var i = 0; i < MAX_COLORS; i++) {
                    if (content.Colors[i] < draw.Colors[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public long CalculatePower() {
            var bagContent = new Draw();
            foreach (var draw in Draws) {
                for (var i = 0; i < MAX_COLORS; i++) {
                    bagContent.Colors[i] = Math.Max(bagContent.Colors[i], draw.Colors[i]);
                }
            }

            return bagContent.CalculatePower();
        }
    }

    private const int RED = 0;
    private const int BLUE = 1;
    private const int GREEN = 2;
    private const int MAX_COLORS = 3;

    public CubeConundrum(IEnumerable<string> input) {
        Games = ParseGames(input);
    }

    public Game[] Games { get; }

    private static Game[] ParseGames(IEnumerable<string> input) {
        return input.Select(ParseGame).ToArray();
    }

    internal static Game ParseGame(string input) {
        var gameAndDraws = input.Split(":");
        var id = int.Parse(gameAndDraws[0][5..]);
        return new Game(id, gameAndDraws[1].Split(";").Select(ParseDraw).ToArray());
    }

    private static Draw ParseDraw(string input) {
        var colors = input.Split(",");
        var result = new Draw();
        foreach (var color in colors) {
            if (color.Contains("red")) {
                result.Red = ExtractNumber(color);
            } else if (color.Contains("blue")) {
                result.Blue = ExtractNumber(color);
            } else if (color.Contains("green")) {
                result.Green = ExtractNumber(color);
            }
        }

        return result;
    }

    private static int ExtractNumber(string color) {
        return int.Parse(Regex.Replace(color, "[^0-9]+", ""));
    }

    public IEnumerable<Game> FindGamesPossibleWithBagContent(Draw draw) {
        return Games.Where(g => g.IsPossibleForBagContent(draw));
    }

    public long CalculatePower() {
        return Games.Select(g => g.CalculatePower()).Sum();
    }
}