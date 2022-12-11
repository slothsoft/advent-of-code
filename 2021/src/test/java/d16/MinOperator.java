package d16;

import java.util.Arrays;

public class MinOperator extends Operator {

    static final int TypeId = 2;

    public MinOperator(BinaryStream stream) {
        super(stream);
    }

    @Override
    public long calculateValue() {
        return Arrays.stream(getSubPackets()).mapToLong(Packet::calculateValue).min().orElseThrow();
    }
}
