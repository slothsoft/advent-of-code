package d10;

import java.util.Optional;
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
        public int syntaxErrorScore;
        public int closingScore;
        public char startingChar;
        public char endingChar;
        ValidationResult(int syntaxErrorScore, int closingScore, char startingChar, char endingChar) {
            this.syntaxErrorScore = syntaxErrorScore;
            this.closingScore = closingScore;
            this.startingChar = startingChar;
            this.endingChar = endingChar;
        }
    }
    private static final ValidationResult[] VALIDATION_RESULTS = ValidationResult.values();

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
            newLine = newLine.replaceFirst("\\(\\)", "");
            newLine = newLine.replaceFirst("\\[\\]", "");
            newLine = newLine.replaceFirst("\\{\\}", "");
            newLine = newLine.replaceFirst("<>", "");

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

        // if the string now has a closing bracket in it, then the char before it is not ok
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

    private SyntaxScoring() {
        // hide this constructor
    }
}
