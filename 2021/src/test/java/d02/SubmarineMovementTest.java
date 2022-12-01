package d02;

import java.io.InputStream;

import org.junit.Assert;
import org.junit.Test;

public class SubmarineMovementTest {

	private static final String[] INPUT = new String[] { "forward 5", "down 5", "forward 8", "up 3", "down 8",
			"forward 2" };

	@Test
	public void testExample1() throws Exception {
		Assert.assertArrayEquals(new int[] { 15, 10 }, SubmarineMovement.CalculateSimple(INPUT));
	}

	@Test
	public void testPuzzle1() throws Exception {
		try (InputStream input = getClass().getResourceAsStream("input.txt")) {
			int[] answer = SubmarineMovement.CalculateSimple(input);
			System.out.println("Puzzle 1: " + answer[SubmarineMovement.HORIZONTAL] + " x " + answer[SubmarineMovement.DEPTHS]
							+ " = " + (answer[SubmarineMovement.HORIZONTAL] * answer[SubmarineMovement.DEPTHS]));
			Assert.assertArrayEquals(new int[] { 1993, 998 }, answer);
		}
	}

	@Test
	public void testExample2() throws Exception {
		Assert.assertArrayEquals(new int[] { 15, 60, 10 }, SubmarineMovement.CalculateComplex(INPUT));
	}

	@Test
	public void testPuzzle2() throws Exception {
		try (InputStream input = getClass().getResourceAsStream("input.txt")) {
			int[] answer = SubmarineMovement.CalculateComplex(input);
			System.out.println("Puzzle 2: " + answer[SubmarineMovement.HORIZONTAL] + " x " + answer[SubmarineMovement.DEPTHS] + " x " + answer[SubmarineMovement.AIM]
							+ " = " + (answer[SubmarineMovement.HORIZONTAL] * answer[SubmarineMovement.DEPTHS]));
			Assert.assertArrayEquals(new int[] { 1993, 1006983, 998 }, answer);
		}
	}
}
