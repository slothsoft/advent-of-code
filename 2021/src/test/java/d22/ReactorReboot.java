package d22;

import org.junit.Assert;

import java.util.*;

/**
 * <a href="https://adventofcode.com/2021/day/22">Day 22: Reactor Reboot</a>
 */
public final class ReactorReboot {

    private static final int X = 0;
    private static final int Y = 1;
    private static final int Z = 2;

    private final String[] inputLines;

    public ReactorReboot(String... inputLines) {
        this.inputLines = inputLines;
    }

    public long reboot(int size) {
        return rebootToSize(-size, size);
    }

    public long rebootToMaxSize() {
        return rebootToSize(Integer.MIN_VALUE, Integer.MAX_VALUE);
    }

    private long rebootToSize(int minValue, int maxValue) {
        BigBox result = new BigBox(minValue, maxValue, false);
        for (String line : inputLines) {
            // on x=-20..26,y=-36..17,z=-47..7
            String[] split = line.split("(on x=|off x=|\\.\\.|,y=|,z=)");

            int[] minCoordinates = new int[]{Integer.parseInt(split[1]), Integer.parseInt(split[3]), Integer.parseInt(split[5])};
            int[] maxCoordinates = new int[]{Integer.parseInt(split[2]), Integer.parseInt(split[4]), Integer.parseInt(split[6])};
            result.addChild(new Box(minCoordinates, maxCoordinates, line.charAt(1) == 'n'));
        }
        return result.countWithValue(true);
    }

    public static class Box {

        protected final int[] minCoordinates;
        protected final int[] maxCoordinates;
        protected boolean value;
        protected long area;

        public Box(int[] minCoordinates, int[] maxCoordinates, boolean value) {
            this.minCoordinates = minCoordinates;
            this.maxCoordinates = maxCoordinates;
            this.value = value;
            this.area = calculateArea();
        }

        private long calculateArea() {
            long result = 1;
            for (int i = 0; i < minCoordinates.length; i++) {
                result *= (maxCoordinates[i] - minCoordinates[i] + 1);
            }
            return result;
        }


