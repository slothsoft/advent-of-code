package d17;

import java.util.ArrayList;
import java.util.List;

/**
 * <a href="https://adventofcode.com/2021/day/17">Day 17: Trick Shot</a>: You finally decode the Elves' message. HI,
 * the message says. You continue searching for the sleigh keys.
 *
 * Ahead of you is what appears to be a large ocean trench. Could the keys have fallen into it? You'd better send a
 * probe to investigate.
 */
public final class TrickShot {

    private final int targetStartX;
    private final int targetEndX;
    private final int targetStartY;
    private final int targetEndY;

    private final int minX;
    private final int maxX;
    private final int minY;
    private final int maxY;

    public TrickShot(int startX, int endX, int startY, int endY) {
        this.targetStartX = startX;
        this.targetEndX = endX;
        this.targetStartY = startY;
        this.targetEndY = endY;

        this.minX = 0;
        this.maxX = endX;
        this.minY = startY;
        this.maxY = calculateHighestY();
    }

    public int calculateHighestY() {
        // .............#....#............
        // .......#..............#........
        // ...............................
        // S........................#.....
        // ...............................
        // ...............................
        // ...........................#...
        // ...............................
        // ....................TTTTTTTTTTT
        //
        // The y velocity in the S line = -start velocity
        // The first point have a difference of y = -(startVelocity + 1), then -(startVelocity + 2), -(startVelocity + 3), ...
        // which is the formula n * (n + 1) / 2 in total.
        //
        // To have the highest throw, the next-to-last point has to be on the S line, and the last point has to be in
        // the target rectangle, exactly on startY to be exact. Because then the curve has the highest velocity
        // for the final part, which means it has the highest height before that.
        //
        // This means if startY is the last element of the triangular number
        // ( https://de.wikipedia.org/wiki/Dreieckszahl ), the value before that is (n - 1) * n / 2 which is the
        // maximum height
        // Which means that (n - 1) * n / 2 - startY = n * (n + 1) / 2      | * 2 / n
        //                  n - 1 - 2 * startY / n = n + 1                  | - n + 1
        //                  - 2 * startY / n = 2                            | * n / -2
        //                  startY = - n
        // So we just put that into the formular (n - 1) * n / 2 for the height
        return (-targetStartY - 1) * -targetStartY / 2;
    }

    public boolean isTargetHitByShot(int xVelocity, int yVelocity) {
        int x = 0;
        int y = 0;
        do {
            x += xVelocity;
            y += yVelocity;

            xVelocity = xVelocity > 0 ? xVelocity - 1 : xVelocity < 0 ? xVelocity + 1 : 0;
            yVelocity--;

            if (isPointInRectangle(x, y, targetStartX, targetEndX, targetStartY, targetEndY)) {
                return true;
            }
        } while (isPointInRectangle(x, y, minX, maxX, minY, maxY));
        return false;
    }

    private boolean isPointInRectangle(int x, int y, int minX, int maxX, int minY, int maxY) {
        return x >= minX && x <= maxX && y >= minY && y <= maxY;
    }

    public List<String> calculatePossibleVelocities() {
        List<String> result = new ArrayList<>();

        for (int x = minX; x <= maxX; x++) {
            for (int y = minY; y <= maxY; y++) {
                if (isTargetHitByShot(x, y)) {
                    result.add(x + "," + y);
                }
            }
        }
        return result;
    }
}
