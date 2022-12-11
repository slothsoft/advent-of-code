package d16;

public interface Packet {

    int BINARY = 2;

    static Packet parse(BinaryStream stream) {
        String versionAndType = stream.peak(6);
        int typeId = Integer.parseInt(versionAndType.substring(3), BINARY);

        switch (typeId) {
            case EqualsOperator.TypeId:
                return new EqualsOperator(stream);
            case GreaterThanOperator.TypeId:
                return new GreaterThanOperator(stream);
            case LessThanOperator.TypeId:
                return new LessThanOperator(stream);
            case LiteralValue.TypeId:
                return new LiteralValue(stream);
            case MaxOperator.TypeId:
                return new MaxOperator(stream);
            case MinOperator.TypeId:
                return new MinOperator(stream);
            case ProductOperator.TypeId:
                return new ProductOperator(stream);
            case SumOperator.TypeId:
                return new SumOperator(stream);
            default:
                throw new IllegalArgumentException("Cannot parse type ID " + typeId + " of binary: " + stream.peak(16));
        }
    }

    int getVersion();
    int getTypeId();
    int calculateVersion();
    long calculateValue();
}
