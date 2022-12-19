package d19;

import java.util.*;
import java.util.List;
import java.util.function.Function;
import java.util.stream.Collectors;

/**
 * <a href="https://adventofcode.com/2021/day/19">Day 19: Beacon Scanner</a>
 */
public final class BeaconScanner {
    static class Vector {
        final int x;
        final int y;
        final int z;

        public Vector(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public boolean AreCoordinatesEqual(Vector that) {
            return this.x == that.x && this.y == that.y && this.z == that.z;
        }

        @Override
        public boolean equals(Object o) {
            if (this == o) return true;
            if (o == null || getClass() != o.getClass()) return false;

            Vector that = (Vector) o;
            if (x != that.x) return false;
            if (y != that.y) return false;
            return z == that.z;
        }

        @Override
        public int hashCode() {
            return Objects.hash(x, y, z);
        }

        @Override
        public String toString() {
            return "Vector{" + "x=" + x + ", y=" + y + ", z=" + z + '}';
        }

        public Vector add(Vector other) {
            return new Vector(this.x + other.x, this.y + other.y, this.z + other.z);
        }

        public Vector negate() {
            return new Vector(-this.x, -this.y, -this.z);
        }
    }

    static class VectorBetweenVectors extends Vector {
        final Vector fromVector;
        final Vector toVector;

        public VectorBetweenVectors(Vector fromVector, Vector toVector) {
            super(fromVector.x - toVector.x, fromVector.y - toVector.y, fromVector.z - toVector.z);
            this.fromVector = fromVector;
            this.toVector = toVector;
        }
    }

    static class Scanner {
        final int number;
        final List<Vector> points;
        // these vectors should have identical absolute values no matter the rotation of the scanner
        final Set<VectorBetweenVectors> vectors;

        public Scanner(int number, List<Vector> points) {
            this.number = number;
            this.points = points;

            this.vectors = new HashSet<>(points.size() * (points.size() - 1));
            for (Vector point1 : points) {
                for (Vector point2 : points) {
                    if (point1 != point2) {
                        vectors.add(new VectorBetweenVectors(point1, point2));
                    }
                }
            }
        }
    }

    interface Transformation {
        Vector transform(Vector vector);
    }

    enum Rotation implements Transformation {
        TO_X_Y_Z(v -> new Vector(v.x, v.y, v.z)),
        TO_X_MY_MZ(v -> new Vector(v.x, -v.y, -v.z)),
        TO_X_Z_MY(v -> new Vector(v.x, v.z, -v.y)),
        TO_X_MZ_Y(v -> new Vector(v.x, -v.z, v.y)),

        TO_MX_Y_MZ(v -> new Vector(-v.x, v.y, -v.z)),
        TO_MX_MY_Z(v -> new Vector(-v.x, -v.y, v.z)),
        TO_MX_Z_Y(v -> new Vector(-v.x, v.z, v.y)),
        TO_MX_MZ_MY(v -> new Vector(-v.x, -v.z, -v.y)),

        TO_Y_X_MZ(v -> new Vector(v.y, v.x, -v.z)),
        TO_Y_MX_Z(v -> new Vector(v.y, -v.x, v.z)),
        TO_Y_Z_X(v -> new Vector(v.y, v.z, v.x)),
        TO_Y_MZ_MX(v -> new Vector(v.y, -v.z, -v.x)),

        TO_MY_X_Z(v -> new Vector(-v.y, v.x, v.z)),
        TO_MY_MX_MZ(v -> new Vector(-v.y, -v.x, -v.z)),
        TO_MY_Z_MX(v -> new Vector(-v.y, v.z, -v.x)),
        TO_MY_MZ_X(v -> new Vector(-v.y, -v.z, v.x)),

        TO_Z_X_Y(v -> new Vector(v.z, v.x, v.y)),
        TO_Z_MX_MY(v -> new Vector(v.z, -v.x, -v.y)),
        TO_Z_Y_MX(v -> new Vector(v.z, v.y, -v.x)),
        TO_Z_MY_X(v -> new Vector(v.z, -v.y, v.x)),

        TO_MZ_X_MY(v -> new Vector(-v.z, v.x, -v.y)),
        TO_MZ_MX_Y(v -> new Vector(-v.z, -v.x, v.y)),
        TO_MZ_Y_X(v -> new Vector(-v.z, v.y, v.x)),
        TO_MZ_MY_MX(v -> new Vector(-v.z, -v.y, -v.x)),
        ;

        public static final Rotation[] ALL_VALUES = Rotation.values();

        private final Function<Vector, Vector> delegate;
        private final String displayName;

        Rotation(Function<Vector, Vector> delegate) {
            this.delegate = delegate;
            Vector vector = new Vector(1, 2, 3);
            this.displayName = "Rotate " + vector + " -> " + delegate.apply(vector);
        }

        public Vector transform(Vector vector) {
            return delegate.apply(vector);
        }

        @Override
        public String toString() {
            return displayName;
        }
    }

    static class Alignment {
        final Transformation rotation;
        final Vector movement;

        public Alignment(Transformation rotation, Vector movement) {
            this.rotation = rotation;
            this.movement = movement;
        }

        public Vector applyMovement(Vector vector) {
            return vector.add(movement);
        }
    }


    final List<Scanner> scanners;

    public BeaconScanner(String[] inputLines) {
        this.scanners = new ArrayList<>();

        int index = 0;
        while (index < inputLines.length) {

            // line: --- scanner 0 ---
            int number = Integer.parseInt(inputLines[index].split(" ")[2]);
            index++;

            List<Vector> points = new ArrayList<>();
            while (index < inputLines.length) {
                if (inputLines[index].isBlank()) {
                    break;
                }
                // line: 404,-588,-901
                String[] vectorAsStrings = inputLines[index].split(",");
                points.add(new Vector(Integer.parseInt(vectorAsStrings[0]), Integer.parseInt(vectorAsStrings[1]), Integer.parseInt(vectorAsStrings[2])));
                index++;
            }
            scanners.add(new Scanner(number, points));
            index++;
        }
    }

