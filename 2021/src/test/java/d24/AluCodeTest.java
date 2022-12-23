package d24;

import org.junit.Assert;
import org.junit.Test;

public class AluCodeTest {

    @Test
    public void testExecute() {
        AluCode aluCode = new AluCode();
        System.out.println(aluCode.execute(new int[]{1, 3, 5, 7, 9, 2, 4, 6, 8, 9, 9, 9, 9, 9}));
    }
}
