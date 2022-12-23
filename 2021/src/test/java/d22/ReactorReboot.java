package d22;

import org.junit.Assert;

import java.util.*;
import java.util.function.Consumer;

/**
 * <a href="https://adventofcode.com/2021/day/22">Day 22: Reactor Reboot</a>
 */
public final class ReactorReboot {

    public static class Cube {

        private final int minX;
        private final int maxX;
        private final int minY;
        private final int maxY;
        private final int minZ;
        private final int maxZ;
        private final boolean[][][] elements;

        public Cube(int minX, int maxX, int minY, int maxY, int minZ, int maxZ) {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
            this.minZ = minZ;
            this.maxZ = maxZ;

            elements = new boolean[maxX - minX + 1][][];
            for (int x = 0; x < elements.length; x++) {
                elements[x] = new boolean[maxY - minY + 1][];
                for (int y = 0; y < elements[x].length; y++) {
                    elements[x][y] = new boolean[maxZ - minZ + 1];
                }
            }
        }

        public void setValues(int x1, int x2, int y1, int y2, int z1, int z2, boolean value) {
            // don't do anything if values are completely outside the cube
            if ((x1 < minX && x2 < minX) || (x1 > maxX && x2 > maxX))
                return;
            if ((y1 < minY && y2 < minY) || (y1 > maxY && y2 > maxY))
                return;
            if ((z1 < minZ && z2 < minZ) || (z1 > maxZ && z2 > maxZ))
                return;

            // make sure the coordinates are inside the cube
            var sanitizeX1 = sanitize(x1, minX, maxX);
            var sanitizeX2 = sanitize(x2, minX, maxX);
            var sanitizeY1 = sanitize(y1, minY, maxY);
            var sanitizeY2 = sanitize(y2, minY, maxY);
            var sanitizeZ1 = sanitize(z1, minZ, maxZ);
            var sanitizeZ2 = sanitize(z2, minZ, maxZ);

            // now set the correct elements to the value
            for (int x = sanitizeX1; x <= sanitizeX2; x++) {
                for (int y = sanitizeY1; y <= sanitizeY2; y++) {
                    for (int z = sanitizeZ1; z <= sanitizeZ2; z++) {
                        elements[x - minX][y - minY][z - minZ] = value;
                    }
                }
            }
        }

        private int sanitize(int value, int min, int max) {
            return Math.min(max, Math.max(min, value));
        }

        public long countCubes(boolean value) {
            long result = 0;
            for (int x = 0; x < elements.length; x++) {
                for (int y = 0; y < elements[x].length; y++) {
                    for (int z = 0; z < elements[x][y].length; z++) {
                        if (elements[x][y][z] == value) {
                            result++;
                        }
                    }
                }
            }
            return result;
        }
    }

    private final String[] inputLines;

    public ReactorReboot(String... inputLines) {
        this.inputLines = inputLines;
    }

    // NOTE: this method does not work for the big files
    public Cube rebootToMaxSize() {
        int minX = Integer.MAX_VALUE;
        int minY = Integer.MAX_VALUE;
        int minZ = Integer.MAX_VALUE;
        int maxX = 0;
        int maxY = 0;
        int maxZ = 0;

        for (String line : inputLines) {
            // on x=-20..26,y=-36..17,z=-47..7
            String[] split = line.split("(on x=|off x=|\\.\\.|,y=|,z=)");

            minX = Math.min(minX, Math.min(Integer.parseInt(split[1]), Integer.parseInt(split[2])));
            minY = Math.min(minY, Math.min(Integer.parseInt(split[3]), Integer.parseInt(split[4])));
            minZ = Math.min(minZ, Math.min(Integer.parseInt(split[5]), Integer.parseInt(split[6])));

            maxX = Math.max(maxX, Math.max(Integer.parseInt(split[1]), Integer.parseInt(split[2])));
            maxY = Math.max(maxY, Math.max(Integer.parseInt(split[3]), Integer.parseInt(split[4])));
            maxZ = Math.max(maxZ, Math.max(Integer.parseInt(split[5]), Integer.parseInt(split[6])));
        }

        return reboot(minX, maxX, minY, maxY, minZ, maxZ);
    }

    public Cube reboot(int size) {
        return reboot(-size, size, -size, size, -size, size);
    }

    private Cube reboot(int minX, int maxX, int minY, int maxY, int minZ, int maxZ) {
        Cube result = new Cube(minX, maxX, minY, maxY, minZ, maxZ);
        for (String line : inputLines) {
            // on x=-20..26,y=-36..17,z=-47..7
            String[] split = line.split("(on x=|off x=|\\.\\.|,y=|,z=)");
            result.setValues(
                    Integer.parseInt(split[1]), Integer.parseInt(split[2]),
                    Integer.parseInt(split[3]), Integer.parseInt(split[4]),
                    Integer.parseInt(split[5]), Integer.parseInt(split[6]),
                    line.charAt(1) == 'n');
        }
        return result;
    }

