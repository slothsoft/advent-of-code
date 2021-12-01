package d01;

import java.io.InputStream;

import org.junit.Assert;
import org.junit.Test;

public class DepthMeasurementTest {

	private static final String[] INPUT = new String[] { "199", "200", "208", "210", "200", "207", "240", "269", "260", "263" };

	@Test
	public void testExample1() throws Exception {
		Assert.assertEquals(7, DepthMeasurement.Calculate(INPUT, 1));
	}

	@Test
	public void testPuzzle1() throws Exception {
		try (InputStream input = getClass().getResourceAsStream("input.txt")) {
			int answer = DepthMeasurement.Calculate(input, 1);
			System.out.println("Puzzle 1: " + answer);
			Assert.assertEquals(1393, answer);
		}
	}

	@Test
	public void testExample2() throws Exception {
		Assert.assertEquals(5, DepthMeasurement.Calculate(INPUT, 3));
	}

	@Test
	public void testPuzzle2() throws Exception {
		try (InputStream input = getClass().getResourceAsStream("input.txt")) {
			int answer = DepthMeasurement.Calculate(input, 3);
			System.out.println("Puzzle 2: " + answer);
			Assert.assertEquals(1359, answer);
		}
	}
}
