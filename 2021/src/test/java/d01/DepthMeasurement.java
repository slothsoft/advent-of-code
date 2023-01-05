package d01;

import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.Scanner;

/**
 * <a href="https://adventofcode.com/2021/day/1">Day 1: Sonar Sweep</a>: The
 * first order of business is to figure out how quickly the depth increases,
 * just so you know what you're dealing with - you never know if the keys will
 * get carried into deeper water by an ocean current or a fish or something.
 */
public final class DepthMeasurement {

	public static int Calculate(InputStream input, int number) {
		return Calculate(ReadInput(input), number);
	}

	private static String[] ReadInput(InputStream input) {
	    try (Scanner scanner = new Scanner(input, StandardCharsets.UTF_8)) {
	        return scanner.useDelimiter("\\A").next().split("\n");
	    }
	}

	public static int Calculate(String[] input, int number) {
		int lastValue = Integer.MAX_VALUE;
		int result = 0;

		for (int i = 0; i < input.length - number + 1; i++) {
			int currentValue = 0;
			for (int j = 0; j < number; j++) {
				currentValue += Integer.parseInt(input[i + j].trim());
			}
			if (currentValue > lastValue) {
				result++;
			}
			lastValue = currentValue;
		}
		return result;
	}

	private DepthMeasurement() {
		// hide this constructor
	}
}