        public long countWithValue(boolean value) {
            if (this.value == value) {
                return area;
            }
            return 0;
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

        public Box createIntersection(Box other) {
            return createIntersection(other, other.value);
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

        @Override
        public boolean equals(Object o) {
            if (this == o) return true;
            if (o == null || getClass() != o.getClass()) return false;

            Box box = (Box) o;

            if (!Arrays.equals(minCoordinates, box.minCoordinates)) return false;
            return Arrays.equals(maxCoordinates, box.maxCoordinates);
        }

        @Override
        public int hashCode() {
            int result = Arrays.hashCode(minCoordinates);
            result = 31 * result + Arrays.hashCode(maxCoordinates);
            return result;
        }
    }

    public static class BigBox extends Box {

        private final List<Box> children = new ArrayList<>();

        public BigBox(int minCoordinate, int maxCoordinate, boolean value) {
            super(new int[]{minCoordinate, minCoordinate, minCoordinate}, new int[]{maxCoordinate, maxCoordinate, maxCoordinate}, value);
        }

        public BigBox(int[] minCoordinates, int[] maxCoordinates, boolean value) {
            super(minCoordinates, maxCoordinates, value);
        }

        public long countWithValue(boolean value) {
            long result = 0;
            if (this.value == value) {
                result += area;

                for (Box child : children) {
                    result -= child.countWithValue(!value);
                }
            } else {
                for (Box child : children) {
                    result += child.countWithValue(value);
                }
            }
            return result;
        }

        private void doAddChildren(Collection<Box> children) {
            children.forEach(this::doAddChild);
        }

        public void addChild(Box newChildBig) {
            // we only add the part of the child that intersects this box, or nothing if it's outside
            if (!intersects(newChildBig)) {
                return;
            }
            doAddChild(createIntersection(newChildBig));
        }

        private void doAddChild(Box newChild) {
            // children of the big box should not intersect, else we break them down, so they don't

            Box[] siblings = children.toArray(new Box[0]);
            List<Box> intersectingSiblings = new ArrayList<>();
            for (Box sibling : siblings) {
                if (sibling.intersects(newChild)) {
                    intersectingSiblings.add(sibling);
                }
            }

            // we need to break the child apart, so it won't intersect multiple of it's siblings
            if (intersectingSiblings.size() > 1) {
                Box sibling = intersectingSiblings.get(0);
                Box intersection = sibling.createIntersection(newChild);
                addSplitChild(newChild, intersection);
                return;
            }

            // if we have only one intersection, we need to make sure that the intersection is small enough,
            // else we will also split the child further
            if (intersectingSiblings.size() == 1) {
                Box sibling = intersectingSiblings.get(0);
                Box intersection = sibling.createIntersection(newChild);

                // if the intersection is a sibling, i.e. the new child completely contains its sibling
                if (intersection.equals(sibling)) {
                    // remove the sibling, because it is useless
                    children.remove(sibling);
                    children.add(newChild);
                    return;
                }

                // if the intersection is the entire new child (the sibling completely contains the new child),
                // then we need to split up the sibling, and retry
                if (intersection.equals(newChild)) {
                    // if both have the same value, we don't need to do anything (because why would we?!)
                    if (newChild.value != sibling.value) {
                        children.remove(sibling);
                        addSplitChild(sibling, intersection);
                        addChild(newChild);
                    }
                    return;
                }

                // if neither of above special cases are true, we just split the child further
                addSplitChild(newChild, intersection);
                return;
            }

            // if we are here, then the new child did not intersect with any of its new siblings, which is good and we just add it
            if (newChild.value != value) {
                // if the child has the same value as the parent, we don't need it, because it won't change the result
                children.add(newChild);
            }
        }

        private void addSplitChild(Box newChild, Box intersection) {
            // Visualization https://www.matheretter.de/geoservant/de
            List<Box> split = new ArrayList<>(27);

            // there is space TO THE LEFT of the intersection that needs a smaller child to fill it
            if (newChild.minCoordinates[X] < intersection.minCoordinates[X])
                split.add(new Box(
                        new int[] {newChild.minCoordinates[X], newChild.minCoordinates[Y], newChild.minCoordinates[Z]},
                        new int[] {intersection.minCoordinates[X] - 1, newChild.maxCoordinates[Y], newChild.maxCoordinates[Z]},
                        newChild.value));

            // there is space TO THE RIGHT of the intersection that needs a smaller child to fill it
            if (newChild.maxCoordinates[X] > intersection.maxCoordinates[X])
                split.add(new Box(
                        new int[] {intersection.maxCoordinates[X] + 1, newChild.minCoordinates[Y], newChild.minCoordinates[Z]},
                        new int[] {newChild.maxCoordinates[X], newChild.maxCoordinates[Y], newChild.maxCoordinates[Z]},
                        newChild.value));


            // there is space ON THE TOP of the intersection that needs a smaller child to fill it
            if (newChild.minCoordinates[Y] < intersection.minCoordinates[Y])
                split.add(new Box(
                        new int[] {intersection.minCoordinates[X], newChild.minCoordinates[Y], newChild.minCoordinates[Z]},
                        new int[] {intersection.maxCoordinates[X], intersection.minCoordinates[Y] - 1, newChild.maxCoordinates[Z]},
                        newChild.value));

            // there is space ON THE BOTTOM of the intersection that needs a smaller child to fill it
            if (newChild.maxCoordinates[Y] > intersection.maxCoordinates[Y])
                split.add(new Box(
                        new int[] {intersection.minCoordinates[X], intersection.maxCoordinates[Y] + 1, newChild.minCoordinates[Z]},
                        new int[] {intersection.maxCoordinates[X], newChild.maxCoordinates[Y], newChild.maxCoordinates[Z]},
                        newChild.value));

            // there is space TO THE BACK of the intersection that needs a smaller child to fill it
            if (newChild.minCoordinates[Z] < intersection.minCoordinates[Z])
                split.add(new Box(
                        new int[] {intersection.minCoordinates[X], intersection.minCoordinates[Y], newChild.minCoordinates[Z]},
                        new int[] {intersection.maxCoordinates[X], intersection.maxCoordinates[Y], intersection.minCoordinates[Z] - 1},
                        newChild.value));

            // there is space TO THE FRONT of the intersection that needs a smaller child to fill it
            if (newChild.maxCoordinates[Z] > intersection.maxCoordinates[Z])
                split.add(new Box(
                        new int[] {intersection.minCoordinates[X], intersection.minCoordinates[Y], intersection.maxCoordinates[Z] + 1},
                        new int[] {intersection.maxCoordinates[X], intersection.maxCoordinates[Y], newChild.maxCoordinates[Z]},
                        newChild.value));

            // the intersection itself is needed, too
            split.add(intersection);
            Assert.assertEquals(newChild.calculateArea(), split.stream().mapToLong(Box::calculateArea).sum());
            doAddChildren(split);
        }
    }
}
