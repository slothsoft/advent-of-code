package d24;

public class AluCode {

    public int executeOptimized(int[] input) {
        int w = 0;
        int x = 0;
        int y = 0;
        int z = 0;

        // here comes what the ALU does
        // ----------------------------
        w = input[0]; // is 1 on default
        x = 11 == w ? 0 : 1;
        y = (w + 1) * x;
        z = y; // ------------------ for w == single digit -> x = 1, y = w + 1, z = w + 1

        w = input[1]; // is 3 on default
        x = z % 26 + 11;
        x = x == w ? 0 : 1;
        y = 25 * x + 1;
        z = z * y;
        y = (w + 11) * x;
        z = z + y; // ------------------ for w == single digit -> x = 1, y = w + 1, z = z * 26 + w + 1

        w = input[2]; // is 5 on default
        x = z % 26 + 14;
        x = x == w ? 0 : 1;
        y = 25 * x + 1;
        z = z * y;
        y = (w + 1) * x;
        z = z + y;  // ------------------ for w == single digit -> x = 1, y = w + 1, z = z * 26 + w + 1

        w = input[3]; // is 7 on default
        x = z % 26 + 11;
        x = x == w ? 0 : 1;
        y = 25 * x + 1;
        z = z * y;
        y = (w + 11) * x;
        z = z + y; // ------------------ for w == single digit -> x = 1, y = w + 1, z = z * 26 + w + 1

        w = input[4]; // is 9 on default
        x = z % 26 - 8;
        z = z / 26;
        x = x == w ? 0 : 1;
        y = 25 * x + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 2;
        y = y * x;
        z = z + y;

        w = input[5]; // is 2 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 26;
        x = x + -5;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 9;
        y = y * x;
        z = z + y;

        w = input[6]; // is 4 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 1;
        x = x + 11;

        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 7;
        y = y * x;
        z = z + y;
        w = input[7]; // is 6 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 26;
        x = x + -13;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 11;
        y = y * x;
        z = z + y;
        w = input[8]; // is 8 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 1;
        x = x + 12;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 6;
        y = y * x;
        z = z + y;
        w = input[9]; // is 9 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 26;
        x = x + -1;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 15;
        y = y * x;
        z = z + y;
        w = input[10]; // is 9 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 1;
        x = x + 14;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 7;
        y = y * x;
        z = z + y;
        w = input[11]; // is 9 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 26;
        x = x + -5;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 1;
        y = y * x;
        z = z + y;
        w = input[12]; // is 9 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 26;
        x = x + -4;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 8;
        y = y * x;
        z = z + y;
        w = input[13]; // is 9 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 26;
        x = x + -8;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 6;
        y = y * x;
        z = z + y;
        // ----------------------------
        // end of ALU stuff
        return z;
    }

    public int execute(int[] input) {
        int w = 0;
        int x = 0;
        int y = 0;
        int z = 0;

        // here comes what the ALU does
        // ----------------------------
        w = input[0]; // is 1 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 1;
        x = x + 11;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 1;
        y = y * x;
        z = z + y;
        w = input[1]; // is 3 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 1;
        x = x + 11;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 11;
        y = y * x;
        z = z + y;
        w = input[2]; // is 5 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 1;
        x = x + 14;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 1;
        y = y * x;
        z = z + y;
        w = input[3]; // is 7 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 1;
        x = x + 11;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 11;
        y = y * x;
        z = z + y;
        w = input[4]; // is 9 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 26;
        x = x + -8;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 2;
        y = y * x;
        z = z + y;
        w = input[5]; // is 2 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 26;
        x = x + -5;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 9;
        y = y * x;
        z = z + y;
        w = input[6]; // is 4 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 1;
        x = x + 11;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 7;
        y = y * x;
        z = z + y;
        w = input[7]; // is 6 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 26;
        x = x + -13;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 11;
        y = y * x;
        z = z + y;
        w = input[8]; // is 8 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 1;
        x = x + 12;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 6;
        y = y * x;
        z = z + y;
        w = input[9]; // is 9 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 26;
        x = x + -1;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 15;
        y = y * x;
        z = z + y;
        w = input[10]; // is 9 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 1;
        x = x + 14;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 7;
        y = y * x;
        z = z + y;
        w = input[11]; // is 9 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 26;
        x = x + -5;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 1;
        y = y * x;
        z = z + y;
        w = input[12]; // is 9 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 26;
        x = x + -4;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 8;
        y = y * x;
        z = z + y;
        w = input[13]; // is 9 on default
        x = x * 0;
        x = x + z;
        x = x % 26;
        z = z / 26;
        x = x + -8;
        x = x == w ? 1 : 0;
        x = x == 0 ? 1 : 0;
        y = y * 0;
        y = y + 25;
        y = y * x;
        y = y + 1;
        z = z * y;
        y = y * 0;
        y = y + w;
        y = y + 6;
        y = y * x;
        z = z + y;
        // ----------------------------
        // end of ALU stuff
        return z;
    }
}
