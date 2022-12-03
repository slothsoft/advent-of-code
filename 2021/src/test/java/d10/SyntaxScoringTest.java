package d10;

import org.junit.Assert;
import org.junit.Test;

import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.Scanner;

public class SyntaxScoringTest {

    @Test
    public void testExample1ValidateSyntax() {
        Assert.assertEquals(SyntaxScoring.ValidationResult.OK, SyntaxScoring.validateSyntax("[({(<(())[]>[[{[]{<()<>>"));
        Assert.assertEquals(SyntaxScoring.ValidationResult.OK, SyntaxScoring.validateSyntax("[(()[<>])]({[<{<<[]>>("));
        Assert.assertEquals(SyntaxScoring.ValidationResult.BRACE, SyntaxScoring.validateSyntax("{([(<{}[<>[]}>{[]{[(<()>"));
        Assert.assertEquals(SyntaxScoring.ValidationResult.OK, SyntaxScoring.validateSyntax("(((({<>}<{<{<>}{[]{[]{}"));
        Assert.assertEquals(SyntaxScoring.ValidationResult.BRACKET, SyntaxScoring.validateSyntax("[[<[([]))<([[{}[[()]]]"));
        Assert.assertEquals(SyntaxScoring.ValidationResult.SQUARE_BRACKET, SyntaxScoring.validateSyntax("[{[{({}]{}}([{[{{{}}([]"));
        Assert.assertEquals(SyntaxScoring.ValidationResult.OK, SyntaxScoring.validateSyntax("{<[[]]>}<{[{[{[]{()[[[]"));
        Assert.assertEquals(SyntaxScoring.ValidationResult.BRACKET, SyntaxScoring.validateSyntax("[<(<(<(<{}))><([]([]()"));
        Assert.assertEquals(SyntaxScoring.ValidationResult.GREATER_THAN, SyntaxScoring.validateSyntax("<{([([[(<>()){}]>(<<{{"));
        Assert.assertEquals(SyntaxScoring.ValidationResult.OK, SyntaxScoring.validateSyntax("<{([{{}}[<[[[<>{}]]]>[]]"));
    }

    @Test
    public void testExample1() throws IOException {
        String[] inputLines = readInput("example.txt");

        Assert.assertEquals(26397, SyntaxScoring.calculateSyntaxErrorScore(inputLines));
    }

    private String[] readInput(String fileName) throws IOException {
        try (InputStream input = getClass().getResourceAsStream(fileName);
             Scanner scanner = new Scanner(input, StandardCharsets.UTF_8.name())) {
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

}
