package d03;

import java.io.InputStream;

import org.junit.Assert;
import org.junit.Test;

public class BinaryDiagnosticTest {

	private static final String[] INPUT = new String[] { "00100", "11110", "10110", "10111", "10101", "01111", "00111",
			"11100", "10000", "11001", "00010", "01010" };

	@Test
	public void testExample1() throws Exception {
		Assert.assertEquals("10110", BinaryDiagnostic.CalculateSimple(INPUT, true));
		Assert.assertEquals("01001", BinaryDiagnostic.CalculateSimple(INPUT, false));
	}

	@Test
	public void testPuzzle1() throws Exception {
		try (InputStream input = getClass().getResourceAsStream("input.txt")) {
			String[] inputArray = BinaryDiagnostic.ReadInput(input);
			String gamma = BinaryDiagnostic.CalculateSimple(inputArray, true);
			String epsilon = BinaryDiagnostic.CalculateSimple(inputArray, false);
			System.out.println("Puzzle 1: " + gamma + " x " + epsilon + " = "
					+ (Integer.parseInt(gamma, 2) * Integer.parseInt(epsilon, 2)));
			Assert.assertEquals("111011110011", gamma);
			Assert.assertEquals("000100001100", epsilon);
		}
	}

	@Test
	public void testExample2() throws Exception {
		Assert.assertEquals("10111", BinaryDiagnostic.CalculateComplex(INPUT, true));
		Assert.assertEquals("01010", BinaryDiagnostic.CalculateComplex(INPUT, false));
	}

	@Test
	public void testPuzzle2() throws Exception {
		try (InputStream input = getClass().getResourceAsStream("input.txt")) {
			String[] inputArray = BinaryDiagnostic.ReadInput(input);
			String oxygen = BinaryDiagnostic.CalculateComplex(inputArray, true);
			String co2 = BinaryDiagnostic.CalculateComplex(inputArray, false);
			System.out.println("Puzzle 2: " + oxygen + " x " + co2 + " = "
					+ (Integer.parseInt(oxygen, 2) * Integer.parseInt(co2, 2)));
			Assert.assertEquals("110000010001", oxygen);
			Assert.assertEquals("000100000001", co2);
		}
	}
}
