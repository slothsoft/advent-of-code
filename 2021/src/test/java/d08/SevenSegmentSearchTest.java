package d08;

import org.junit.Assert;
import org.junit.Test;

import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;
import java.util.Scanner;

public class SevenSegmentSearchTest {
    //     0:(6)   1:(2)   2:(5)   3:(5)   4:(4)
    //    aaaa    ....    aaaa    aaaa    ....
    //   b    c  .    c  .    c  .    c  b    c
    //   b    c  .    c  .    c  .    c  b    c
    //    ....    ....    dddd    dddd    dddd
    //   e    f  .    f  e    .  .    f  .    f
    //   e    f  .    f  e    .  .    f  .    f
    //    gggg    ....    gggg    gggg    ....
    //
    //     5:(5)   6:(6)   7:(3)   8:(7)   9:(6)
    //    aaaa    aaaa    aaaa    aaaa    aaaa
    //   b    .  b    .  .    c  b    c  b    c
    //   b    .  b    .  .    c  b    c  b    c
    //    dddd    dddd    ....    dddd    dddd
    //   .    f  e    f  .    f  e    f  .    f
    //   .    f  e    f  .    f  e    f  .    f
    //    gggg    gggg    ....    gggg    gggg
    @Test
    public void testExample1Decode() {
        final String[] brokenSegments = {
                "be", // 1 (CF)
                "cfbegad", // 8 (ABCDEF)
                "cbdgef", // /w4 -> 9 f=G
                "fgaecd", // /w1 -> 6 b=C
                "cgeb", // 4 (BCDF) /wa -> g=B a=E /w1 c=D
                "fdcge", // /w4 -> 2 e=F
                "agebfd", // /w6&9 -> 0
                "fecdb", // /w1 3 (ACDFG)
                "fabcd", //
                "edb" // 7 (ACF) -> /w1 d=A
                // a) 5er (fdcge-fecdb-fabcd) -> ag=BE
        };
        Map<Character, Character> decodeMap = new HashMap<>();
        decodeMap.put('d', 'A');
        decodeMap.put('g', 'B');
        decodeMap.put('b', 'C');
        decodeMap.put('c', 'D');
        decodeMap.put('a', 'E');
        decodeMap.put('e', 'F');
        decodeMap.put('f', 'G');
        Assert.assertEquals(decodeMap, SevenSegmentSearch.decode(brokenSegments));
    }

    @Test
    public void testExample1Minus() {
        Assert.assertEquals("d", SevenSegmentSearch.minus("edb", "be"));
    }

    @Test
    public void testExample1CreateNumber() {
        Map<Character, Character> decodeMap = new HashMap<>();
        decodeMap.put('d', 'A');
        decodeMap.put('g', 'B');
        decodeMap.put('b', 'C');
        decodeMap.put('c', 'D');
        decodeMap.put('a', 'E');
        decodeMap.put('e', 'F');
        decodeMap.put('f', 'G');

        Assert.assertEquals(0, SevenSegmentSearch.createNumber(decodeMap, "ebagfd").intValue());
        Assert.assertEquals(1, SevenSegmentSearch.createNumber(decodeMap, "be").intValue());
        Assert.assertEquals(2, SevenSegmentSearch.createNumber(decodeMap, "fabcd").intValue());
        Assert.assertEquals(3, SevenSegmentSearch.createNumber(decodeMap, "fecdb").intValue());
        Assert.assertEquals(4, SevenSegmentSearch.createNumber(decodeMap, "cgeb").intValue());
        Assert.assertEquals(5, SevenSegmentSearch.createNumber(decodeMap, "fdcge").intValue());
        Assert.assertEquals(6, SevenSegmentSearch.createNumber(decodeMap, "fgaecd").intValue());
        Assert.assertEquals(7, SevenSegmentSearch.createNumber(decodeMap, "edb").intValue());
        Assert.assertEquals(8, SevenSegmentSearch.createNumber(decodeMap, "gadcfbe").intValue());
        Assert.assertEquals(9, SevenSegmentSearch.createNumber(decodeMap, "efcbdg").intValue());

        Assert.assertNull(SevenSegmentSearch.createNumber(decodeMap, "abcefg"));
    }

    @Test
    public void testExample1ContainsAll() {
        Assert.assertTrue(SevenSegmentSearch.containsAll("edb", "be"));
        Assert.assertFalse(SevenSegmentSearch.containsAll("abc", "def"));
        Assert.assertFalse(SevenSegmentSearch.containsAll("abc", "abd"));
    }

    @Test
    public void testExample1DecodeNumbers() throws IOException {
        int[][] expected = {
                new int[]{8,3,9,4},
                new int[]{9,7,8,1},
                new int[]{1,1,9,7},
                new int[]{9,3,6,1},
                new int[]{4,8,7,3},
                new int[]{8, 4, 1, 8},
                new int[]{4,5,4,8},
                new int[]{1,6,2,5},
                new int[]{8, 7, 1, 7},
                new int[]{4,3,1,5},
        };
        String[] inputLines = readInput("example.txt");
        for (int i = 0; i < inputLines.length; i++) {
            Assert.assertArrayEquals("decodeNumbers() for " + i + " is incorrect:\n\t" + inputLines[i] + "\n\t" + Arrays.toString(expected[i]),
                    expected[i], SevenSegmentSearch.decodeNumbers(inputLines[i]).toArray());
        }
    }

    private String[] readInput(String fileName) throws IOException {
        try (InputStream input = getClass().getResourceAsStream(fileName)) {
            return readInput(input);
        }
    }

    private static String[] readInput(InputStream input) {
        try (Scanner scanner = new Scanner(input, StandardCharsets.UTF_8)) {
            return scanner.useDelimiter("\\A").next().split("\\R");
        }
    }

    @Test
    public void testExample1() throws IOException {
        String[] inputLines = readInput("example.txt");
        Assert.assertEquals(26, SevenSegmentSearch.decodeAndCountNumbers(inputLines, new int[] {
                1, 4, 7, 8
        }));
    }

    @Test
    public void testPuzzle1() throws IOException {
        String[] inputLines = readInput("input.txt");
        long result = SevenSegmentSearch.decodeAndCountNumbers(inputLines, new int[] {
                1, 4, 7, 8
        });
        System.out.println("Puzzle 1: " + result);
        Assert.assertEquals(525, result);
    }

    @Test
    public void testExample2() throws IOException {
        String[] inputLines = readInput("example.txt");
        long result = SevenSegmentSearch.decodeBigNumbers(inputLines);
        Assert.assertEquals(61229, result);
    }

    @Test
    public void testPuzzle2() throws IOException {
        String[] inputLines = readInput("input.txt");
        long result = SevenSegmentSearch.decodeBigNumbers(inputLines);
        System.out.println("Puzzle 2: " + result);
        Assert.assertEquals(1083859, result);
    }
}
