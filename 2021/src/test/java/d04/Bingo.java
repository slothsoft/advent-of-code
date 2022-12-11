package d04;

import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;
import java.util.stream.Stream;

/**
 * <a href="https://adventofcode.com/2021/day/4">Day 4: Giant Squid</a>: The
 * submarine has a bingo subsystem to help passengers (currently, you and the
 * giant squid) pass the time. It automatically generates a random order in
 * which to draw numbers and a random set of boards (your puzzle input).
 */
public class Bingo {

	private class Board {

		private static final int SIZE = 5;

		private final int[][] numbers = new int[SIZE][SIZE];
		private final boolean[][] called = new boolean[SIZE][SIZE];

		public Board(String[] lines, int startingIndex) {
			for (int row = 0; row < SIZE; row++) {
				String[] line = lines[startingIndex + row].trim().split("\\s+");
				for (int col = 0; col < line.length; col++) {
					numbers[row][col] = Integer.parseInt(line[col]);
				}
			}
		}

		public void call(int drawnNumber) {
			for (int row = 0; row < SIZE; row++) {
				for (int col = 0; col < SIZE; col++) {
					if (numbers[row][col] == drawnNumber) {
						called[row][col] = true;
					}
				}
			}
		}

		public boolean hasWon() {
			// check if row has won
			for (int row = 0; row < SIZE; row++) {
				boolean allCalled = true;
				for (int col = 0; col < SIZE; col++) {
					if (!called[row][col]) {
						allCalled = false;
						break;
					}
				}
				if (allCalled) {
					return true;
				}
			}
			// check if column has won
			for (int col = 0; col < SIZE; col++) {
				boolean allCalled = true;
				for (int row = 0; row < SIZE; row++) {
					if (!called[row][col]) {
						allCalled = false;
						break;
					}
				}
				if (allCalled) {
					return true;
				}
			}
			return false;
		}

		public int calculateUnmarkedNumbers() {
			int unmarkedNumbers = 0;
			for (int row = 0; row < SIZE; row++) {
				for (int col = 0; col < SIZE; col++) {
					if (!called[row][col]) {
						unmarkedNumbers += numbers[row][col];
					}
				}
			}
			return unmarkedNumbers;
		}

		@Override
		public String toString() {
			StringBuilder sb = new StringBuilder();
			for (int row = 0; row < SIZE; row++) {
				for (int col = 0; col < SIZE; col++) {
					if (called[row][col]) {
						sb.append("(" + (numbers[row][col] < 10 ? "0" + numbers[row][col] : numbers[row][col]) + ")  ");
					} else {
						sb.append(" " + (numbers[row][col] < 10 ? "0" + numbers[row][col] : numbers[row][col]) + "   ");
					}
				}
				sb.append("\n");
			}
			return sb.toString();
		}

	}

	public class Score {
		public final int unmarkedNumbers;
		public final int lastCalledNumber;
		public final int score;
		public final int placement;

		public Score(int unmarkedNumbers, int lastCalledNumber, int placement) {
			this.unmarkedNumbers = unmarkedNumbers;
			this.lastCalledNumber = lastCalledNumber;
			this.placement = placement;
			this.score = unmarkedNumbers * lastCalledNumber;
		}
	}

	private final int[] drawnNumbers;
	private final Board[] boards;

	public Bingo(InputStream input) {
		String[] inputLines = ReadInput(input);
		this.drawnNumbers = Stream.of(inputLines[0].split(",")).mapToInt(s -> Integer.parseInt(s.trim())).toArray();
		this.boards = parseBoards(inputLines, 2);
	}

	private static String[] ReadInput(InputStream input) {
		try (Scanner scanner = new Scanner(input, StandardCharsets.UTF_8)) {
			return scanner.useDelimiter("\\A").next().split("\n");
		}
	}

	public Bingo(int[] drawnNumbers, String[] boardLines) {
		this.drawnNumbers = drawnNumbers;
		this.boards = parseBoards(boardLines, 0);
	}

	private Board[] parseBoards(String[] boardLines, int startIndex) {
		List<Board> result = new ArrayList<Bingo.Board>();
		for (int i = startIndex; i < boardLines.length; i += Board.SIZE + 1) {
			result.add(new Board(boardLines, i));
		}
		return result.toArray(new Board[result.size()]);
	}

	public Score calculateWinningScore() {
		return Stream.of(calculateScores()).filter(s -> s.placement == 0).findFirst().get();
	}

	public Score calculateLosingScore() {
		return Stream.of(calculateScores()).filter(s -> s.placement == boards.length - 1).findFirst().get();
	}

	Score[] calculateScores() {
		Score[] result = new Score[boards.length];
		int placement = 0;
		final int watchedBoard = -1;

		for (int drawnNumber : drawnNumbers) {
			for (int b = 0; b < boards.length; b++) {
				if (b == watchedBoard) {
					System.out.println(boards[b].toString());
					System.out.println("-> " + drawnNumber);
				}

				boards[b].call(drawnNumber);

				if (result[b] == null && boards[b].hasWon()) {
					if (b == watchedBoard) {
						System.out.println(boards[b].toString());
					}
					result[b] = new Score(boards[b].calculateUnmarkedNumbers(), drawnNumber, placement++);
				}
			}
		}
		return result;
	}
}
