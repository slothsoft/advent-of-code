using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC;

public class CubeConundrumTest {
    private static readonly string[] ExampleInput1 = {
        "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
        "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",
        "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",
        "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
        "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green"
    };

    [Test]
    public void Example1ParseGame1() {
        var game = CubeConundrum.ParseGame("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green");
        Assert.AreEqual(1, game.Id);
        Assert.AreEqual(3, game.Draws.Length);

        var draw = game.Draws[0];
        Assert.AreEqual(4, draw.Red);
        Assert.AreEqual(0, draw.Green);
        Assert.AreEqual(3, draw.Blue);
        
        draw = game.Draws[1];
        Assert.AreEqual(1, draw.Red);
        Assert.AreEqual(2, draw.Green);
        Assert.AreEqual(6, draw.Blue);
        
        draw = game.Draws[2];
        Assert.AreEqual(0, draw.Red);
        Assert.AreEqual(2, draw.Green);
        Assert.AreEqual(0, draw.Blue);
    }
    
    [Test]
    [TestCase(1, true)]
    [TestCase(2, true)]
    [TestCase(3, false)]
    [TestCase(4, false)]
    [TestCase(5, true)]
    public void Example1IsPossible(int gameId, bool expected) {
        var example = new CubeConundrum(ExampleInput1);
        var draw = new CubeConundrum.Draw {Red = 12, Green = 13, Blue = 14};

        var game = example.Games.SingleOrDefault(g => g.Id == gameId);
        Assert.NotNull(game, $"Could not find game with ID {gameId}");
        Assert.AreEqual(expected, game!.IsPossibleForBagContent(draw));
    }

    [Test]
    public void Example1() {
        var example = new CubeConundrum(ExampleInput1);
        var bagContent = new CubeConundrum.Draw {Red = 12, Green = 13, Blue = 14};

        Assert.AreEqual(8, example.FindGamesPossibleWithBagContent(bagContent).Sum(g => g.Id));
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new CubeConundrum(File.ReadAllLines(@"02\input.txt"));
        var bagContent = new CubeConundrum.Draw {Red = 12, Green = 13, Blue = 14};

        Assert.AreEqual(2061, puzzle.FindGamesPossibleWithBagContent(bagContent).Sum(g => g.Id));
    }

    [Test]
    [TestCase(1, 48)]
    [TestCase(2, 12)]
    [TestCase(3, 1560)]
    [TestCase(4, 630)]
    [TestCase(5, 36)]
    public void Example2CalculatePower(int gameId, long expectedPower) {
        var example = new CubeConundrum(ExampleInput1);

        var game = example.Games.SingleOrDefault(g => g.Id == gameId);
        Assert.NotNull(game, $"Could not find game with ID {gameId}");
        Assert.AreEqual(expectedPower, game!.CalculatePower());
    }
    
    [Test]
    public void Example2() {
        var example = new CubeConundrum(ExampleInput1);

        Assert.AreEqual(2286, example.CalculatePower());
    }

    [Test]
    public void Puzzle2() {
        var example = new CubeConundrum(File.ReadAllLines(@"02\input.txt"));

        Assert.AreEqual(72596, example.CalculatePower());
    }
}