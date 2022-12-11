package d16;

public class GreaterThanOperator extends Operator {

    static final int TypeId =5;

    public GreaterThanOperator(BinaryStream stream) {
        super(stream);
    }

    @Override
    public long calculateValue() {
        return getSubPackets()[0].calculateValue() > getSubPackets()[1].calculateValue() ? 1L : 0L;
    }
}
