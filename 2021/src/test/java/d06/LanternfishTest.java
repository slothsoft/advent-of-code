package d06;

import org.junit.Assert;
import org.junit.Test;

public class LanternfishTest {

    private static final int[] EXAMPLE_INPUT = new int[]{3, 4, 3, 1, 2};
    private static final int[] PUZZLE_INPUT = new int[]{ 3, 5, 2, 5, 4, 3, 2, 2, 3, 5, 2, 3, 2, 2, 2, 2, 3, 5, 3, 5, 5, 2, 2, 3, 4, 2, 3, 5, 5, 3, 3, 5, 2, 4, 5, 4, 3, 5, 3, 2, 5, 4, 1, 1, 1, 5, 1, 4, 1, 4, 3, 5, 2, 3, 2, 2, 2, 5, 2, 1, 2, 2, 2, 2, 3, 4, 5, 2, 5, 4, 1, 3, 1, 5, 5, 5, 3, 5, 3, 1, 5, 4, 2, 5, 3, 3, 5, 5, 5, 3, 2, 2, 1, 1, 3, 2, 1, 2, 2, 4, 3, 4, 1, 3, 4, 1, 2, 2, 4, 1, 3, 1, 4, 3, 3, 1, 2, 3, 1, 3, 4, 1, 1, 2, 5, 1, 2, 1, 2, 4, 1, 3, 2, 1, 1, 2, 4, 3, 5, 1, 3, 2, 1, 3, 2, 3, 4, 5, 5, 4, 1, 3, 4, 1, 2, 3, 5, 2, 3, 5, 2, 1, 1, 5, 5, 4, 4, 4, 5, 3, 3, 2, 5, 4, 4, 1, 5, 1, 5, 5, 5, 2, 2, 1, 2, 4, 5, 1, 2, 1, 4, 5, 4, 2, 4, 3, 2, 5, 2, 2, 1, 4, 3, 5, 4, 2, 1, 1, 5, 1, 4, 5, 1, 2, 5, 5, 1, 4, 1, 1, 4, 5, 2, 5, 3, 1, 4, 5, 2, 1, 3, 1, 3, 3, 5, 5, 1, 4, 1, 3, 2, 2, 3, 5, 4, 3, 2, 5, 1, 1, 1, 2, 2, 5, 3, 4, 2, 1, 3, 2, 5, 3, 2, 2, 3, 5, 2, 1, 4, 5, 4, 4, 5, 5, 3, 3, 5, 4, 5, 5, 4, 3, 5, 3, 5, 3, 1, 3, 2, 2, 1, 4, 4, 5, 2, 2, 4, 2, 1, 4};

    @Test
    public void testExample1() throws Exception {
        Assert.assertEquals(5, Lanternfish.calculateGrowth(EXAMPLE_INPUT, 1));
        Assert.assertEquals(6, Lanternfish.calculateGrowth(EXAMPLE_INPUT, 2));
        Assert.assertEquals(7, Lanternfish.calculateGrowth(EXAMPLE_INPUT, 3));
        Assert.assertEquals(9, Lanternfish.calculateGrowth(EXAMPLE_INPUT, 4));
        Assert.assertEquals(10, Lanternfish.calculateGrowth(EXAMPLE_INPUT, 5));
        Assert.assertEquals(10, Lanternfish.calculateGrowth(EXAMPLE_INPUT, 6));
        Assert.assertEquals(10, Lanternfish.calculateGrowth(EXAMPLE_INPUT, 7));
        Assert.assertEquals(10, Lanternfish.calculateGrowth(EXAMPLE_INPUT, 8));
        Assert.assertEquals(26, Lanternfish.calculateGrowth(EXAMPLE_INPUT, 18));
        Assert.assertEquals(5934, Lanternfish.calculateGrowth(EXAMPLE_INPUT, 80));
    }

    @Test
    public void testPuzzle1() throws Exception {
        long result = Lanternfish.calculateGrowth(PUZZLE_INPUT, 80);
        System.out.println("Puzzle 1: " + result);
        Assert.assertEquals(343441, result);
    }

    @Test
    public void testExample2() throws Exception {
        Assert.assertEquals(26984457539L, Lanternfish.calculateGrowth(EXAMPLE_INPUT, 256));
    }


    @Test
    public void testPuzzle2() throws Exception {
        long result = Lanternfish.calculateGrowth(PUZZLE_INPUT, 256);
        System.out.println("Puzzle 2: " + result);
        Assert.assertEquals(1569108373832L, result);
    }
}
