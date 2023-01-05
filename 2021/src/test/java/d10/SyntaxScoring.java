package d10;

import org.jetbrains.annotations.NotNull;
import org.junit.Assert;

import java.util.*;
import java.util.stream.Stream;

/**
 * <a href="https://adventofcode.com/2021/day/10">Day 10: Syntax Scoring</a>: You ask the submarine to determine the
 * best route out of the deep-sea cave, but it only replies:
 *
 * Syntax error in navigation subsystem on line: all of them
 *
 * All of them?! The damage is worse than you thought. You bring up a copy of the navigation subsystem (your puzzle
 * input).
 */
public final class SyntaxScoring {

    enum ValidationResult {
        OK(0, 0, ' ', ' '),
        BRACKET(3, 1,'(', ')'), // )
        SQUARE_BRACKET(57, 2, '[', ']'), // ]
        BRACE(1197, 3, '{', '}'),  // }
        GREATER_THAN(25137, 4, '<', '>'), // >
        ;
        public final int syntaxErrorScore;
        public final int closingScore;
        public final char startingChar;
        public final char endingChar;
        ValidationResult(int syntaxErrorScore, int closingScore, char startingChar, char endingChar) {
            this.syntaxErrorScore = syntaxErrorScore;
            this.closingScore = closingScore;
            this.startingChar = startingChar;
            this.endingChar = endingChar;
        }
    }
    static final ValidationResult[] VALIDATION_RESULTS = ValidationResult.values();

    public static int calculateSyntaxErrorScore(String[] lines) {
        return Stream.of(lines)
                .map(SyntaxScoring::validateSyntax)
                .mapToInt(r -> r.syntaxErrorScore)
                .sum();
    }

    static ValidationResult validateSyntax(String line) {
        String currentLine = line;
        do {
            String newLine = currentLine;
            // replace all matching brackets
            newLine = replaceMatchingBrackets(newLine);

            // replace opening brackets that where not closed
            newLine = newLine.replaceFirst("\\($", "");
            newLine = newLine.replaceFirst("\\[$", "");
            newLine = newLine.replaceFirst("\\{$", "");
            newLine = newLine.replaceFirst("<$", "");

            if (newLine.equals(currentLine)) {
                break;
            }
            currentLine = newLine;
        } while (true);

        // if the string now has a closing bracket in it, then it is not ok
        int size = currentLine.length();
        for (int i = 0; i < size; i++) {
            char c = currentLine.charAt(i);
            Optional<ValidationResult> validationResult = Stream.of(VALIDATION_RESULTS).filter(r -> c == r.endingChar).findFirst();
            if (validationResult.isPresent()) {
                return validationResult.get();
            }
        }
        return ValidationResult.OK;
    }

    @NotNull
    private static String replaceMatchingBrackets(String newLine) {
        newLine = newLine.replaceFirst("\\(\\)", "");
        newLine = newLine.replaceFirst("\\[\\]", "");
        newLine = newLine.replaceFirst("\\{\\}", "");
        newLine = newLine.replaceFirst("<>", "");
        return newLine;
    }

    public static long calculateClosingScore(String[] lines) {
        long[] scores = Stream.of(lines)
                .map(SyntaxScoring::tryToFixSyntax)
                .mapToLong(SyntaxScoring::calculateFixSyntaxScore)
                .filter(i -> i != 0)
                .sorted()
                .toArray();
        Assert.assertEquals("There will always be an odd number of scores to consider!", 1, scores.length % 2);
        Assert.assertFalse("The score cannot be negative (possibly int overflow?)!", Arrays.stream(scores).anyMatch(i -> i < 0));
        return scores[scores.length / 2];
    }

    public static long calculateFixSyntaxScore(List<ValidationResult> fixes) {
        return fixes.stream()
                .mapToLong(r -> r.closingScore)
                // this is that weird rule:
                // "Start with a total score of 0. Multiply the total score by 5 to get 0, then add the value of the bracket"
                .reduce(0, (x, y) -> x * 5 + y);
    }

    static List<ValidationResult> tryToFixSyntax(String line) {
        List<ValidationResult> possibleFixes = new ArrayList<>();
        String currentLine = line;
        do {
            String newLine = currentLine;
            // replace all matching brackets
            newLine = replaceMatchingBrackets(newLine);

            // replace opening brackets that where not closed
            for (ValidationResult validationResult : VALIDATION_RESULTS) {
                if (newLine.endsWith(Character.toString(validationResult.startingChar))) {
                    // this bracket is missing its partner - we remove it and remember what was necessary to fix it
                    possibleFixes.add(validationResult);
                    newLine = newLine.substring(0, newLine.length() - 1);
                }
            }

            if (newLine.equals(currentLine)) {
                break;
            }
            currentLine = newLine;
        } while (true);

        // if the string now has a closing bracket in it, then we can't fix it
        int size = currentLine.length();
        for (int i = 0; i < size; i++) {
            char c = currentLine.charAt(i);
            Optional<ValidationResult> validationResult = Stream.of(VALIDATION_RESULTS).filter(r -> c == r.endingChar).findFirst();
            if (validationResult.isPresent()) {
                return Collections.emptyList();
            }
        }
        // if not, we can fix it, so return our idea on how
        return possibleFixes;
    }

    private SyntaxScoring() {
        // hide this constructor
    }
}