    public long countRebootCubes(int minValue, int maxValue) {
        Box result = new Box(minValue, maxValue, false);
        for (String line : inputLines) {
            // on x=-20..26,y=-36..17,z=-47..7
            String[] split = line.split("(on x=|off x=|\\.\\.|,y=|,z=)");

            int[] minCoordinates = new int[]{Integer.parseInt(split[1]), Integer.parseInt(split[3]), Integer.parseInt(split[5])};
            int[] maxCoordinates = new int[]{Integer.parseInt(split[2]), Integer.parseInt(split[4]), Integer.parseInt(split[6])};
            Box possibleChild = new Box(minCoordinates, maxCoordinates, line.charAt(1) == 'n');

            result.traverse(box -> box.createIntersectionChildren(possibleChild));
        }
        return result.countWithValue(true);
    }


    public static class Box {

        private final int[] minCoordinates;
        private final int[] maxCoordinates;
        private final boolean value;
        public List<Box> children = new ArrayList<>();

        public Box(int minCoordinate, int maxCoordinate, boolean value) {
            this(new int[]{minCoordinate, minCoordinate, minCoordinate}, new int[]{maxCoordinate, maxCoordinate, maxCoordinate}, value);
        }

        public Box(int[] minCoordinates, int[] maxCoordinates, boolean value) {
            this.minCoordinates = minCoordinates;
            this.maxCoordinates = maxCoordinates;
            this.value = value;
        }

        public long calculateArea() {
            long result = 1;
            for (int i = 0; i < minCoordinates.length; i++) {
                result *= (maxCoordinates[i] - minCoordinates[i] + 1);
            }
            return result;
        }

        public void traverse(Consumer<Box> consumer) {
            Box[] childrenArray = children.toArray(new Box[0]);
            consumer.accept(this);
            for (Box child : childrenArray) {
                child.traverse(consumer);
            }
        }

        public long countWithValue(boolean value) {
            if (this.value == value) {
                return calculateArea() - calculateChildrenWithValue(!value);
            }
            return calculateChildrenWithValue(value);
        }

        private long calculateChildrenWithValue(boolean value) {
            long result = 0;
            Box[] childrenArray = children.toArray(new Box[0]);

            for (Box child : childrenArray) {
                result += child.countWithValue(value);
            }

            // FIXME: this works so far, but the current problem is that two intersecting children have their intersection
            // counted twice. So for the A example, the 8 cubes between the first and second child are counted double
            for (int i = 0; i < childrenArray.length; i++) {
                Box child1 = childrenArray[i];

                for (int j = i + 1; j < childrenArray.length; j++) {
                    Box child2 = childrenArray[j];

                    // the problem only persists for children with the same value
                    if (child1.value != child2.value) continue;

                    // and if they don't intersect, we don't have a problem
                    if (!child1.intersects(child2)) continue;

                    // so now we have an intersection, but this intersection might again be changed by the
                    // parents' children (they have the same children for the intersection part)
                    Box intersection = child1.createIntersection(child2, child2.value);
                    intersection.createIntersectionChildren(child2.children);
                    long intersectionValue = intersection.countWithValue(child2.value);
                    Assert.assertTrue("Intersection value should not be bigger than result " + intersectionValue +" < " + result, intersectionValue <= result);
                    result -= intersectionValue;
                }
            }

            return result;
        }

        public void createIntersectionChildren(List<Box> possibleChildren) {
            for (Box possibleChild : possibleChildren) {
                createIntersectionChildren(possibleChild);
            }
        }

        public void createIntersectionChildren(Box possibleChild) {
            // if the boxes do not intersect we can ignore it (the box WILL intersect the result box anyway)
            if (!intersects(possibleChild))
                return;

            // if the box has the same value, we do not need to check this box
            if (value == possibleChild.value)
                return;

            Box intersection = createIntersection(possibleChild, possibleChild.value);
            children.add(intersection);
        }

        public boolean intersects(Box other) {
            for (int i = 0; i < minCoordinates.length; i++) {
                if (other.minCoordinates[i] > this.maxCoordinates[i]) {
                    return false;
                }
                if (this.minCoordinates[i] > other.maxCoordinates[i]) {
                    return false;
                }
            }
            return true;
        }

        public Box createIntersection(Box other, boolean newValue) {
            int[] newMinCoordinates = new int[3];
            int[] newMaxCoordinates = new int[3];

            for (int i = 0; i < newMinCoordinates.length; i++) {
                newMinCoordinates[i] = Math.max(other.minCoordinates[i], this.minCoordinates[i]);
                newMaxCoordinates[i] = Math.min(other.maxCoordinates[i], this.maxCoordinates[i]);
            }
            return new Box(newMinCoordinates, newMaxCoordinates, newValue);
        }
    }
}
