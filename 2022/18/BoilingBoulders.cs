namespace AoC._18;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/18">Day 18: Boiling Boulders</a>
/// </summary>
public class BoilingBoulders {
    private const int Exterior = 1;
    private const int Lava = 2;

    private readonly bool[][][] _cubes;

    public BoilingBoulders(string[] lines, int maxSize) {
        const int border = 2;
        _cubes = new bool[maxSize + border][][];
        for (var x = 0; x < maxSize + border; x++) {
            _cubes[x] = new bool[maxSize + border][];
            for (var y = 0; y < maxSize + border; y++) {
                _cubes[x][y] = new bool[maxSize + border];
            }
        }

        foreach (var line in lines) {
            var split = line.Split(",");
            _cubes[int.Parse(split[0])][int.Parse(split[1])][int.Parse(split[2])] = true;
        }
    }

    public int CalculateSurfaceArea() {
        var result = 0;
        for (var x = 0; x < _cubes.Length; x++) {
            for (var y = 0; y < _cubes[x].Length; y++) {
                for (var z = 0; z < _cubes[x][y].Length; z++) {
                    result += CalculateSurfaceArea(x, y, z);
                }
            }
        }

        return result;
    }

    private int CalculateSurfaceArea(int x, int y, int z) {
        if (!_cubes[x][y][z]) {
            return 0;
        }

        var result = 0;
        var plus = new[] {-1, 1};
        foreach (var xPlus in plus) {
            if (x + xPlus < 0 || !_cubes[x + xPlus][y][z]) result++;
        }

        foreach (var yPlus in plus) {
            if (y + yPlus < 0 || !_cubes[x][y + yPlus][z]) result++;
        }

        foreach (var zPlus in plus) {
            if (z + zPlus < 0 || !_cubes[x][y][z + zPlus]) result++;
        }

        return result;
    }

    public int CalculateExteriorSurfaceArea() {
        var cubeTypes = CreateCubesByType();
        
        var result = 0;
        for (var x = 0; x < cubeTypes.Length; x++) {
            for (var y = 0; y < cubeTypes[x].Length; y++) {
                for (var z = 0; z < cubeTypes[x][y].Length; z++) {
                    result += CalculateExteriorSurfaceArea(x, y, z, cubeTypes);
                }
            }
        }

        return result;
    }

    private int[][][] CreateCubesByType() {
        var result = new int[_cubes.Length][][];
        for (var x = 0; x < result.Length; x++) {
            result[x] = new int[_cubes[x].Length][];
            for (var y = 0; y < _cubes[x].Length; y++) {
                result[x][y] = new int[_cubes[x][y].Length];
            }
        }

        bool somethingWasChanged;
        do {
            somethingWasChanged = false;
            for (var x = 0; x < result.Length; x++) {
                for (var y = 0; y < result[x].Length; y++) {
                    for (var z = 0; z < result[x][y].Length; z++) {
                        if (result[x][y][z] != 0) continue;

                        if (_cubes[x][y][z]) {
                            // it's lava, baby
                            result[x][y][z] = Lava;
                            somethingWasChanged = true;
                        } else if (x * y * z == 0 || x == result.Length - 1 || y == result[x].Length - 1 || z == result[x][y].Length - 1) {
                            // the outside of the 3D array
                            result[x][y][z] = Exterior;
                            somethingWasChanged = true;
                        } else if (IsNextToExterior(x, y, z, result)) {
                            // so if an adjecent field is exterior, so is this
                            result[x][y][z] = Exterior;
                            somethingWasChanged = true;
                        }
                    }
                }
            }
        } while (somethingWasChanged);

        return result;
    }

    private static bool IsNextToExterior(int x, int y, int z, int[][][] cubeTypes) {
        var plus = new[] {-1, 1};
        foreach (var xPlus in plus) {
            if (x + xPlus < 0 || cubeTypes[x + xPlus][y][z] == Exterior) return true;
        }

        foreach (var yPlus in plus) {
            if (y + yPlus < 0 || cubeTypes[x][y + yPlus][z] == Exterior) return true;
        }

        foreach (var zPlus in plus) {
            if (z + zPlus < 0 || cubeTypes[x][y][z + zPlus] == Exterior) return true;
        }

        return false;
    }
    
    private static int CalculateExteriorSurfaceArea(int x, int y, int z, int[][][] cubeTypes) {
        if (cubeTypes[x][y][z] != Lava) {
            return 0;
        }

        var result = 0;
        var plus = new[] {-1, 1};
        foreach (var xPlus in plus) {
            if (x + xPlus < 0 || cubeTypes[x + xPlus][y][z] == Exterior) result++;
        }

        foreach (var yPlus in plus) {
            if (y + yPlus < 0 || cubeTypes[x][y + yPlus][z] == Exterior) result++;
        }

        foreach (var zPlus in plus) {
            if (z + zPlus < 0 || cubeTypes[x][y][z + zPlus] == Exterior) result++;
        }

        return result;
    }
}