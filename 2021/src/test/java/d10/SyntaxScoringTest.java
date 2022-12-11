package d10;

import d10.SyntaxScoring.ValidationResult;
import org.junit.Assert;
import org.junit.Test;

import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.Arrays;
import java.util.List;
import java.util.Scanner;
import java.util.stream.Collectors;

public class SyntaxScoringTest {

    @Test
    public void testExample1ValidateSyntax() {
        Assert.assertEquals(ValidationResult.OK, SyntaxScoring.validateSyntax("[({(<(())[]>[[{[]{<()<>>"));
        Assert.assertEquals(ValidationResult.OK, SyntaxScoring.validateSyntax("[(()[<>])]({[<{<<[]>>("));
        Assert.assertEquals(ValidationResult.BRACE, SyntaxScoring.validateSyntax("{([(<{}[<>[]}>{[]{[(<()>"));
        Assert.assertEquals(ValidationResult.OK, SyntaxScoring.validateSyntax("(((({<>}<{<{<>}{[]{[]{}"));
        Assert.assertEquals(ValidationResult.BRACKET, SyntaxScoring.validateSyntax("[[<[([]))<([[{}[[()]]]"));
        Assert.assertEquals(ValidationResult.SQUARE_BRACKET, SyntaxScoring.validateSyntax("[{[{({}]{}}([{[{{{}}([]"));
        Assert.assertEquals(ValidationResult.OK, SyntaxScoring.validateSyntax("{<[[]]>}<{[{[{[]{()[[[]"));
        Assert.assertEquals(ValidationResult.BRACKET, SyntaxScoring.validateSyntax("[<(<(<(<{}))><([]([]()"));
        Assert.assertEquals(ValidationResult.GREATER_THAN, SyntaxScoring.validateSyntax("<{([([[(<>()){}]>(<<{{"));
        Assert.assertEquals(ValidationResult.OK, SyntaxScoring.validateSyntax("<{([{{}}[<[[[<>{}]]]>[]]"));
    }

    @Test
    public void testExample1() throws IOException {
        String[] inputLines = readInput("example.txt");

        Assert.assertEquals(26397, SyntaxScoring.calculateSyntaxErrorScore(inputLines));
    }

    private String[] readInput(String fileName) throws IOException {
        try (InputStream input = getClass().getResourceAsStream(fileName);
             Scanner scanner = new Scanner(input, StandardCharsets.UTF_8)) {
            return scanner.useDelimiter("\\A").next().split("\\R");
        }
    }

    @Test
    public void testPuzzle1() throws IOException {
        String[] inputLines = readInput("input.txt");

        int result = SyntaxScoring.calculateSyntaxErrorScore(inputLines);
        System.out.println("Puzzle 1: " + result);
        Assert.assertEquals(215229, result);
    }

    @Test
    public void testExample2TryToFixSyntax() {
        Assert.assertEquals(toFixSyntaxResult("}}]])})]"), SyntaxScoring.tryToFixSyntax("[({(<(())[]>[[{[]{<()<>>"));
        Assert.assertEquals(toFixSyntaxResult(")}>]})"), SyntaxScoring.tryToFixSyntax("[(()[<>])]({[<{<<[]>>("));
        Assert.assertEquals(toFixSyntaxResult(""), SyntaxScoring.tryToFixSyntax("{([(<{}[<>[]}>{[]{[(<()>"));
        Assert.assertEquals(toFixSyntaxResult("}}>}>))))"), SyntaxScoring.tryToFixSyntax("(((({<>}<{<{<>}{[]{[]{}"));
        Assert.assertEquals(toFixSyntaxResult(""), SyntaxScoring.tryToFixSyntax("[[<[([]))<([[{}[[()]]]"));
        Assert.assertEquals(toFixSyntaxResult(""), SyntaxScoring.tryToFixSyntax("[{[{({}]{}}([{[{{{}}([]"));
        Assert.assertEquals(toFixSyntaxResult("]]}}]}]}>"), SyntaxScoring.tryToFixSyntax("{<[[]]>}<{[{[{[]{()[[[]"));
        Assert.assertEquals(toFixSyntaxResult(""), SyntaxScoring.tryToFixSyntax("[<(<(<(<{}))><([]([]()"));
        Assert.assertEquals(toFixSyntaxResult(""), SyntaxScoring.tryToFixSyntax("<{([([[(<>()){}]>(<<{{"));
        Assert.assertEquals(toFixSyntaxResult("])}>"), SyntaxScoring.tryToFixSyntax("<{([{{}}[<[[[<>{}]]]>[]]"));
    }

    private List<ValidationResult> toFixSyntaxResult(String string) {
        return string.chars().mapToObj(c -> Arrays.stream(SyntaxScoring.VALIDATION_RESULTS).filter(v -> v.endingChar == c).findFirst().get()).collect(Collectors.toList());
    }

    @Test
    public void testExample2CalculateFixSyntaxScore() {
        Assert.assertEquals(288957, SyntaxScoring.calculateFixSyntaxScore(toFixSyntaxResult("}}]])})]")));
        Assert.assertEquals(5566, SyntaxScoring.calculateFixSyntaxScore(toFixSyntaxResult(")}>]})")));
        Assert.assertEquals(0, SyntaxScoring.calculateFixSyntaxScore(toFixSyntaxResult("")));
        Assert.assertEquals(1480781, SyntaxScoring.calculateFixSyntaxScore(toFixSyntaxResult("}}>}>))))")));
        Assert.assertEquals(995444, SyntaxScoring.calculateFixSyntaxScore(toFixSyntaxResult("]]}}]}]}>")));
        Assert.assertEquals(294, SyntaxScoring.calculateFixSyntaxScore(toFixSyntaxResult("])}>")));
    }

    @Test
    public void testExample2() throws IOException {
        String[] inputLines = readInput("example.txt");

        Assert.assertEquals(288957, SyntaxScoring.calculateClosingScore(inputLines));
    }

    @Test
    public void testPuzzle2() throws IOException {
        String[] inputLines = readInput("input.txt");

        long result = SyntaxScoring.calculateClosingScore(inputLines);
        System.out.println("Puzzle 2: " + result);
        Assert.assertEquals(1105996483, result);
    }
}
