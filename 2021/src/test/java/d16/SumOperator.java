package d16;

import java.util.Arrays;

public class SumOperator extends Operator {

    static final int TypeId = 0;

    public SumOperator(BinaryStream stream) {
        super(stream);
    }

    @Override
    public long calculateValue() {
        return Arrays.stream(getSubPackets()).mapToLong(Packet::calculateValue).sum();
    }
}
