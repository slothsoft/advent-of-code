package d04;

import java.io.InputStream;

import org.junit.Assert;
import org.junit.Test;

public class BingoTest {

	private static final int[] INPUT_DRAWN_NUMBERS = new int[] { 7, 4, 9, 5, 11, 17, 23, 2, 0, 14, 21, 24, 10, 16, 13,
			6, 15, 25, 12, 22, 18, 20, 8, 19, 3, 26, 1 };
	private static final String[] INPUT_BOARDS = new String[] { //
			"22 13 17 11  0", //
			" 8  2 23  4 24", //
			"21  9 14 16  7", //
			" 6 10  3 18  5", //
			" 1 12 20 15 19", //
			"", //
			"3 15  0  2 22", //
			"9 18 13 17  5", //
			"19  8  7 25 23", //
			"20 11 10 24  4", //
			"14 21 16 12  6", //
			"", //
			"14 21 17 24  4", //
			"10 16 15  9 19", //
			"18  8 23 26 20", //
			"22 11 13  6  5", //
			"2  0 12  3  7" //
	};

	@Test
	public void testExample1() throws Exception {
		Bingo bingo = new Bingo(INPUT_DRAWN_NUMBERS, INPUT_BOARDS);

		Bingo.Score score = bingo.calculateWinningScore();
		Assert.assertEquals(24, score.lastCalledNumber);
		Assert.assertEquals(188, score.unmarkedNumbers);
		Assert.assertEquals(4512, score.score);
	}

	@Test
	public void testPuzzle1() throws Exception {
		try (InputStream input = getClass().getResourceAsStream("input.txt")) {
			Bingo bingo = new Bingo(input);

			Bingo.Score score = bingo.calculateWinningScore();
			System.out.println("Puzzle 1: " + score.score);
			Assert.assertEquals(871, score.unmarkedNumbers);
			Assert.assertEquals(75, score.lastCalledNumber);
			Assert.assertEquals(65325, score.score);
		}
	}

	@Test
	public void testExample2() throws Exception {
		Bingo bingo = new Bingo(INPUT_DRAWN_NUMBERS, INPUT_BOARDS);

		Bingo.Score score = bingo.calculateLosingScore();
		Assert.assertEquals(13, score.lastCalledNumber);
		Assert.assertEquals(148, score.unmarkedNumbers);
		Assert.assertEquals(1924, score.score);
	}

