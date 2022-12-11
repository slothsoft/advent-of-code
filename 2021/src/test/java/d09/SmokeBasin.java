package d09;

import java.awt.*;
import java.util.*;
import java.util.List;
import java.util.stream.IntStream;

/**
 * <a href="https://adventofcode.com/2021/day/9">Day 9: Smoke Basin</a>: These caves seem to be lava tubes. Parts are
 * even still volcanically active; small hydrothermal vents release smoke into the caves that slowly settles like rain.
 *
 * If you can model how the smoke flows through the caves, you might be able to avoid it and be that much safer. The
 * submarine generates a heightmap of the floor of the nearby caves for you (your puzzle input).
 */
public final class SmokeBasin {

    private final int[][] map;

    public SmokeBasin(String[] lines) {
        map = new int[lines.length][lines[0].length()];
        for (int i = 0; i < lines.length; i++) {
            map[i] = lines[i].chars().map(c -> Integer.parseInt("" + (char) c)).toArray();
        }
    }

    public int calculateLowPointsRisk() {
        int result = 0;
        for (Point lowPoint : findLowPoints()) {
            result += map[lowPoint.y][lowPoint.x] + 1;
        }
        return result;
    }

    Point[] findLowPoints() {
        List<Point> result = new ArrayList<>();

        for (int row = 0; row < map.length; row++) {
            for (int col = 0; col < map[row].length; col++) {
                if (isLowerThan(map[row][col], row - 1, col) && isLowerThan(map[row][col], row + 1, col) &&
                        isLowerThan(map[row][col], row, col - 1) && isLowerThan(map[row][col], row, col + 1)) {
                    result.add(new Point(col, row));
                }
            }
        }
        return result.toArray(new Point[result.size()]);
    }

    private boolean isLowerThan(int value, int row, int col) {
        if (row < 0 || col < 0) {
            return true;
        }
        if (row >= map.length || col >= map[row].length) {
            return true;
        }
        return value < map[row][col];
    }

    public int calculateTopBasinSize(int top) {
        return IntStream.of(findBasinSizes())
                .map(i -> -i).sorted().map(i -> -i)
                .limit(top)
                .reduce(1, (x, y) -> x * y); // create the product of the top basin sizes
    }

    public int[] findBasinSizes() {
        Point[] lowPoints = findLowPoints();
        int[] basinSizes = new int[lowPoints.length];

        for (int i = 0; i < lowPoints.length; i++) {
            Point lowPoint = lowPoints[i];
            basinSizes[i] = calculateBasinSize(lowPoint.y, lowPoint.x, new ArrayList<>());
        }
        return basinSizes;
    }

    private int calculateBasinSize(int row, int col, List<Point> alreadyChecked) {
        Point point = new Point(col, row);
        if (alreadyChecked.contains(point)) {
            return 0;
        }
        if (row < 0 || col < 0) {
            return 0;
        }
        if (row >= map.length || col >= map[row].length) {
            return 0;
        }
        if (map[row][col] == 9) {
            return 0;
        }
        alreadyChecked.add(point);
        return 1
                + calculateBasinSize(row - 1, col, alreadyChecked)
                + calculateBasinSize(row + 1, col, alreadyChecked)
                + calculateBasinSize(row, col - 1, alreadyChecked)
                + calculateBasinSize(row, col + 1, alreadyChecked);
    }

}
