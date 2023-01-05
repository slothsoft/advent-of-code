package d16;

import java.util.Arrays;

public class ProductOperator extends Operator {

    static final int TypeId = 1;

    public ProductOperator(BinaryStream stream) {
        super(stream);
    }

    @Override
    public long calculateValue() {
        return Arrays.stream(getSubPackets()).mapToLong(Packet::calculateValue).reduce(1, (x, y) -> x * y);
    }
}
