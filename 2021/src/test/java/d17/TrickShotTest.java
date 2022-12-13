package d17;

import org.junit.Assert;
import org.junit.Test;

import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.Arrays;
import java.util.List;
import java.util.Scanner;

public class TrickShotTest {

    @Test
    public void testExample1() {
        TrickShot trickShot = new TrickShot(20, 30, -10, -5);
        Assert.assertEquals(45, trickShot.calculateHighestY());
    }

    @Test
    public void testPuzzle1() {
        TrickShot trickShot = new TrickShot(269, 292, -68, -44);
        int result = trickShot.calculateHighestY();
        System.out.println("Puzzle 1: " + result);
        Assert.assertEquals(2278, result);
    }

    @Test
    public void testExample2IsTargetHitByShot() throws IOException {
        TrickShot trickShot = new TrickShot(20, 30, -10, -5);

        for (String line : readInput("example.txt")) {
            String[] lineSplit = line.split(",");
            Assert.assertTrue(
                    "isTargetHitByShot() returns false value for " + line,
                    trickShot.isTargetHitByShot(Integer.parseInt(lineSplit[0]), Integer.parseInt(lineSplit[1])));
        }
    }

    private String[] readInput(String fileName) throws IOException {
        try (InputStream input = getClass().getResourceAsStream(fileName);
             Scanner scanner = new Scanner(input, StandardCharsets.UTF_8)) {
            return scanner.useDelimiter("\\A").next().split("\\R");
        }
    }

    @Test
    public void testExample2() throws IOException {
        TrickShot trickShot = new TrickShot(20, 30, -10, -5);

        List<String> expectedVelocities = Arrays.asList(readInput("example.txt"));
        List<String> actualVelocities = trickShot.calculatePossibleVelocities();

        for (String actualVelocity : actualVelocities) {
            Assert.assertTrue(
                    actualVelocity + " was not an expected velocity, but was found anyway",
                    expectedVelocities.contains(actualVelocity));
        }
        for (String expectedVelocity : expectedVelocities) {
            Assert.assertTrue(
                    expectedVelocity + " was an expected velocity, but was not found",
                    actualVelocities.contains(expectedVelocity));
        }
        Assert.assertEquals(expectedVelocities.size(), actualVelocities.size());
    }


    @Test
    public void testPuzzle2() throws IOException {
        TrickShot trickShot = new TrickShot(269, 292, -68, -44);
        int result = trickShot.calculatePossibleVelocities().size();
        System.out.println("Puzzle 2: " + result);
        Assert.assertEquals(996, result);
    }
}