    public Map<Scanner, Alignment> alignScanners() {
        Map<Scanner, Alignment> result = new HashMap<>();
        result.put(scanners.get(0), new Alignment(Rotation.TO_X_Y_Z, new Vector(0, 0, 0)));

        List<Scanner> scannersToCheck = new ArrayList<>(scanners);
        while (scannersToCheck.size() > 0) {
            Scanner scanner = scannersToCheck.get(0);
            scannersToCheck.remove(scanner);

            if (result.containsKey(scanner)) {
                continue;
            }
            Alignment alignment = alignScanner(result, scanner);
            if (alignment != null) {
                result.put(scanner, alignment);
            } else {
                // add this scanner to the list to recheck again later
                scannersToCheck.add(scanner);
            }
        }
        return result;
    }

    private Alignment alignScanner(Map<Scanner, Alignment> existingAlignments, Scanner scanner) {
        for (Rotation possibleRotation : Rotation.ALL_VALUES) {
            Map<Vector, VectorBetweenVectors> rotatedVectors = scanner.vectors.stream().collect(Collectors.toMap(possibleRotation::transform, v -> v));

            for (Scanner alignedScanner : existingAlignments.keySet()) {
                Set<Vector> alignedVectors = rotatedVectors.keySet().stream().filter(v ->
                        alignedScanner.vectors.stream().anyMatch(a -> a.AreCoordinatesEqual(v))).collect(Collectors.toSet());

                if (alignedVectors.size() >= 12 * 11) {
                    // find two points so we can calculate the difference
                    Map.Entry<VectorBetweenVectors, List<Vector>> vector = alignedVectors.stream()
                            .collect(Collectors.groupingBy(v -> rotatedVectors.get(v)))
                            .entrySet().stream()
                            .findFirst().get();

                    // find the same point in the aligned scanner
                    Map.Entry<VectorBetweenVectors, List<VectorBetweenVectors>> alignedVector = alignedScanner.vectors.stream()
                            .collect(Collectors.groupingBy(v -> v))
                            .entrySet().stream()
                            .filter(v -> areListsEqual(v.getValue(), vector.getValue()))
                            .findFirst().get();

                    return CalculateAlignment(vector.getKey().fromVector, possibleRotation, alignedVector.getKey().fromVector, existingAlignments.get(alignedScanner));
                }
            }
        }
        return null;
    }

    private Alignment CalculateAlignment(Vector vectorToBeAligned, Rotation alignedRotation, Vector alignedVector, Alignment alignment) {
        // 1 must be at 68,-1246,-43 (-> scanner 0)
        // - vectorToBeAligned = -322, 571, 750
        // - alignedRotation = -x, y, -z
        // - alignedVector = 390, -675, -793
        // - alignment.rotation = x, y, z
        // - alignment.movement = 0, 0, 0

        // 2 must be at 1105,-1205,1229

        // 3 must be at -92,-2380,-20 (-> scanner 1 -> scanner 0)
        // - vectorToBeAligned = -626, 468, -788}
        // - alignedRotation = x, y, z
        // - alignedVector = -466, -666, -811}
        // - alignment.rotation = -x, y, -z
        // - alignment.movement = 68,-1246,-43

        // 4 is at -20,-1133,1061 (-> scanner 1 -> scanner 0)
        // - vectorToBeAligned = 416, -9, 1308
        // - alignedRotation = y, -z, -x
        // - alignedVector = -364, -763, -893
        // - alignment.rotation = -x, y, -z
        // - alignment.movement = 68,-1246,-43

        // now we can calculate translation
        Vector movement = alignment.rotation.transform(alignedRotation.transform(vectorToBeAligned.negate()));
        movement = movement.add(alignment.rotation.transform(alignedVector));
        movement = alignment.applyMovement(movement);

        return new Alignment(v -> alignment.rotation.transform(alignedRotation.transform(v)), movement);
    }

    private boolean areListsEqual(List<VectorBetweenVectors> list1, List<Vector> list2) {
        for (VectorBetweenVectors element : list1) {
            if (list2.stream().noneMatch(e -> e.AreCoordinatesEqual(element)))
                return false;
        }
        for (Vector element : list2) {
            if (list1.stream().noneMatch(e -> e.AreCoordinatesEqual(element)))
                return false;
        }
        return true;
    }

    public Vector[] mergeIntoOneCoordinateSystem() {
        Set<Vector> results = new HashSet<>();
        Map<Scanner, Alignment> scannerAlignments = alignScanners();
        for (Map.Entry<Scanner, Alignment> entry : scannerAlignments.entrySet()) {
            for (Vector point : entry.getKey().points) {
                results.add(entry.getValue().rotation.transform(point).add(entry.getValue().movement));
            }
        }
        return results.toArray(new Vector[0]);
    }

    public int calculateManhattenDistance() {
        var result = 0;
        Map<Scanner, Alignment> scannerAlignments = alignScanners();
        for (Map.Entry<Scanner, Alignment> entry : scannerAlignments.entrySet()) {
            for (Map.Entry<Scanner, Alignment> other : scannerAlignments.entrySet()) {
                if (entry != other) {
                    Vector vector = entry.getValue().movement;
                    Vector otherVector = other.getValue().movement;
                    result = Math.max(result, Math.abs(vector.x - otherVector.x) + Math.abs(vector.y - otherVector.y) + Math.abs(vector.z - otherVector.z));
                }
            }
        }
        return result;
    }

}
