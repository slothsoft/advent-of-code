package d24;

import org.junit.Test;

public class AluCodeTest {

    @Test
    public void testExecute() {
        int[] result = new int[14];
        int log = 1000000000;
        AluCode aluCode = new AluCode();
        for (long i = 99_790_000_000_000L /* 99_999_999_999_999L */; i >= 10_000_000_000_000L; i--) {
            if (String.valueOf(i).contains("0")) {
                continue;
            }

            convertSerialNumberToArray(i, result);

            int z = aluCode.execute(result);
            if (z == 1) {
                System.out.println("Correct answer: " + i);
                break;
            }
            if (i % log == log - 1) {
                System.out.println("Tested until " + (i + 1) + " - " + z);
            }
        }
    }

    static void convertSerialNumberToArray(long serialNumber, int[] result) {
        for (int i = 0; i < result.length; i++) {
            result[result.length - i - 1] = (int) (serialNumber % 10);
            serialNumber /= 10;
        }
    }
}
