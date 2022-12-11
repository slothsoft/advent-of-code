package d09;

import org.junit.Assert;
import org.junit.Test;

import java.awt.*;
import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.Scanner;

public class SmokeBasinTest {

    @Test
    public void testExample1() throws IOException {
        String[] inputLines = readInput("example.txt");

        SmokeBasin smokeBasin = new SmokeBasin(inputLines);
        Point[] lowPoints = smokeBasin.findLowPoints();

        Assert.assertNotNull(lowPoints);
        Assert.assertEquals(4, lowPoints.length);
        Assert.assertEquals(1, lowPoints[0].x);
        Assert.assertEquals(0, lowPoints[0].y);
        Assert.assertEquals(9, lowPoints[1].x);
        Assert.assertEquals(0, lowPoints[1].y);
        Assert.assertEquals(2, lowPoints[2].x);
        Assert.assertEquals(2, lowPoints[2].y);
        Assert.assertEquals(6, lowPoints[3].x);
        Assert.assertEquals(4, lowPoints[3].y);

        Assert.assertEquals(15, smokeBasin.calculateLowPointsRisk());
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

        SmokeBasin smokeBasin = new SmokeBasin(inputLines);
        int result = smokeBasin.calculateLowPointsRisk();
        System.out.println("Puzzle 1: " + result);
        Assert.assertEquals(572, result);
    }

    @Test
    public void testExample2() throws IOException {
        String[] inputLines = readInput("example.txt");

        SmokeBasin smokeBasin = new SmokeBasin(inputLines);
        int[] basinSizes = smokeBasin.findBasinSizes();

        Assert.assertNotNull(basinSizes);
        Assert.assertEquals(4, basinSizes.length);
        Assert.assertEquals(3, basinSizes[0]);
        Assert.assertEquals(9, basinSizes[1]);
        Assert.assertEquals(14, basinSizes[2]);
        Assert.assertEquals(9, basinSizes[3]);

        Assert.assertEquals(1134, smokeBasin.calculateTopBasinSize(3));
    }

    @Test
    public void testPuzzle2() throws IOException {
        String[] inputLines = readInput("input.txt");

        SmokeBasin smokeBasin = new SmokeBasin(inputLines);
        int result = smokeBasin.calculateTopBasinSize(3);
        System.out.println("Puzzle 2: " + result);
        Assert.assertEquals(847044, result);
    }
}
