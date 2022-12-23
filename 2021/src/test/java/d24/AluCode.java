package d24;

import org.junit.Test;

public class AluCode {

    @Test
    public int execute(int[] input) {
        int w = 0;
        int x = 0;
        int y = 0;
        int z = 0;

        // here comes what the ALU does
        // ----------------------------
        w = input[0]; // is 1 on default
        // x = x * 0; // is always 0
        // x = x + z; // is always 0
        // x = x % 26; // is always 0
        // z = z / 1; // is always 0
        // x = x + 11; // these and the next line are  always 0, since w is a single digit
        // x = x == w ? 1 : 0;
        // x = 0; // this and the next line are always 1
        // x = x == 0 ? 1 : 0;
        x = 1;
        // y = y * 0; // is always 0
        //y = 25; // y = y + 25; // set below
        // y = y * x; // is always 25
        // y = y + 1; // is always 26, but set below
        // z = z * y; // is always 0
        // y = 0; // y = y * 0; // is always 0
        y = w; // y = y + w; // is always w
        y = y + 1;
        // y = y * x; // is always y
        z = z + y; // --------------------------------------------------------------------------AT THIS POINT, Z = W + 1

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

    public int executeOptimized(int[] input) {
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
