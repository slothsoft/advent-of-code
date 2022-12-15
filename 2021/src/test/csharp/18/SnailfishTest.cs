using System.IO;
using NUnit.Framework;

namespace AoC._18;

public class SnailfishTest {
    
    [Test]
    [TestCase("[1,2]", "[[3,4],5]", "[[1,2],[[3,4],5]]")]
    [TestCase("[[[[4,3],4],4],[7,[[8,4],9]]]", "[1,1]", "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]")]
    public void Example1Add(string leftString, string rightString, string resultString) {
        var left = Snailfish.ParseNumber(leftString);
        var right = Snailfish.ParseNumber(rightString);
        var result = left.Add(right);
        
        Assert.AreEqual(resultString, result.Stringify());
    }

    [Test]
    [TestCase("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]")]
    [TestCase("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]")]
    [TestCase("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]")]
    [TestCase("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")]
    [TestCase("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")]
    [TestCase("[20,14]", "[[[5,5],[5,5]],[7,7]]")]
    [TestCase("[1,[2,[[20,14],3]]]", "[1,[[6,[6,6]],[[0,6],[7,8]]]]")]
    public void Example1Reduce(string inputString, string expectedResult) {
        var input = Snailfish.ParseNumber(inputString);
        input.Reduce();
        Assert.AreEqual(expectedResult, input.Stringify());
    }

    [Test]
    [TestCase("[1,1]\n[2,2]\n[3,3]\n[4,4]", "[[[[1,1],[2,2]],[3,3]],[4,4]]")]
    [TestCase("[1,1]\n[2,2]\n[3,3]\n[4,4]\n[5,5]", "[[[[3,0],[5,3]],[4,4]],[5,5]]")]
    [TestCase("[1,1]\n[2,2]\n[3,3]\n[4,4]\n[5,5]\n[6,6]", "[[[[5,0],[7,4]],[5,5]],[6,6]]")]
    public void Example1Sum(string input, string expectedResult) {
        var numbers = Snailfish.ParseNumbers(input.Split("\n"));
        Assert.AreEqual(expectedResult, numbers.Sum().Stringify());
    }
    
    [Test]
    public void Example1LargerExampleInParts() {
        var number = Snailfish.ParseNumber("[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]");
        
        number = number.Add(Snailfish.ParseNumber("[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]"));
        Assert.AreEqual("[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]", number.Stringify());
        
        number = number.Add(Snailfish.ParseNumber("[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]"));
        Assert.AreEqual("[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]", number.Stringify());
        
        number = number.Add(Snailfish.ParseNumber("[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]"));
        Assert.AreEqual("[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]", number.Stringify());

        number = number.Add(Snailfish.ParseNumber("[7,[5,[[3,8],[1,4]]]]"));
        Assert.AreEqual("[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]", number.Stringify());

        number = number.Add(Snailfish.ParseNumber("[[2,[2,2]],[8,[8,1]]]"));
        Assert.AreEqual("[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]", number.Stringify());
        
        number = number.Add(Snailfish.ParseNumber("[2,9]"));
        Assert.AreEqual("[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]", number.Stringify());
        
        number = number.Add(Snailfish.ParseNumber("[1,[[[9,3],9],[[9,0],[0,7]]]]"));
        Assert.AreEqual("[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]", number.Stringify());
        
        number = number.Add(Snailfish.ParseNumber("[[[5,[7,4]],7],1]"));
        Assert.AreEqual("[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]", number.Stringify());
        
        number = number.Add(Snailfish.ParseNumber("[[[[4,2],2],6],[8,7]]"));
        Assert.AreEqual("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", number.Stringify());
    }
    
    [Test]
    public void Example1LargerExample() {
        var numbers = Snailfish.ParseNumbers(File.ReadAllLines(@"18\exampleA.txt"));
        Assert.AreEqual("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", numbers.Sum().Stringify());
    }

    [Test]
    [TestCase("[9,1]", 29)]
    [TestCase("[1,9]", 21)]
    [TestCase("[[9,1],[1,9]]", 129)]
    [TestCase("[[1,2],[[3,4],5]]", 143)]
    [TestCase("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]", 1384)]
    [TestCase("[[[[1,1],[2,2]],[3,3]],[4,4]]", 445)]
    [TestCase("[[[[3,0],[5,3]],[4,4]],[5,5]]", 791)]
    [TestCase("[[[[5,0],[7,4]],[5,5]],[6,6]]", 1137)]
    [TestCase("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", 3488)]
    public void Example1Magnitude(string inputString, int expectedResult) {
        var input = Snailfish.ParseNumber(inputString);
        Assert.AreEqual(expectedResult, input.Magnitude);
    }
    
    [Test]
    public void Example1() {
        var numbers = Snailfish.ParseNumbers(File.ReadAllLines(@"18\exampleB.txt"));
        Assert.AreEqual(4140, numbers.Sum().Magnitude);
    }
    
    [Test]
    public void Puzzle1() {
        var numbers = Snailfish.ParseNumbers(File.ReadAllLines(@"18\input.txt"));
        var result = numbers.Sum().Magnitude;
        Assert.AreEqual(3892, result);
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    public void Example2() {
        var numbers = Snailfish.ParseNumbers(File.ReadAllLines(@"18\exampleB.txt"));
        var result = numbers.CalculateLargestMagnitude();
        Assert.AreEqual(3993, result);
    }

    [Test]
    public void Puzzle2() {
        var numbers = Snailfish.ParseNumbers(File.ReadAllLines(@"18\input.txt"));
        var result = numbers.CalculateLargestMagnitude();
        Assert.AreEqual(4909, result);
        Assert.Pass("Puzzle 1: " + result);
    }
}