	@Test
	public void testExample2b() throws Exception {
		int[] drawnNumbers = new int[] { 49, 48, 98, 84, 71, 59, 37, 36, 6, 21, 46, 30, 5, 33, 3, 62, 63, 45, 43, 35,
				65, 77, 57, 75, 19, 44, 4, 76, 88, 92, 12, 27, 7, 51, 14, 72, 96, 9, 0, 17, 83, 64, 38, 95, 54, 20, 1,
				74, 69, 80, 81, 56, 10, 68, 42, 15, 99, 53, 93, 94, 47, 13, 29, 34, 60, 41, 82, 90, 25, 85, 78, 91, 32,
				70, 58, 28, 61, 24, 55, 87, 39, 11, 79, 50, 22, 8, 89, 26, 16, 2, 73, 23, 18, 66, 52, 31, 86, 97, 67,
				40 };
		String[] inputBoards = new String[] { //
				"86 46 47 61 57", // 46 (3) 57 (8) 47 (31!)
				"44 74 17  5 87", // 5 (5) 44 (10) 17 (16) 74 (23)
				"78  8 54 55 97", // 54 (22)
				"11 90  7 75 70", // 75 (9) 7 (13)
				"81 50 84 10 60", // 84 (1) 81 (24) 10 (26)
				"", //
				"47 28 64 52 44", // 44 (10) 64 (19) 47 (31)
				"73 48 30 15 53", // 48 (0) 30 (4) 15 (27) 53 (28)
				"57 21 78 75 26", // 21 (2) 57 (8) 75 (9)
				"51 39 72 18 25", // 51 (14) 72 (15)
				"29 76 83 54 82", // 76 (11) 83 (18) 29 (32) 54 (22) 82 (33!)
				"", //
				"81  1 18 24 12", // 12 (12) 1 (22) 81 (24)
				" 3 38 15 85 50", // 3 (6) 38 (20) 15 (27)
				"32 10 74 86 84", // 84 (1) 74 (23) 10 (26)
				"30 64 56 79 95", // 30 (4) 64 (19) 95 (21) 56 (25)
				"78 94 35 93  8" // 35 (7) 93 (29) 94 (30!)
		};

		Bingo bingo = new Bingo(drawnNumbers, inputBoards);

		// try general scores

		Bingo.Score[] scores = bingo.calculateScores();

		Bingo.Score score = scores[0];
		Assert.assertEquals(47, score.lastCalledNumber);
		Assert.assertEquals(753, score.unmarkedNumbers);
		Assert.assertEquals(47 * 753, score.score);
		Assert.assertEquals(1, score.placement);

		score = scores[1];
		Assert.assertEquals(82, score.lastCalledNumber);
		Assert.assertEquals(339, score.unmarkedNumbers);
		Assert.assertEquals(82 * 339, score.score);
		Assert.assertEquals(2, score.placement);

		score = scores[2];
		Assert.assertEquals(94, score.lastCalledNumber);
		Assert.assertEquals(460, score.unmarkedNumbers);
		Assert.assertEquals(94 * 460, score.score);
		Assert.assertEquals(0, score.placement);

		// now check the losing score

		bingo = new Bingo(drawnNumbers, inputBoards);
		score = bingo.calculateLosingScore();
		Assert.assertEquals(82, score.lastCalledNumber);
		Assert.assertEquals(339, score.unmarkedNumbers);
		Assert.assertEquals(82 * 339, score.score);
		Assert.assertEquals(2, score.placement);
	}

	@Test
	public void testExample2c() throws Exception {
		try (InputStream input = getClass().getResourceAsStream("input-c.txt")) {
			Bingo bingo = new Bingo(input); // same example as testExample2c

			// try general scores

			Bingo.Score[] scores = bingo.calculateScores();

			Bingo.Score score = scores[0];
			Assert.assertEquals(47, score.lastCalledNumber);
			Assert.assertEquals(753, score.unmarkedNumbers);
			Assert.assertEquals(47 * 753, score.score);
			Assert.assertEquals(1, score.placement);

			score = scores[1];
			Assert.assertEquals(82, score.lastCalledNumber);
			Assert.assertEquals(339, score.unmarkedNumbers);
			Assert.assertEquals(82 * 339, score.score);
			Assert.assertEquals(2, score.placement);

			score = scores[2];
			Assert.assertEquals(94, score.lastCalledNumber);
			Assert.assertEquals(460, score.unmarkedNumbers);
			Assert.assertEquals(94 * 460, score.score);
			Assert.assertEquals(0, score.placement);
		}

		// now check the losing score

		try (InputStream input = getClass().getResourceAsStream("input-c.txt")) {
			Bingo bingo = new Bingo(input);
			Bingo.Score score = bingo.calculateLosingScore();
			Assert.assertEquals(82, score.lastCalledNumber);
			Assert.assertEquals(339, score.unmarkedNumbers);
			Assert.assertEquals(82 * 339, score.score);
			Assert.assertEquals(2, score.placement);
		}
	}

	@Test
	public void testPuzzle2() throws Exception {
		try (InputStream input = getClass().getResourceAsStream("input.txt")) {
			Bingo bingo = new Bingo(input);

			Bingo.Score score = bingo.calculateLosingScore();
			System.out.println("Puzzle 2: " + score.score);
			Assert.assertEquals(16, score.lastCalledNumber);
			Assert.assertEquals(289, score.unmarkedNumbers);
			Assert.assertEquals(4624, score.score);
		}
	}
}
