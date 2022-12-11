package d16;

import java.util.Arrays;

public class MaxOperator extends Operator {

    static final int TypeId = 3;

    public MaxOperator(BinaryStream stream) {
        super(stream);
    }

    @Override
    public long calculateValue() {
        return Arrays.stream(getSubPackets()).mapToLong(Packet::calculateValue).max().orElseThrow();
    }
}
