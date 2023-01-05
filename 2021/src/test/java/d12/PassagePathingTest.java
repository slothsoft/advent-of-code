package d12;

import org.junit.Assert;
import org.junit.Test;

import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.Scanner;
import java.util.Set;

public class PassagePathingTest {

    @Test
    public void testExample1a() throws IOException {
        PassagePathing passagePathing = new PassagePathing(readInput("example1.txt"));
        String[] expectedPaths = readInput("example1-paths.txt");
        Assert.assertArrayEquals(expectedPaths, stringify(passagePathing.createAllPaths()));
    }

    private String[] readInput(String fileName) throws IOException {
        try (InputStream input = getClass().getResourceAsStream(fileName);
             Scanner scanner = new Scanner(input, StandardCharsets.UTF_8)) {
            return scanner.useDelimiter("\\A").next().split("\\R");
        }
    }

    private String[] stringify(Set<String[]> allPaths) {
        String[] result = allPaths.stream().map(s -> String.join(",", s)).sorted().toArray(String[]::new);
        // System.out.println(Arrays.toString(result));
        return result;
    }

    @Test
    public void testExample1b() throws IOException {
        PassagePathing passagePathing = new PassagePathing(readInput("example2.txt"));
        String[] expectedPaths = readInput("example2-paths.txt");
        Assert.assertArrayEquals(expectedPaths, stringify(passagePathing.createAllPaths()));
    }

    @Test
    public void testExample1c() throws IOException {
        PassagePathing passagePathing = new PassagePathing(readInput("example3.txt"));
        Assert.assertEquals(226, passagePathing.calculatePathCombinations());
    }

    @Test
    public void testPuzzle1() throws IOException {
        PassagePathing passagePathing = new PassagePathing(readInput("input.txt"));
        int result = passagePathing.calculatePathCombinations();
        System.out.println("Puzzle 1: " + result);
        Assert.assertEquals(4573, result);
    }

    @Test
    public void testExample2a() throws IOException {
        PassagePathing passagePathing = new PassagePathing(readInput("example1.txt"));
        String[] expectedPaths = readInput("example1-more-paths.txt");
        Assert.assertArrayEquals(expectedPaths, stringify(passagePathing.createAllPaths2()));
    }

    @Test
    public void testExample2b() throws IOException {
        PassagePathing passagePathing = new PassagePathing(readInput("example2.txt"));
        Assert.assertEquals(103, passagePathing.calculatePathCombinations2());
    }

    @Test
    public void testExample2c() throws IOException {
        PassagePathing passagePathing = new PassagePathing(readInput("example3.txt"));
        Assert.assertEquals(3509, passagePathing.calculatePathCombinations2());
    }

    @Test
    public void testPuzzle2() throws IOException {
        PassagePathing passagePathing = new PassagePathing(readInput("input.txt"));
        int result = passagePathing.calculatePathCombinations2();
        System.out.println("Puzzle 2: " + result);
        Assert.assertEquals(117509, result);

    }
}
