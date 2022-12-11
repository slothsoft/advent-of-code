package d03;

import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Scanner;
import java.util.stream.Collectors;
import java.util.stream.Stream;

/**
 * <a href="https://adventofcode.com/2021/day/3">Day 3: Binary Diagnostic</a>:
 * Use the binary numbers in your diagnostic report to calculate the gamma rate
 * and epsilon rate, then multiply them together. What is the power consumption
 * of the submarine? (Be sure to represent your answer in decimal, not binary.)
 */
public final class BinaryDiagnostic {

	public static final int HORIZONTAL = 0;
	public static final int DEPTHS = 1;
	public static final int AIM = 2;

	public static String CalculateSimple(InputStream input, boolean isGamma) {
		return CalculateSimple(ReadInput(input), isGamma);
	}

	public static String[] ReadInput(InputStream input) {
		try (Scanner scanner = new Scanner(input, StandardCharsets.UTF_8)) {
			return scanner.useDelimiter("\\A").next().split("\n");
		}
	}

	public static String CalculateSimple(String[] input, boolean isGamma) {
		char[] result = new char[input[0].trim().length()];

		for (int c = 0; c < result.length; c++) {
			int zero = 0;
			int one = 0;
			
			for (int i = 0; i < input.length; i++) {
				
				switch (input[i].trim().charAt(c)) {
				case '0':
					zero++;
					break;
				case '1':
					one++;
					break;
				default:
					throw new IllegalArgumentException("Unsupported char: " + input[i].trim().charAt(c));
				}
			}
			if (isGamma) {
				result[c] = zero > one ? '0' : '1';
			} else {
				result[c] = zero > one ? '1' : '0';
			}
		}
		return new String(result);
	}

	public static String CalculateComplex(InputStream input, boolean isGamma) {
		return CalculateComplex(ReadInput(input), isGamma);
	}

	public static String CalculateComplex(String[] input, boolean isOxygen) {
		char[] result = new char[input[0].trim().length()];
		List<String> inputAsList = Stream.of(input).map(i -> i.trim()).collect(Collectors.toList());

		for (int c = 0; c < result.length; c++) {
			int zero = 0;
			int one = 0;
			
			for (String string : inputAsList) {
				switch (string.charAt(c)) {
				case '0':
					zero++;
					break;
				case '1':
					one++;
					break;
				default:
					throw new IllegalArgumentException("Unsupported char: " + string.charAt(c));
				}
			}
			if (isOxygen) {
				result[c] = zero > one ? '0' : '1';
			} else {
				result[c] = zero > one ? '1' : '0';
			}
			int finalC = c;
			inputAsList = inputAsList.stream().filter(i -> i.charAt(finalC) == result[finalC]).collect(Collectors.toList());
			
			if (inputAsList.size() == 1) {
				return inputAsList.get(0);
			}
		}
		return new String(result);
	}
	
	private BinaryDiagnostic() {
		// hide this constructor
	}
}
