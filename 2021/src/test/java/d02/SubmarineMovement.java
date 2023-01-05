package d02;

import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.Scanner;

/**
 * <a href="https://adventofcode.com/2021/day/2">Day 2: Dive</a>: Calculate the
 * horizontal position and depth you would have after following the planned
 * course. What do you get if you multiply your final horizontal position by
 * your final depth?
 */
public final class SubmarineMovement {

	public static final int HORIZONTAL = 0;
	public static final int DEPTHS = 1;
	public static final int AIM = 2;
	
	public static int[] CalculateSimple(InputStream input) {
		return CalculateSimple(ReadInput(input));
	}

	private static String[] ReadInput(InputStream input) {
		try (Scanner scanner = new Scanner(input, StandardCharsets.UTF_8)) {
			return scanner.useDelimiter("\\A").next().split("\n");
		}
	}

	public static int[] CalculateSimple(String[] input) {
		int[] result = new int[2];

		for (int i = 0; i < input.length; i++) {
			String[] split = input[i].trim().split(" ");
			SimpleDirection direction = SimpleDirection.valueOf(split[0].toUpperCase());
			direction.apply(result, Integer.parseInt(split[1]));
		}
		return result;
	}

	public static int[] CalculateComplex(InputStream input) {
		return CalculateComplex(ReadInput(input));
	}

	public static int[] CalculateComplex(String[] input) {
		int[] result = new int[3];

		for (int i = 0; i < input.length; i++) {
			String[] split = input[i].trim().split(" ");
			ComplexDirection direction = ComplexDirection.valueOf(split[0].toUpperCase());
			direction.apply(result, Integer.parseInt(split[1]));
		}
		return result;
	}

	private SubmarineMovement() {
		// hide this constructor
	}
	
	private enum SimpleDirection {
		FORWARD {
			@Override
			protected void apply(int[] coordinates, int value) {
				coordinates[HORIZONTAL] += value;
			}
		},
		DOWN {
			@Override
			protected void apply(int[] coordinates, int value) {
				coordinates[DEPTHS] += value;
			}
		},
		UP {
			@Override
			protected void apply(int[] coordinates, int value) {
				coordinates[DEPTHS] -= value;
			}
		},
		;
		
		protected abstract void apply(int[] coordinates, int value);
	}

	private enum ComplexDirection {
		FORWARD {
			@Override
			protected void apply(int[] coordinates, int value) {
				coordinates[HORIZONTAL] += value;
				coordinates[DEPTHS] += coordinates[AIM] * value;
			}
		},
		DOWN {
			@Override
			protected void apply(int[] coordinates, int value) {
				coordinates[AIM] += value;
			}
		},
		UP {
			@Override
			protected void apply(int[] coordinates, int value) {
				coordinates[AIM] -= value;
			}
		},
		;
		
		protected abstract void apply(int[] coordinates, int value);
	}
}
