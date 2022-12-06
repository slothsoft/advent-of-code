package d08;

import org.junit.Assert;

import java.util.*;
import java.util.stream.Collectors;
import java.util.stream.IntStream;
import java.util.stream.Stream;

/**
 * <a href="https://adventofcode.com/2021/day/8">Day 8: Seven Segment Search</a>: You barely reach the safety of the
 * cave when the whale smashes into the cave mouth, collapsing it. Sensors indicate another exit to this cave at a much
 * greater depth, so you have no choice but to press on.
 * <p>
 * As your submarine slowly makes its way through the cave system, you notice that the four-digit seven-segment displays
 * in your submarine are malfunctioning; they must have been damaged during the escape. You'll be in a lot of trouble
 * without them, so you'd better figure out what's wrong.
 */
public final class SevenSegmentSearch {

    enum DisplayNumber {
        ZERO(0, "ABCEFG"),
        ONE(1, "CF"),
        TWO(2, "ACDEG"),
        THREE(3, "ACDFG"),
        FOUR(4, "BCDF"),
        FIVE(5, "ABDFG"),
        SIX(6, "ABDEFG"),
        SEVEN(7, "ACF"),
        EIGHT(8, "ABCDEFG"),
        NINE(9, "ABCDFG"),
        ;

        final int number;
        final String segments;

        DisplayNumber(int number, String segments) {
            this.number = number;
            this.segments = segments;
        }
    }
    private static final DisplayNumber[] DISPLAY_NUMBERS = DisplayNumber.values();

    public static long decodeBigNumbers(String[] lines) {
        return Arrays.stream(lines).mapToLong(line -> Long.parseLong(decodeNumbers(line).mapToObj(i -> "" + i).collect(Collectors.joining("")))).sum();
    }

    public static long decodeAndCountNumbers(String[] lines, int[] numbersToCount) {
        List<Integer> numbers = Arrays.stream(numbersToCount).mapToObj(i -> i).collect(Collectors.toList());
        return decodeNumbers(lines).filter(i -> numbers.contains(i)).count();
    }

    static IntStream decodeNumbers(String[] lines) {
        return Arrays.stream(lines).flatMapToInt(SevenSegmentSearch::decodeNumbers);
    }

    static IntStream decodeNumbers(String line) {
        String[] lineSplit = line.split(" \\| ");
        Map<Character, Character> decodeMap = decode(lineSplit[0].split(" "));
        return Stream.of(lineSplit[1].split(" "))
                .map(s -> createNumber(decodeMap, s))
                .filter(i -> i != null)
                .mapToInt(i -> i);
    }

    static Map<Character, Character> decode(String[] brokenSegments) {
        Map<Character, Character> result = new HashMap<>();

        String brokenOne = Stream.of(brokenSegments).filter(s -> s.length() == 2).findFirst().get();
        String brokenFour = Stream.of(brokenSegments).filter(s -> s.length() == 4).findFirst().get();
        String brokenSeven = Stream.of(brokenSegments).filter(s -> s.length() == 3).findFirst().get();
        String brokenEight = Stream.of(brokenSegments).filter(s -> s.length() == 7).findFirst().get();

        // 1 and 7 have exactly one segment difference
        String topMiddle = minus(brokenSeven, brokenOne);
        Assert.assertEquals("1 minus 7 should be 1 segment!", 1, topMiddle.length());
        result.put(topMiddle.charAt(0), 'A');

        // the only one of the 5 segment numbers with the 1 inside is the 3
        String brokenThree = Stream.of(brokenSegments).filter(s -> s.length() == 5).filter(s -> containsAll(s, brokenOne)).findFirst().get();

        // the only one of the 6 segment numbers without the 1 inside is the 6
        String brokenSix = Stream.of(brokenSegments).filter(s -> s.length() == 6).filter(s -> !containsAll(s, brokenOne)).findFirst().get();

        // the only one of the 6 segment numbers without the 4 inside is the 9
        String brokenNine = Stream.of(brokenSegments).filter(s -> s.length() == 6).filter(s -> containsAll(s, brokenFour)).findFirst().get();

        // and the only one left is the 0
        String brokenZero = Stream.of(brokenSegments).filter(s -> s.length() == 6).filter(s -> s != brokenNine && s != brokenSix).findFirst().get();

        // the only segment the 6 has but the 9 doesn't is bottom left
        String bottomLeft = minus(brokenSix, brokenNine);
        Assert.assertEquals("6 minus 9 should be 1 segment!", 1, bottomLeft.length());
        result.put(bottomLeft.charAt(0), 'E');

        // the only segment the 9 has but the 6 doesn't is top right
        String topRight = minus(brokenNine, brokenSix);
        Assert.assertEquals("9 minus 6 should be 1 segment!", 1, topRight.length());
        result.put(topRight.charAt(0), 'C');

        // if you remove 3 from 4, only the bottom center segment is missing
        String bottomCenter = minus(brokenFour, brokenThree);
        Assert.assertEquals("4 minus 3 should be 1 segment!", 1, bottomCenter.length());
        result.put(bottomCenter.charAt(0), 'B');

        // the only one of the 5 segment numbers with the B segment is the 5; and the only one left is the 2
        String brokenFive = Stream.of(brokenSegments).filter(s -> s.length() == 5).filter(s -> s.contains(bottomCenter)).findFirst().get();
        String brokenTwo = Stream.of(brokenSegments).filter(s -> s.length() == 5).filter(s -> s != brokenFive && s != brokenThree).findFirst().get();

        // if you remove 2 from 7, only one segment is missing
        String bottomRight = minus(brokenSeven, brokenTwo);
        Assert.assertEquals("7 minus 2 should be 1 segment!", 1, bottomRight.length());
        result.put(bottomRight.charAt(0), 'F');

        // if you remove 0 from 4, only one segment is missing
        String middle = minus(brokenFour, brokenZero);
        Assert.assertEquals("4 minus 0 should be 1 segment!", 1, middle.length());
        result.put(middle.charAt(0), 'D');

        // ...and now figure out the only one left
        String bottomMiddle = minus(brokenEight, result.keySet().stream().map(c -> c.toString()).collect(Collectors.joining("")));
        Assert.assertEquals("8 minus all known should be 1 segment!", 1, bottomMiddle.length());
        result.put(bottomMiddle.charAt(0), 'G');

        return result;
    }

    static String minus(String biggerString, String toBeSubtracted) {
        return biggerString
                .chars()
                .filter(c -> !toBeSubtracted.chars().anyMatch(d -> c == d))
                .mapToObj(c -> String.valueOf((char) c))
                .collect(Collectors.joining(""));
    }

    static boolean containsAll(String biggerString, String contains) {
        return contains
                .chars()
                .allMatch(c -> biggerString.chars().anyMatch(d -> c == d));
    }

    static Integer createNumber(Map<Character, Character> decoder, String brokenSegment) {
        var repairedSegment = brokenSegment.chars().map(c -> {
                    Character value = decoder.get((char) c);
                    if (value == null) {
                        throw new IllegalArgumentException("Could not find translation for " + (char) c);
                    }
                    return value;
                }).sorted()
                .mapToObj(c -> String.valueOf((char) c)).collect(Collectors.joining(""));

        for(DisplayNumber number : DISPLAY_NUMBERS) {
            if (number.segments.equals(repairedSegment)) {
                return number.number;
            }
        }
        return null;
    }

    private SevenSegmentSearch() {
        // hide this constructor
    }
}
